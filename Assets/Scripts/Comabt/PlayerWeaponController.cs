using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCharacterAnims;
using RPGCharacterAnims.Actions;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] List<GameObject> _weaponTriggerVolumes = new List<GameObject>();

     Animator _animator;
     int _currentIndex = 0;
     private WeaponData _weaponData;

     void Awake()
     {
         _animator = GetComponentInChildren<Animator>();
     }

     void Start()
     {
         var animatorEvents = _animator.gameObject.GetComponent<RPGCharacterAnimatorEvents>();
         animatorEvents.OnWeaponSwitch.AddListener(WeaponSwitch);
         animatorEvents.OnHit.AddListener(Hit);

         for (int i = 1; i < _weaponTriggerVolumes.Count; i++)
         {
             _weaponTriggerVolumes[i].SetActive(false);
         }

         _weaponData = _weaponTriggerVolumes[_currentIndex].GetComponent<WeaponData>();
     }

     void Hit()
     {
         AudioManager.Instance.PlaySFX("Hit");
         foreach (Health enemy in _weaponData.Enemies)
         {
             enemy.DealDamage(_weaponData.Damage);
         }
     }

     void WeaponSwitch()
     {
         _weaponTriggerVolumes[_currentIndex].SetActive(false);
         if (_currentIndex == _weaponTriggerVolumes.Count - 1)
         {
             _currentIndex = 0;
         }
         else
         {
             _currentIndex++;
         }
         _weaponData.ClearEnemies();
         _weaponData = _weaponTriggerVolumes[_currentIndex].GetComponent<WeaponData>();

         _weaponTriggerVolumes[_currentIndex].SetActive(true);
     }

}
