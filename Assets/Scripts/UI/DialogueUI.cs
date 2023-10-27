using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField]  TMP_Text _AItext;
    [SerializeField]  Button _nextButton;
    [SerializeField]  GameObject _AIresponse;
    [SerializeField]  Transform _choiceRoot;
    [SerializeField]  GameObject _choicePrefab;
    PlayerDialogue _playerDialogue;
    
    void Start()
    {
        _playerDialogue = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogue>();
        _nextButton.onClick.AddListener(Next);
        
        UpdateUI();
    }

     void Next()
    {
        _playerDialogue.Next();
        UpdateUI();
    }


    void UpdateUI()
    {
        _AIresponse.SetActive(!_playerDialogue.IsChoosing());
        _choiceRoot.gameObject.SetActive(_playerDialogue.IsChoosing());
        if (_playerDialogue.IsChoosing())
        {
            BuildChoiceList();
        }
        else
        {
            _AItext.text = _playerDialogue.GetText();
            _nextButton.gameObject.SetActive(_playerDialogue.HasNext());
        }
    }

    void BuildChoiceList()
    {
      _choiceRoot.DetachChildren();
      foreach (DialogueNode choice in _playerDialogue.GetChoices())
      {
          GameObject choiceInstance = Instantiate(_choicePrefab, _choiceRoot);
          var textComponet = choiceInstance.GetComponentInChildren<TMP_Text>();
          textComponet.text = choice.GetText();
          Button button = choiceInstance.GetComponentInChildren<Button>();
          button.onClick.AddListener(() =>
          {
            _playerDialogue.SelectChoice(choice);
            UpdateUI();
          });
      }
      
    }
}
