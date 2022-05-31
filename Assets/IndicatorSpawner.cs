using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    private bool _prevCanPerformAction = false;
    private bool spawnIndicator;
    private bool _prevState = false;
    private int _counter = 0;
    [SerializeField] float beforeBeatBy = 1.5f;
    [SerializeField] public RawImage _indicatorPrefab;
    [SerializeField] SOAudioStats _audioStats;
    void Start()
    {
        spawnIndicator = _audioStats.beforePlayableBeatBy(beforeBeatBy);
    }

    // Update is called once per frame
    void Update()
    {
        spawnIndicator = _audioStats.beforePlayableBeatBy(beforeBeatBy);
        if (spawnIndicator != _prevState)
        {
            Debug.Log(_counter + ", Spawn Indicator: " + spawnIndicator + ", Prev: " + _prevState);
            //Debug.Log(_audioStats.LoopPositionInBeats);
            if (_prevState)
            {

                GameObject left = Instantiate(_indicatorPrefab, this.gameObject.transform).gameObject;
                GameObject right = Instantiate(_indicatorPrefab, this.gameObject.transform).gameObject;
                if (left != null)
                {
                    left.SendMessage("MoveLeft");
                }
                if (right != null)
                {
                    right.SendMessage("MoveRight");
                }
            }
            _prevState = spawnIndicator;
            _counter++;
        }
        //if (_audioStats.onBeat())
        //{
        //    
        //}
        //if (_audioStats.canPerformAction != _prevCanPerformAction)
        //{
        //    //if(_audioStats.canPerformAction == true)
        //    //{

        //    //}

        //    _prevCanPerformAction = _audioStats.canPerformAction;
        //}
    }
}
