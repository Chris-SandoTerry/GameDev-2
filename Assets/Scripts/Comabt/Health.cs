using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    
    int _health;
    
    void Start()
    {
        _health = _maxHealth;
    }

    public void DealDamage(int damage)
    {
        if (_health == 0) return;

        _health = Mathf.Max(_health - damage, 0);
        Debug.Log(_health);
    }
}