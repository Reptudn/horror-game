using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light : MonoBehaviour
{

    public Light lightPog;

    public GameObject dancer;

    void Start(){
        InvokeRepeating("changeLight", 1f, 1.2f);
        dancer.SetActive(true);
    }
    
    void changeLight(){
        lightPog.color = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
        lightPog.intensity = Random.Range(1, 4);
    }

}
