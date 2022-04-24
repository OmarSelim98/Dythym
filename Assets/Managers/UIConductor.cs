using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIConductor : MonoBehaviour
{
    [SerializeField] GameObject debugImageGO;
    [SerializeField] SOAudioStats audioStats;
    Image debugImage;
    // Start is called before the first frame update
    void Start()
    {
        debugImage = debugImageGO.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (audioStats.canPerformAction)
        {
            debugImage.color = Color.green;
        }
        else
        {
            debugImage.color = Color.red;
        }
    }
}
