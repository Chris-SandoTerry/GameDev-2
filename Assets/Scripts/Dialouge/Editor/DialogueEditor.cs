using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class DialogueEditor : EditorWindow
{
   Dialogue _selectDialouge = null;
   GUIStyle _nodeStyle;
   DialogueNode _draggingNode = null;
   Vector2 _draggingOffset;
    
   [MenuItem("Window/Dialogue Editor")]
   public static void ShowEditorWindow()
   {
      GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
   }

   [OnOpenAsset(1)]
   public static bool OpenDialogue(int instanceID, int line)
   {
      Dialogue dialouge = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
      if (dialouge != null)
      {
         ShowEditorWindow();
         return true;
      }

      return false;
   }

    void OnEnable()
    {
       Selection.selectionChanged += OnSelectChanged;
       _nodeStyle = new GUIStyle();
       _nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
       _nodeStyle.normal.textColor = Color.white;
       _nodeStyle.padding = new RectOffset(20, 20, 20, 20);
       _nodeStyle.border = new RectOffset(12, 12, 12, 12);
    }

     void OnSelectChanged()
    {
      Dialogue newDialogue = Selection.activeObject as Dialogue;
      if (newDialogue != null)
      {
         _selectDialouge = newDialogue;
         Repaint();
      }
    }

    void OnGUI()
   {
      if (_selectDialouge == null)
      {
         EditorGUILayout.LabelField("No Dialogue Selected");
      }
      else
      {
         ProcessEvents();
         foreach (DialogueNode node in _selectDialouge.GetAllNodes())
         {
            OnGUINode(node);
         }
      }
   }

    void ProcessEvents()
    {
       if (Event.current.type == EventType.MouseDown && _draggingNode == null)
       {
          _draggingNode = GetNodeAtPoint(Event.current.mousePosition);
          if (_draggingNode != null)
          {
             _draggingOffset = _draggingNode.Rect.position - Event.current.mousePosition;
          }
       }
       else if(Event.current.type == EventType.MouseDrag && _draggingNode != null)
       {
          Undo.RecordObject(_selectDialouge,"Move Dialogue Node");
          _draggingNode.Rect.position = Event.current.mousePosition + _draggingOffset;
          GUI.changed = true;
       }
       else if (Event.current.type == EventType.MouseUp && _draggingNode != null)
       {
          _draggingNode = null;
       }
    }

    DialogueNode GetNodeAtPoint(Vector2 point)
    {
       DialogueNode foundNode = null;
       foreach (DialogueNode node in _selectDialouge.GetAllNodes())
       {
          if (node.Rect.Contains(point))
          {
             foundNode = node;
          }
       }

       return foundNode;
    }

    private void OnGUINode(DialogueNode node)
    {
       GUILayout.BeginArea(node.Rect, _nodeStyle);
       EditorGUI.BeginChangeCheck();
       EditorGUILayout.LabelField("Node:", EditorStyles.whiteLabel);
       string newText = EditorGUILayout.TextField(node.Text);
       string newUniqueID = EditorGUILayout.TextField(node.UniqueID);
       if (EditorGUI.EndChangeCheck())
       {
          Undo.RecordObject(_selectDialouge, "Update Dialogue Text");
          node.Text = newText;
          node.UniqueID = newUniqueID;
       }

       foreach (DialogueNode childNode in _selectDialouge.GetAllChildren(node))
       {
          EditorGUILayout.LabelField(childNode.Text);
       }
       
       GUILayout.EndArea();
    }
}
