using System;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    public string UniqueID;
    public string Text;
    public string[] Children;
    public Rect Rect = new Rect(0, 0, 200, 100);
}
