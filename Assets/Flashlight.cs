using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(!this.gameObject.activeSelf) return;

        if(Input.GetMouseButtonDown(0)){
            Debug.Log("Toggle Light");
            light.SetActive(!light.activeSelf);
        }
    }
}
