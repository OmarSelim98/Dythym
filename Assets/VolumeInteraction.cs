using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class VolumeInteraction : MonoBehaviour
{
    float mainIntensity;
    Volume volume;
    private Bloom bloom;

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();

        Bloom tmp;
        if (volume.profile.TryGet<Bloom>(out tmp))
        {
            bloom = tmp;
            mainIntensity = bloom.intensity.value;
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (bloom != null)
        {
            if (bloom.intensity.value < 50f)
            {
                bloom.intensity = new MinFloatParameter(bloom.intensity.value + 0.01f, 0, true);

            }
        }
    }
}
