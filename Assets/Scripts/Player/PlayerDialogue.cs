
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerDialogue : MonoBehaviour
{
    [SerializeField] private Dialogue _currentDialogue;
    private DialogueNode _currentNode = null;
    bool _choosing = false;

    private void Awake()
    {
        _currentNode = _currentDialogue.GetRootNode();
    }

    public bool IsChoosing()
    {
        return _choosing;
    }

    public string GetText()
    {
        if (_currentDialogue == null)
        {
            return "";
        }

        return _currentNode.GetText();
    }

    public IEnumerable<DialogueNode> GetChoices()
    {
        return _currentDialogue.GetPlayerChildren(_currentNode);
    }

    public void SelectChoice(DialogueNode chosenNode)
    {
        _currentNode = chosenNode;
        _choosing = false;
        Next();
    }

    public void Next()
    {
        int numPlayerResponses = _currentDialogue.GetPlayerChildren(_currentNode).Count();

        if (numPlayerResponses > 0)
        {
            _choosing = true;
            return;
        }

        DialogueNode[] children = _currentDialogue.GetAIChildren(_currentNode).ToArray();
        int randomInddex = Random.Range(0, children.Length);
        _currentNode = children[randomInddex];
    }

    public bool HasNext()
    {
        return _currentDialogue.GetAllChildren(_currentNode).Count() > 0;
    }

}
