using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorEffects : MonoBehaviour
{

    public GameObject ParticleSystemContainer;
    private ParticleSystem particleSystem;
    
    void Start(){
        particleSystem = ParticleSystemContainer.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        
        ParticleSystemContainer.transform.position = Input.mousePosition;

        if(Input.GetAxis("Mouse X") < 0 || Input.GetAxis("Mouse Y") < 0)
        {

            if (!Input.GetMouseButtonDown(0)) return;
                
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 1.5f;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                ParticleSystemContainer.transform.position = worldPos;
                Debug.Log("Cum");
                particleSystem.Stop();
                particleSystem.Play();
            
        }

    }        
}
