using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "SO/Audio Manager Stats File")]
public class SOAudioStats : ScriptableObject
{
    [SerializeField] public SORoomAudio roomAudio;
    [SerializeField] public bool canPerformAction;
    [SerializeField] private float songBpm;
    [SerializeField] private float secPerBeat;
    [SerializeField] private float songPosition;
    [SerializeField] private float songPositionInBeats;
    [SerializeField] private float dspSongTime;
    [SerializeField] private float beatsPerLoop;
    [SerializeField] private int completedLoops = 0;
    [SerializeField] private float loopPositionInBeats;
    [SerializeField] private float loopPositionInAnalog;
    [SerializeField] private float errorMarginInBeats = 0.25f;
    [SerializeField] private float currentPlayableBeat = -1;

    public bool CanPerformAction { get => CanPerformAction; set => CanPerformAction = value; }
    public float SongBpm { get => songBpm; set => songBpm = value; }
    public float SecPerBeat { get => secPerBeat; set => secPerBeat = value; }
    public float SongPosition { get => songPosition; set => songPosition = value; }
    public float SongPositionInBeats { get => songPositionInBeats; set => songPositionInBeats = value; }
    public float DspSongTime { get => dspSongTime; set => dspSongTime = value; }
    public float BeatsPerLoop { get => beatsPerLoop; set => beatsPerLoop = value; }
    public int CompletedLoops { get => completedLoops; set => completedLoops = value; }
    public float LoopPositionInBeats { get => loopPositionInBeats; set => loopPositionInBeats = value; }
    public float ErrorMarginInBeats { get => errorMarginInBeats; set => errorMarginInBeats = value; }
    public float CurrentPlayableBeat { get => currentPlayableBeat; }

    public void Initiate()
    {
        songBpm = roomAudio.BPM;
        beatsPerLoop = roomAudio.LengthInBeats;
        secPerBeat = 0;
        songPosition = 0;
        songPosition = 0;
        songPositionInBeats = 0;
        dspSongTime = 0;
        completedLoops = 0;
        loopPositionInAnalog = 0;
        errorMarginInBeats = roomAudio.ErrorMargin;
    }
    public void CalculateSecPerBeat()
    {
        secPerBeat = 60f / songBpm;
    }

    public void RecordDspTime()
    {
        dspSongTime = (float)AudioSettings.dspTime;
    }

    public void UpdateSongPosition()
    {
        songPosition = (float)(AudioSettings.dspTime) - dspSongTime;
    }
    public void UpdateSongBeats()
    {
        songPositionInBeats = songPosition / secPerBeat;
    }

    public void UpdateLoopPosition()
    {
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPositionInBeats = (float) Math.Round((double) (songPositionInBeats - completedLoops * beatsPerLoop) , 2);
    }
    public void UpdateAnaglogLoopPosition()
    {
        loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;
    }

    public void UpdateCanPerformAction()
    {
        foreach(float beat in roomAudio.PlayableBeats)
        {
            float max = beat + errorMarginInBeats;
            float min = beat - errorMarginInBeats;

            if ( (loopPositionInBeats-min) * (max-loopPositionInBeats)  >= 0) {
                canPerformAction = true;
                currentPlayableBeat = beat;
                break;
            }
            else
            {
                canPerformAction = false;
                currentPlayableBeat = -1;
            }
        }
    }

    public float BeatsToSeconds(float beats)
    {
        return beats * 60 / songBpm;
    }

    public float AbsoluteAnimationSpeedByFrame(float speed) // Gets speed running on x frames to run on 0.5 frames
    {
        return speed / 0.5f;
    }
    public float RelativeAnimationSpeed(float mainSpeed)
    {
        return mainSpeed * songBpm / 120;
    }
    public bool onBeat() {
        //Debug.Log(Mathf.Ceil(loopPositionInBeats) - loopPositionInBeats);
        return Mathf.Ceil(loopPositionInBeats) - loopPositionInBeats <= 0.05f;
    }
    public bool beforePlayableBeatBy(float indicatingBeat) {
        foreach (float playableBeat in roomAudio.PlayableBeats)
        {
            float c = playableBeat - ((loopPositionInBeats + indicatingBeat)%beatsPerLoop);
            if (c >= -0.05 && c <= 0.05)
            {
                return true;
            }
        }   

        return false;
    }
}
