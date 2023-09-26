using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCharacterAnims;
using RPGCharacterAnims.Actions;
using RPGCharacterAnims.Lookups;
using UnityEngine.AI;


[RequireComponent(typeof(RPGCharacterController))]
[RequireComponent(typeof(RPGCharacterNavigationController))]

public class Enemy : MonoBehaviour
{
    [SerializeField]  float _detectionRange = 15f;
    [SerializeField]  float _attackRange = 2f;
    [SerializeField]  float _rotationSpeed = 1f;
    [SerializeField] float _timeBetweenAttacks = 3f;
    [SerializeField] int _damage = 10;
    

    RPGCharacterController _rpgCharacterController;
     RPGCharacterNavigationController _rpgNavigationController; 
    NavMeshAgent _navMeshAgent;
    Health _health;
    GameObject _player;
     Health _playerHealth;
    Vector3 _targetPosition;
    Vector3 _originPosition;
   bool _aggro = false;
    float _timeSinceLastAttack = 0f;


   private void Awake()
   {
       _rpgCharacterController = GetComponent<RPGCharacterController>();
       _rpgNavigationController = GetComponent<RPGCharacterNavigationController>();
       _navMeshAgent = GetComponent<NavMeshAgent>();
       _health = GetComponent<Health>();
   }

   void Start()
   {
       _player = GameObject.FindGameObjectWithTag("Player");
       _playerHealth = _player.GetComponent<Health>();
       _rpgCharacterController.target = _player.transform;
       _originPosition = transform.position;
   }

   
    void Update()
    {
        if (!_health.IsAlive()) return;
        
        if (InDetectionRange())
        {
            _aggro = true;
            _targetPosition = _rpgCharacterController.target.transform.position;
            if (!InAttackRange())
            {
                _rpgCharacterController.StartAction(HandlerTypes.Navigation, _targetPosition);
                _timeSinceLastAttack = 0;
            }
            else
            {
                _rpgNavigationController.StopNavigating();
                _rpgNavigationController.StopAnimation();
                RotateTowardsTarget();
                _timeSinceLastAttack += Time.deltaTime;
                if (_timeSinceLastAttack >= _timeBetweenAttacks)
                {
                    Attack();
                    _timeSinceLastAttack = 0;
                }
            }
        }
        else if (_aggro)
        {
            _aggro = false;
            Reset();
        }
    }

    bool InDetectionRange()
    {
        if (!_playerHealth.IsAlive()) return false;
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

    void Attack()
    {
        _rpgCharacterController.StartAction(HandlerTypes.Attack, new AttackContext(HandlerTypes.Attack, Side.Right));
        _playerHealth.DealDamage(_damage);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, _detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _attackRange);
        
    }
}
