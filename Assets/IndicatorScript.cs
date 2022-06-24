using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class IndicatorScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SOAudioStats _audioStats;
    RectTransform _t;
    float _reachPoint = 0;
    RawImage _img;
    bool _prevState = false;
    private IndicatorScript _instance;

    public IndicatorScript Instance { get => _instance; set => _instance = value; }

    private void Awake()
    {
        _instance = this;
        _t = (RectTransform)this.gameObject.transform;
        _img = GetComponent<RawImage>();
    }
    void Start()
    {
        
    }
    public void MoveLeft()
    {
        this._t.localPosition = new Vector3(_t.localPosition.x + 200, _t.localPosition.y, _t.localPosition.z);
        _reachPoint = 50;
        this.DestroyOnCompletion();
    }
    public void MoveRight()
    {
        _t.localPosition = new Vector3(_t.localPosition.x - 200, _t.localPosition.y, _t.localPosition.z);
        _reachPoint = -50;
        DestroyOnCompletion();
    }
    async void DestroyOnCompletion()
    {
        await _t.DOLocalMoveX(_reachPoint, _audioStats.BeatsToSeconds(3.0f) - _audioStats.roomAudio.ErrorMargin).SetEase(Ease.Linear).AsyncWaitForCompletion();
        _t.DOScale(new Vector3(1.5f,1.5f,1.5f), _audioStats.roomAudio.ErrorMargin);
        await _t.DOLocalMoveX(0, _audioStats.roomAudio.ErrorMargin).SetEase(Ease.Linear).AsyncWaitForCompletion();
        Destroy(this.gameObject);
    }
}
