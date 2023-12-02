using System;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform _player;
 
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.position = new Vector3(_player.position.x, transform.position.y, _player.position.z);
    }
}
