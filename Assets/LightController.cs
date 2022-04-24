using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LightController : MonoBehaviour
{
    [SerializeField] bool switchable = false;
    [SerializeField] SOAudioStats audioStats;
    [SerializeField] List<Color> colorList;
    Light light;
    int counter = 0;
    bool prevCanPerformAction;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        prevCanPerformAction = audioStats.canPerformAction;
    }

    // Update is called once per frame
    void Update()
    {
        if(light != null)
        {
            if(counter == colorList.Count)
            {
                counter = 0;
            }
            if (audioStats.canPerformAction != prevCanPerformAction)
            {
                DOTween.To(x => light.color = new Color(x, light.color.g, light.color.b, light.color.a), light.color.r, colorList[counter].r, audioStats.BeatsToSeconds(0.25f));
                DOTween.To(x => light.color = new Color(light.color.r, x, light.color.b, light.color.a), light.color.g, colorList[counter].g, audioStats.BeatsToSeconds(0.25f));
                DOTween.To(x => light.color = new Color(light.color.r, light.color.g, x, light.color.a), light.color.b, colorList[counter].b, audioStats.BeatsToSeconds(0.25f));
                DOTween.To(x => light.color = new Color(light.color.r, light.color.g, light.color.b, x), light.color.a, colorList[counter].a, audioStats.BeatsToSeconds(0.25f));
                //light.color = colorList[counter];
                counter++;
                    prevCanPerformAction = audioStats.canPerformAction;
            }
        }
    }
}
