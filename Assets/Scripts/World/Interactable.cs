using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionInfo))]
public class Interactable : MonoBehaviour
{

    public void Interaction(){
        Debug.Log("Interaction");
    }
}
