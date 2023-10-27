
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerDialogue : MonoBehaviour
{
    [SerializeField]  string _name;
    Dialogue _currentDialogue;
    DialogueNode _currentNode = null; 
    AIDialogue _currentAI = null;
    bool _choosing = false;

    public event Action onConcersationUpdated;
    
    public void startDialogue(Dialogue newDialogue, AIDialogue newAIDialogue)
    {
        _currentDialogue = newDialogue;
        _currentAI = newAIDialogue;
        _currentNode = _currentDialogue.GetRootNode();
        onConcersationUpdated();
    }

    public bool IsActive()
    {
        return _currentDialogue != null;
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

    public string GetCurrentName()
    {
        if (IsChoosing())
        {
            return _name;
        }
        else
        {
            return _currentAI.GetName();
        }
        
    }

    public void Next()
    {
        int numPlayerResponses = _currentDialogue.GetPlayerChildren(_currentNode).Count();

        if (numPlayerResponses > 0)
        {
            _choosing = true;
            onConcersationUpdated();
            return;
        }

        DialogueNode[] children = _currentDialogue.GetAIChildren(_currentNode).ToArray();
        int randomInddex = Random.Range(0, children.Length);
        _currentNode = children[randomInddex];
        onConcersationUpdated();
    }

    public bool HasNext()
    {
        return _currentDialogue.GetAllChildren(_currentNode).Count() > 0;
    }

    public void Quit()
    {
        _currentDialogue = null;
        _currentAI = null;
        _currentNode = null;
        _choosing = false;
        onConcersationUpdated();

    }

}
