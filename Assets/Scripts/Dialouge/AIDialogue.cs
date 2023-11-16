using System.Linq;
using UnityEngine;

public class AIDialogue : MonoBehaviour, IRaycastable
{
 [SerializeField]  Dialogue _dialogue = null;
 [SerializeField]  string _name;

  GameObject _player;
  PlayerInputSystemController _playerInputSystem;

   void Start()
  {
      _player = GameObject.FindGameObjectWithTag("Player");
      _playerInputSystem = FindObjectOfType<PlayerInputSystemController>();
  }

  public void StartDialogue()
  {
      if (_dialogue == null)
      {
          return;
      }
      _player.GetComponent<PlayerDialogue>().startDialogue(_dialogue, this);
  }

  public string GetName()
  {
      return _name;
  }
  
  public CursorType GetCursorType()
  {
      if (_dialogue.GetAllNodes().Count() > 0)
      {
          return CursorType.Dialogue;
      }
      else
      {
          return 0;
      }
  }
  
  public bool HandleRaycast(CursorController callingController)
  {
      if (_playerInputSystem._playerInputs.Player.Select.WasPressedThisFrame())
      {
         StartDialogue();
      }
      return true;
  }
}
