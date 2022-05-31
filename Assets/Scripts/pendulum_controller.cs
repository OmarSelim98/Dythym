using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class pendulum_controller : MonoBehaviour
{
    [SerializeField]
    SOAudioStats audioStats;
    Transform transform;
    //float animationSpeed = 1;
    //bool startedRotating = false;
    [SerializeField]
    GameObject eye_opened;
    [SerializeField]
    GameObject eye_closed;
    Animator animatorComponent;
    // Start is called before the first frame update
    void Awake()
    {

        transform = GetComponent<Transform>();
        //x speed (songBpm)bpm
        //1 speed 120bpm
        //animatorComponent = GetComponent<Animator>();
        //animationSpeed = audioStats.SongBpm / 120.0f;

        //animatorComponent.speed = animationSpeed;

        //Debug.Log("Pendulum Animation Speed : "+animationSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        if (audioStats.canPerformAction)
        {
            eye_closed.SetActive(true);
            eye_opened.SetActive(false);
        }
        else
        {
            eye_closed.SetActive(false);
            eye_opened.SetActive(true);
            //startedRotating = false;
            //transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    
}
