using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Flashlight : MonoBehaviour
{

    public GameObject lightSource;
    public Animator animator;
    public AudioSource onSound;
    public AudioSource offSound;
    

    // Start is called before the first frame update
    void Start()
    {
        lightSource.SetActive(false);
        animator.SetBool("Light On", false);
    }

    // Update is called once per frame
    void Update()
    {

        if(this.gameObject.activeSelf && transform.parent != null && transform.parent.name == "ItemAnchor"){

            if (Input.GetMouseButtonDown(0)){ 

                lightSource.SetActive(!lightSource.activeSelf);
                animator.SetBool("Light On", !animator.GetBool("Light On"));

                if(lightSource.activeSelf) onSound.Play();
                else offSound.Play();

            }

        }

    }

}
