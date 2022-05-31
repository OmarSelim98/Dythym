using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LightController : MonoBehaviour
{
    [SerializeField] bool isColorSwitching = true;
    [SerializeField] bool isLightChanging = true;
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
            
            if (audioStats.canPerformAction != prevCanPerformAction)
            {
                if (isColorSwitching)
                {
                    switchColors();
                }

                if (isLightChanging)
                {
                    //switchLightIntensity();
                }
                prevCanPerformAction = audioStats.canPerformAction;
            }
        }
    }

    void switchLightIntensity()
    {
        if (audioStats.canPerformAction) {
            float nextPlayableBeat = audioStats.BeatsPerLoop;
            int i = 0;
            foreach (float beat in audioStats.roomAudio.PlayableBeats)
            {
                if (beat == audioStats.CurrentPlayableBeat)
                {
                    nextPlayableBeat = audioStats.roomAudio.PlayableBeats[i + 1];
                    break;
                }
                i++;
            }

            DOTween.To(x => light.intensity = x, 1.4f, 0.3f, audioStats.BeatsToSeconds(Mathf.Abs(nextPlayableBeat - audioStats.CurrentPlayableBeat)));
        }
    }
    void switchColors()
    {
        if(colorList.Count > 1)
        {
            if (counter == colorList.Count)
            {
                counter = 0;
            }
            DOTween.To(x => light.color = new Color(x, light.color.g, light.color.b, light.color.a), light.color.r, colorList[counter].r, audioStats.BeatsToSeconds(0.25f));
            DOTween.To(x => light.color = new Color(light.color.r, x, light.color.b, light.color.a), light.color.g, colorList[counter].g, audioStats.BeatsToSeconds(0.25f));
            DOTween.To(x => light.color = new Color(light.color.r, light.color.g, x, light.color.a), light.color.b, colorList[counter].b, audioStats.BeatsToSeconds(0.25f));
            DOTween.To(x => light.color = new Color(light.color.r, light.color.g, light.color.b, x), light.color.a, colorList[counter].a, audioStats.BeatsToSeconds(0.25f));
            counter++;
        }
    }
}
