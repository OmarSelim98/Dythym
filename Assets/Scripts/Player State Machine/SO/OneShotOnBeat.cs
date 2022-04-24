using UnityEditor;
using UnityEngine;
[System.Serializable]
public struct OneShotOnBeat
{
    [SerializeField] SOOneShot oneShot;
    [SerializeField] float forBeat;

    public SOOneShot OneShot { get => oneShot; set => oneShot = value; }
    public float ForBeat { get => forBeat; set => forBeat = value; }
}