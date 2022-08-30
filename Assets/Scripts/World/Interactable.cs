using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interactable : MonoBehaviour
{

    public string interactionText;
    public bool canBeInteractedWith = true;

    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.tag = "Interactable";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
