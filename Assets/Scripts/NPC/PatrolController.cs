using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatrolController : MonoBehaviour
{
    [SerializeField] NavigationController _navigationController;

    List<Transform> _waypoints;
    private Vector3 _targetPosition;
    int _currentIndex = 0;


    private void Awake()
    {
        _waypoints = GetComponentsInChildren<Transform>().Skip(1).ToList();
    }

    
    void Start()
    {
        if (_waypoints.Count > 0)
        {
            _targetPosition = _waypoints[_currentIndex].position;
            _navigationController.MoveTo(_targetPosition);
        }
    }

  
    void Update()
    {
        if (_waypoints.Count == 0) return;

        Patrol();
    }

    void Patrol()
    {
        if (!_navigationController.HasPath())
        {
            _targetPosition = NextWaypoint();
            _navigationController.MoveTo(_targetPosition);
        }
    }

    Vector3 NextWaypoint()
    {
        _currentIndex = GetNextIndex(_currentIndex);
        return _waypoints[_currentIndex].position;
    }

    int GetNextIndex(int i )
    {
        if (i + 1 == _waypoints.Count)
        {
            return 0;
        }

        return i + 1;
    }
}
