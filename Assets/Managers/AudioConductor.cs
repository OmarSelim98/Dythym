using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioConductor : MonoBehaviour
{
    [SerializeField] SOAudioStats _generalStats;
    AudioSource _musicSource;
    // Start is called before the first frame update
    void Awake()
    {
        _musicSource = GetComponent<AudioSource>();
        _generalStats.Initiate();
        _generalStats.CalculateSecPerBeat();
        _generalStats.RecordDspTime();
        _musicSource.clip = _generalStats.roomAudio.Initial.file;
        _musicSource.Play();
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
}
