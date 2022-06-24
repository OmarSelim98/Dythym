using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Game Event")]
public class SOGameEvent : ScriptableObject
{
    List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise() {
        if (listeners.Count > 0)
        {
            for (int i = listeners.Count - 1; i >= 0; i++)
            {
                Debug.Log(i);
                listeners[i].OnEventRaised();
            }
        }
    }

    public void RegisterListener(GameEventListener listener) {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

}
