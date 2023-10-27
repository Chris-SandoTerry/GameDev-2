using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu (fileName = "NewDialogue", menuName = "Dialogue", order = 0)]
public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
{
   [SerializeField] List<DialogueNode> _nodes = new List<DialogueNode>();

   static Vector2 _newNodeOffset = new Vector2(250, 0);

   Dictionary<string, DialogueNode> _nodeLookup = new Dictionary<string, DialogueNode>();

   void OnValidate()
   {
      _nodeLookup.Clear();
      foreach (DialogueNode node in GetAllNodes())
      {
         _nodeLookup[node.name] = node;
      }
   }
   

   public IEnumerable<DialogueNode> GetAllNodes()
   {
      return _nodes;
   }

   public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
   {
      foreach (string childID in parentNode.GetChildren())
      {
         if (_nodeLookup.ContainsKey(childID))
         {
            yield return _nodeLookup[childID];
         }
      }
   }

#if UNITY_EDITOR

   public void CreateNode(DialogueNode parent)
   {
      DialogueNode newNode = MakeNode(parent);
      Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
      Undo.RecordObject(this, "Added Dialogue Node");
      AddNode(newNode);
      
   }

   public void DeleteNode(DialogueNode parent)
   {
      Undo.RecordObject(this, "Deleted Dialogue Node");
      _nodes.Remove(parent);
      OnValidate();
      CleanDanglingChildren(parent);
      Undo.DestroyObjectImmediate(parent);
   }

   static DialogueNode MakeNode(DialogueNode parent)
   {
      DialogueNode newNode = CreateInstance<DialogueNode>();
      newNode.name = Guid.NewGuid().ToString();
      if (parent != null)
      {
         parent.AddChild(newNode.name);
         newNode.SetIsPlayer(!parent.IsPlayer());
         newNode.SetPosition(parent.GetRect().position + _newNodeOffset);
      }

      return newNode;
   }
   
   void  AddNode(DialogueNode newNode)
   {
      _nodes.Add(newNode);
      OnValidate();
   }

   void CleanDanglingChildren(DialogueNode nodeToDelete)
   {
      foreach (DialogueNode node in GetAllNodes())
      {
         node.RemoveChild(nodeToDelete.name);
      }
   }
#endif
   public void OnBeforeSerialize()
   {
#if UNITY_EDITOR
      if (_nodes.Count == 0)
      {
         DialogueNode newNode = MakeNode(null);
         AddNode(newNode);
      }

      if (AssetDatabase.GetAssetPath(this) != "")
      {
         foreach (DialogueNode node in GetAllNodes())
         {
            if (AssetDatabase.GetAssetPath(node) == "")
            {
               AssetDatabase.AddObjectToAsset(node,this);
            }
         }
      }
#endif
   }

   public void OnAfterDeserialize()
   {
     
   }
}
