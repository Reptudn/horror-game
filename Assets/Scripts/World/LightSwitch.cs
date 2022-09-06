using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    
    public Light lightSource;
    public Animator animator;

    void Start()
    {
        lightSource.enabled = false;
    }

    public void Interact(){
        lightSource.enabled = !lightSource.enabled;
        Debug.Log("Interacted with light switch");
        Animation();
    }

    void Animation(){

    }
}
