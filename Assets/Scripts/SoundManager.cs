using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    public AudioSource walkDirtSound;
    public string dirtTag = "Dirt";

    public Collider leftFoot;
    public Collider rightFoot;

    public float untilNextStepAudioPauseTime = 1f;

    private bool walking;

    void Start(){
        InvokeRepeating("PlayMoveSound", 0f, untilNextStepAudioPauseTime);
    }

    void Update(){

        if(Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f) walking = true;
        else walking = false;

    }

    void PlayMoveSound(){

        if(!walking) return;
        walkDirtSound.Play();
 
    }


}
