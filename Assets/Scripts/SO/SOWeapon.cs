using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon")]
public class SOWeapon : ScriptableObject
{
    [SerializeField]
    float damage = 0.0f;

    [SerializeField]
    LayerMask targetLayer;
}
