using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioConductor : MonoBehaviour
{
    [SerializeField] SOAudioStats _generalStats;
    [SerializeField] SOGameEvent OnAudioStarted;
    float waitFor = 2.0f;
    AudioSource _musicSource;
    // Start is called before the first frame update
    void Awake()
    {
        _musicSource = GetComponent<AudioSource>();
        _generalStats.Initiate();
        _generalStats.CalculateSecPerBeat();
        _generalStats.RecordDspTime();
        _musicSource.clip = _generalStats.roomAudio.Initial.file;
        
    }
    void Start()
    {
        StartCoroutine(StartAudio());
    }
    // Update is called once per frame
    void Update()
    {
        _generalStats.UpdateSongPosition();
        _generalStats.UpdateSongBeats();
        _generalStats.UpdateLoopPosition();
        _generalStats.UpdateAnaglogLoopPosition();
        _generalStats.UpdateCanPerformAction();
    }

    IEnumerator StartAudio()
    {
        yield return new WaitForSeconds(waitFor);
        _musicSource.Play();
        OnAudioStarted.Raise();
    }
}
