using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Damageable")]
public class SODamageable : ScriptableObject
{
    [SerializeField] private int health;
    [SerializeField] private int damage;

    public int Health { get { return health; } }
    public int Damage { get { return damage; } }   
}
