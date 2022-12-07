using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenClose : MonoBehaviour, IInteractable
{
    
    Animator animator;
    InteractionInfo info;

    public bool simulate = false; //later

    void Start(){
        animator = GetComponent<Animator>();
    }

    public void Interaction(){

        animator.SetBool("opened", !animator.GetBool("opened"));

        if(animator.GetBool("opened")) info.interactionText = "Close door";
        else info.interactionText = "Open door";

        Debug.Log("[INTERACTION] Interacted with door");

    }

}