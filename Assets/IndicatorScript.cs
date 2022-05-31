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
        this._t.localPosition = new Vector3(_t.localPosition.x + 300, _t.localPosition.y, _t.localPosition.z);
        this.DestroyOnCompletion();
    }
    public void MoveRight()
    {
        _t.localPosition = new Vector3(_t.localPosition.x - 300, _t.localPosition.y, _t.localPosition.z);
        DestroyOnCompletion();
    }
    async void DestroyOnCompletion()
    {
        await _t.DOLocalMoveX(0, _audioStats.BeatsToSeconds(3.0f)).SetEase(Ease.Linear).AsyncWaitForCompletion();
        Destroy(this.gameObject);
    }
}
