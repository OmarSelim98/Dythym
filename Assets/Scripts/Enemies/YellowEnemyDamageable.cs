using System.Collections;
using UnityEngine;

public class YellowEnemyDamageable : MonoBehaviour, Damageable
{
    [SerializeField]
    SODamageable _stats;
    int _currentHealth;
    int _damage;
    public SODamageable Stats => _stats;

    public int CurrentHealth { get => _currentHealth; set => _currentHealth = value; }

    public int Damage => _damage;

    public void ApplyDamage(int damage, GameObject other)
    {
        Debug.Log("Yellow Enemy Damaged");
    }

    // Use this for initialization
    void Awake()
    {
        _damage = _stats.Damage;
        _currentHealth = _stats.Health;
    }
}
