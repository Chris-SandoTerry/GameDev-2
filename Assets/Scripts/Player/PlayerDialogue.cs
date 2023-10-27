using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogue : MonoBehaviour
{
    [SerializeField] private Dialogue _currentDialogue;

    public string GetText()
    {
        if (_currentDialogue == null)
        {
            return "";
        }

        return _currentDialogue.GetRootNode().GetText();
    }
}
