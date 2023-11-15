using System;
using RPGCharacterAnims;
using RPGCharacterAnims.Lookups;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;


    private RPGCharacterController _rpgCharacterController;
    ItemDropper _itemDropper; 
    InventoryItem _inventoryItem;
     Pickup _pickup;
    int _health;
    bool _alive = true;

    private void Awake()
    {
        _rpgCharacterController = GetComponent<RPGCharacterController>();
        _itemDropper = GetComponent<ItemDropper>();
    }

    void Start()
    {
        _health = _maxHealth;
    }

    public void DealDamage(int damage)
    {
        if (!_alive) return;

        _health = Mathf.Max(_health - damage, 0);

        if (_health > 0)
        {
            
            _rpgCharacterController.GetHit(1);
        }
        else
        {
            _rpgCharacterController.Knockdown(KnockdownType.Knockdown1);
            _alive = false;
            _itemDropper.SpawnPickup(_inventoryItem, 1, transform.position);
            _itemDropper.DropItem(_inventoryItem, 1);
            AudioManager.Instance.musicSource.Stop();
            //AudioManager.Instance.PlaySFX("Death");


            if (GetComponent<Target>())
            {
                Destroy(GetComponent<Target>());
            }
        }
    }

     public bool IsAlive()
     {
         return _alive;
     }
}