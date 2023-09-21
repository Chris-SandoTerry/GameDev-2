using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCharacterAnims;
using RPGCharacterAnims.Lookups;
using UnityEngine.AI;


[RequireComponent(typeof(RPGCharacterController))]
[RequireComponent(typeof(RPGCharacterNavigationController))]

public class Enemy : MonoBehaviour
{
    [SerializeField]  float _detectionRange = 15f;
    [SerializeField]  float _attackRange = 2f;
    [SerializeField]  float _rotationSpeed = 1f;

    RPGCharacterController _rpgCharacterController;
     RPGCharacterNavigationController _rpgNavigationController; 
    NavMeshAgent _navMeshAgent;

    GameObject _player;
    Vector3 _targetPosition;
    Vector3 _originPosition;
   bool aggro = false;


   private void Awake()
   {
       _rpgCharacterController = GetComponent<RPGCharacterController>();
       _rpgNavigationController = GetComponent<RPGCharacterNavigationController>();
       _navMeshAgent = GetComponent<NavMeshAgent>();
   }

   void Start()
   {
       _player = GameObject.FindGameObjectWithTag("Player");
       _rpgCharacterController.target = _player.transform;
       _originPosition = transform.position;
   }

   
    void Update()
    {
        if (InDetectionRange())
        {
            aggro = true;
            _targetPosition = _rpgCharacterController.target.transform.position;
            if (!InAttackRange())
            {
                _rpgCharacterController.StartAction(HandlerTypes.Navigation, _targetPosition);
            }
            else
            {
                _rpgNavigationController.StopNavigating();
                _rpgNavigationController.StopAnimation();
                RotateTowardsTarget();
            }
        }
        else if (aggro)
        {
            aggro = false;
            Reset();
        }
    }

    bool InDetectionRange()
    {
        float playerDistanceSqr = (_player.transform.position - transform.position).sqrMagnitude;

        return playerDistanceSqr <= _detectionRange * _detectionRange;
    }

    bool InAttackRange()
    {
        float playerDistanceSqr = (_player.transform.position - transform.position).sqrMagnitude;

        return playerDistanceSqr <= _attackRange * _attackRange;
    }

    void RotateTowardsTarget()
    {
        Vector3 direction = (_targetPosition - transform.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 
            Time.deltaTime *_navMeshAgent.angularSpeed * _rotationSpeed);
    }

    public void Reset()
    {
        _rpgCharacterController.StartAction(HandlerTypes.Navigation, _originPosition);
    }


     void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, _detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _attackRange);
        
    }
}
