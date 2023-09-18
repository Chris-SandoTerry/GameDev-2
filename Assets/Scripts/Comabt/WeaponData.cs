
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{
    public List<Health> Enemies;
    public int Damage = 10;

    [SerializeField] private Collider _myCollider;


    void OnTriggerEnter(Collider other)
    {
        if (other == _myCollider) return;

        if (other.TryGetComponent(out Health health))
        {
            Enemies.Add(health);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other == _myCollider) return;

        if (other.TryGetComponent(out Health health))
        {
            Enemies.Remove(health);
        }

    }


    public void ClearEnemies()
    {
        Enemies.Clear();
    }
}
