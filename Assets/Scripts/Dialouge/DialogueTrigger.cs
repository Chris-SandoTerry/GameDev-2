using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
  [SerializeField]  string _action;
  [SerializeField]  UnityEvent _onTrigger;

  public void Trigger(string actionToTrigger)
  {
      if (actionToTrigger == _action)
      {
          _onTrigger.Invoke();
      }
  }
}
