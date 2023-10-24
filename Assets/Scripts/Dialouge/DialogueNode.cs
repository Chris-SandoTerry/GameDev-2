using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueNode : ScriptableObject
{
    [SerializeField] string _text;
    [SerializeField] List<string> _children = new List<string>();
    [SerializeField] Rect _rect = new Rect(0, 0, 200, 100);

    public Rect GetRect()
    {
        return _rect;
    }

    public string GetText()
    {
        return _text;
    }

    public List<string> GetChildren()
    {
        return _children;
    }

    #if UNITY_EDITOR
    public void SetPosition(Vector2 newPosition)
    {
        Undo.RecordObject(this, "Move Dialogue Node");
        _rect.position = newPosition;
        EditorUtility.SetDirty(this);
    }

    public void SetText(string newText)
    {
        if (newText != _text)
        {
            Undo.RecordObject(this,"Update Dialogue Text");
            _text = newText;
            EditorUtility.SetDirty(this);
        }
    }

    public void AddChild(string childID)
    {
        Undo.RecordObject(this, "Add Dialogue Link");
        _children.Add(childID);
        EditorUtility.SetDirty(this);
    }
    
    public void RemoveChild(string childID)
    {
        Undo.RecordObject(this, "Remove Dialogue Link");
        _children.Remove(childID);
        EditorUtility.SetDirty(this);
    }
    
#endif
}
