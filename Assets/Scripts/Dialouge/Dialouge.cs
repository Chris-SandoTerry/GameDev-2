using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu (fileName = "NewDialogue", menuName = "Dialogue", order = 0)]
public class Dialogue : ScriptableObject
{
   List<DialogueNode> _nodes = new List<DialogueNode>();

   Dictionary<string, DialogueNode> _nodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
   void Awake()
   {
      if (_nodes.Count == 0)
      {
         DialogueNode rootNode = new DialogueNode();
         rootNode.UniqueID = Guid.NewGuid().ToString();
         _nodes.Add(rootNode);
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


   public void CreateNode(DialogueNode parent)
   {
      DialogueNode newNode = new DialogueNode();
      newNode.UniqueID = Guid.NewGuid().ToString();
      parent.Children.Add(newNode.UniqueID);
      _nodes.Add(newNode);
      OnValidate();
   }

   public void DeleteNode(DialogueNode parent)
   {
      _nodes.Remove(parent);
      OnValidate();
      CleanDanglingChildren(parent);
   }

   void CleanDanglingChildren(DialogueNode nodeToDelete)
   {
      foreach (DialogueNode node in GetAllNodes())
      {
         node.Children.Remove(nodeToDelete.UniqueID);
      }
   }
}
