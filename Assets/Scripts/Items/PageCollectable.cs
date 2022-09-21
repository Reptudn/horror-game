using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageCollectable : MonoBehaviour, IInteractable
{
    [Range(0,10)]
    public float timeUntilDestroy = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interaction(){

        Invoke("DestroyObject", timeUntilDestroy);

    }

    void DestroyObject(){
        GameObject.Destroy(this.gameObject);
        Debug.Log("Page destroyed");
    }

}
