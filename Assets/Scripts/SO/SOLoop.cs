using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "SO/Audio/Loop")]
public class SOLoop : ScriptableObject
{
    [SerializeField] public AudioClip file;
}
