using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    public Light[] thunder;
    public int thunderDelayMin = 5; 
    public int thunderDelayMax = 30;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("PlayThunder", Random.Range(thunderDelayMin, thunderDelayMin));
    }

    public void PlayThunder(){

        Debug.Log("Thunder");

        foreach(Light o in thunder){

            o.intensity = Random.Range(300, 40000);
            Wait(Random.Range(0.4f, 1.2f));
            o.intensity = 0;

        }

        Invoke("PlayThunder", Random.Range(thunderDelayMin, thunderDelayMin));

    }

    IEnumerator Wait(float seconds){
        yield return new WaitForSecondsRealtime(seconds);
    }
}
