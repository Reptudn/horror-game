using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Flashlight : MonoBehaviour
{

    public GameObject light;

    // Start is called before the first frame update
    void Start()
    {
        light.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if(this.gameObject.activeSelf && transform.parent != null && transform.parent.name == "ItemAnchor"){

            if (Input.GetMouseButtonDown(0)){ 

                light.SetActive(!light.activeSelf);

            }

        }

    }

}
