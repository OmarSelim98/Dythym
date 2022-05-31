using System.Collections;
using UnityEngine;
 public interface Damageable
{
    /*
     
     each damageable must have 
        x points of health
        apply hit function
     
     */
    SODamageable Stats { get; }
    int CurrentHealth { get; set; }
    int Damage { get; }
    void ApplyDamage(int damage, GameObject other);
}