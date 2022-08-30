using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggTrigger : MonoBehaviour
{

    private KeyCode[] code = new KeyCode[]{
        KeyCode.P,
        KeyCode.A,
        KeyCode.R,
        KeyCode.T,
        KeyCode.Y
    };

    private bool success = false;

    public GameObject easteregg;

    // Start is called before the first frame update
    void Start(){
        easteregg.SetActive(false);
    }

    void Update(){

        if(Input.GetKeyDown(KeyCode.F6)) easteregg.SetActive(!easteregg.activeSelf);

    }
    
}
