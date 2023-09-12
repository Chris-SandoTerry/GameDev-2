using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField]CinemachineTargetGroup _targetGroup;
    [SerializeField] private CinemachineVirtualCamera _targetingCamera;
    public List<Target> _targets = new List<Target>();


     void OnTriggerEnter(Collider other)
    {
      if(!other.TryGetComponent<Target>(out Target target)) return;
      _targets.Add(target);

      
    }

     void OnTriggerExit(Collider other)
     {
         if(!other.TryGetComponent<Target>(out Target target)) return;
         _targets.Remove(target);

     }

     public bool HasTarget()
     {
         if (_targets.Count == 0)
         {
             return false;
         }

         return true;
     }

     public Target GetTarget()
     {
         return _targets[0];
     }
}
