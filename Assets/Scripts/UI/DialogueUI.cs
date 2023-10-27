using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField]  TMP_Text _text;
    PlayerDialogue _playerDialogue;
    
    void Start()
    {
        _playerDialogue = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogue>();
        _text.text = _playerDialogue.GetText();
    }

   
    void Update()
    {
        
    }
}
