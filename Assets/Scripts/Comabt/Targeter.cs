using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField]CinemachineTargetGroup _targetGroup;
    [SerializeField] private CinemachineVirtualCamera _targetingCamera;
    
    public Target CurrentTarget {get; private set;}

    public List<Target> _targets = new List<Target>();

    Camera _mainCamera;
    PlayerInputSystemController _playerInputController;


    void Awake()
    {
        _playerInputController = GetComponent<PlayerInputSystemController>();
    }

    void Start()
    {
        _mainCamera = Camera.main;
    }


    void OnTriggerEnter(Collider other)
    {
      if(!other.TryGetComponent<Target>(out Target target)) return;
      _targets.Add(target);
      target.OnDestroyed += RemoveTarget;

    }

     void OnTriggerExit(Collider other)
     {
         if(!other.TryGetComponent<Target>(out Target target)) return;
         RemoveTarget(target);

     }

     void RemoveTarget(Target target)
     {
         if (CurrentTarget == target)
         {
             _targetingCamera.Priority = 9;
             _targetGroup.RemoveMember(CurrentTarget.transform);
             CurrentTarget = null;
             _playerInputController.CancelInputAim();
         }

         target.OnDestroyed -= RemoveTarget;
         _targets.Remove(target);
     }

     public bool SelectTarget()
     {
         if (_targets.Count == 0) return false;

         Target closestTarget = null;
         float closestDistance = Mathf.Infinity;

         foreach (Target target in _targets)
         {
             Vector2 _viewPos = _mainCamera.WorldToViewportPoint(target.transform.position);
             if (_viewPos.x < 0 || _viewPos.x > 1 || _viewPos.y < 0 || _viewPos.y > 1)
             {
                 continue;
             }
             
             Vector2 _toCenter = _viewPos - new Vector2(.5f,.5f);
             if (_toCenter.sqrMagnitude < closestDistance)
             {
                 closestTarget = target;
                 closestDistance = _toCenter.sqrMagnitude;
             }
         }

         if (closestTarget == null) return false;
         
         CurrentTarget = closestTarget;

         _targetGroup.AddMember(CurrentTarget.transform, 1f, 2f );
         _targetingCamera.Priority = 11;

         return true;
     }

     public void Cancel()
     {
         if (CurrentTarget == null) return;
         _targetingCamera.Priority = 9;
         _targetGroup.RemoveMember(CurrentTarget.transform);
         CurrentTarget = null;
     }

}
