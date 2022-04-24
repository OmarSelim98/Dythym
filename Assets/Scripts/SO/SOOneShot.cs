using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "SO/Audio/One Shot")]
public class SOOneShot : ScriptableObject
{
    [SerializeField] public AudioClip file;
}
