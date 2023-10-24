using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class DialogueEditor : EditorWindow
{
   Dialogue _selectDialouge = null;
   GUIStyle _nodeStyle;
   DialogueNode _draggingNode = null;
   DialogueNode _creatingNode = null;
   DialogueNode _deletingNode = null;
   DialogueNode _linkingNode = null;
   Vector2 _draggingOffset;
   Vector2 _scrollPosition;
   bool _draggingCanvas = false;
   Vector2 _draggingCanvasOffset;
    
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

         _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

         GUILayoutUtility.GetRect(4000, 4000);
         
         
         foreach (DialogueNode node in _selectDialouge.GetAllNodes())
         {
            DrawConnections(node);
         }
         foreach (DialogueNode node in _selectDialouge.GetAllNodes())
         {
            DrawNode(node);
         }
         
         EditorGUILayout.EndScrollView();

         if (_creatingNode != null)
         {
            _selectDialouge.CreateNode(_creatingNode);
            _creatingNode = null;
         }
         
         if (_deletingNode != null)
         {
            _selectDialouge.DeleteNode(_deletingNode);
            _deletingNode = null;
         }
      }
   }

    void ProcessEvents()
    {
       if (Event.current.type == EventType.MouseDown && _draggingNode == null)
       {
          _draggingNode = GetNodeAtPoint(Event.current.mousePosition + _scrollPosition);
          if (_draggingNode != null)
          {
             _draggingOffset = _draggingNode.GetRect().position - Event.current.mousePosition;
             Selection.activeObject = _draggingNode;
          }
          else
          {
             _draggingCanvas = true;
             _draggingCanvasOffset = Event.current.mousePosition + _scrollPosition;
             Selection.activeObject = _selectDialouge;
          }
       }
       else if(Event.current.type == EventType.MouseDrag && _draggingNode != null)
       {
          Undo.RecordObject(_selectDialouge,"Move Dialogue Node");
          _draggingNode.SetPosition(Event.current.mousePosition + _draggingOffset);
          GUI.changed = true;
       }
       else if (Event.current.type == EventType.MouseDrag && _draggingCanvas)
       {
          _scrollPosition = _draggingCanvasOffset - Event.current.mousePosition;
          GUI.changed = true;
       }
       else if (Event.current.type == EventType.MouseUp && _draggingNode != null)
       {
          _draggingNode = null;
       }
       else if (Event.current.type == EventType.MouseUp && _draggingCanvas)
       { 
          _draggingCanvas = false;
       }
    }

    DialogueNode GetNodeAtPoint(Vector2 point)
    {
       DialogueNode foundNode = null;
       foreach (DialogueNode node in _selectDialouge.GetAllNodes())
       {
          if (node.GetRect().Contains(point))
          {
             foundNode = node;
          }
       }

       return foundNode;
    }

    private void DrawNode(DialogueNode node)
    {
       GUILayout.BeginArea(node.GetRect(), _nodeStyle);
     
       node.SetText(EditorGUILayout.TextField(node.GetText()));

       GUILayout.BeginHorizontal();
       if (GUILayout.Button("+"))
       {
          _creatingNode = node;
       }

       DrawLinkButtons(node);
       
       
       if (GUILayout.Button("-"))
       {
          _deletingNode = node;
       }
       GUILayout.EndHorizontal();


       GUILayout.EndArea();
    }

    void DrawLinkButtons(DialogueNode node)
    {
       if (_linkingNode == null)
       {
          if (GUILayout.Button("Link"))
          {
             _linkingNode = node;
          }
       }
       else if (_linkingNode == node)
       {
          if (GUILayout.Button("Cancel"))
          {
             _linkingNode = null;
          }
       }
       else if (_linkingNode.GetChildren().Contains(node.name))
       {
          if (GUILayout.Button("Unlink"))
          {
             Undo.RecordObject(_selectDialouge,"Remove Dialogue Link");
             _linkingNode.RemoveChild(node.name);
             _linkingNode = null;
          }
       }
       else
       {
          if (GUILayout.Button("Child"))
          {
             Undo.RecordObject(_selectDialouge, "Add Dialogue Link");
             _linkingNode.AddChild(node.name);
             _linkingNode = null;
          }
       }
    }

    void DrawConnections(DialogueNode node)
    {
       Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
       foreach (DialogueNode childNode in _selectDialouge.GetAllChildren(node))
       {
          Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
          Vector3 controlPointOffset = endPosition - startPosition;
          controlPointOffset.y = 0;
          controlPointOffset.x *= .8f;
          Handles.DrawBezier(startPosition,endPosition,startPosition+controlPointOffset,endPosition-controlPointOffset,Color.white,null,4f);
       }
    }
}
