using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCharacterAnims;
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
   
   
   
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}
