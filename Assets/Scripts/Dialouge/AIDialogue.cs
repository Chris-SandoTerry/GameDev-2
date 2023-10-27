using UnityEngine;

public class AIDialogue : MonoBehaviour
{
 [SerializeField]  Dialogue _dialogue = null;
 [SerializeField]  string _name;

  GameObject _player;

   void Start()
  {
      _player = GameObject.FindGameObjectWithTag("Player");
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
}
