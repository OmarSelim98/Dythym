using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "SO/Audio/Room Audio")]
public class SORoomAudio : ScriptableObject
{
    [SerializeField] public SOLoop Initial;
    [SerializeField] public SOLoop Gameplay;
    [SerializeField] public SOLoop Final;
    [SerializeField] public float BPM;
    [SerializeField] public List<float> PlayableBeats;
    [SerializeField] public int LengthInBeats;
    [SerializeField] public float ErrorMargin;
    [SerializeField] public List<OneShotOnBeat> dashList;
    [SerializeField] public List<OneShotOnBeat> attackList;

}

