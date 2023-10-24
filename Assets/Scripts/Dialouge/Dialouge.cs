using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu (fileName = "NewDialogue", menuName = "Dialogue", order = 0)]
public class Dialogue : ScriptableObject
{
   [SerializeField] List<DialogueNode> _nodes = new List<DialogueNode>();

   Dictionary<string, DialogueNode> _nodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
   void Awake()
   {
      if (_nodes.Count == 0)
      {
         _nodes.Add(new DialogueNode());
      }
   }
#endif

   void OnValidate()
   {
      _nodeLookup.Clear();
      foreach (DialogueNode node in GetAllNodes())
      {
         _nodeLookup[node.UniqueID] = node;
      }
   }
   

   public IEnumerable<DialogueNode> GetAllNodes()
   {
      return _nodes;
   }

   public DialogueNode GetRootNode()
   {
      return _nodes[0];
   }

   public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
   {
      foreach (string childID in parentNode.Children)
      {
         if (_nodeLookup.ContainsKey(childID))
         {
            yield return _nodeLookup[childID];
         }
      }
   }
}
