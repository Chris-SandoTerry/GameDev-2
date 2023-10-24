using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    public string UniqueID;
    public string Text;
    public List<string> Children = new List<string>();
    public Rect Rect = new Rect(0, 0, 200, 100);
}
