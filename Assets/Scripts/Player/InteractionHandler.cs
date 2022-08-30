using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Interactable))]
public class InteractionHandler : MonoBehaviour
{

    public float interactionRange = 10f;
    public GameObject interactable;
    public TextMeshProUGUI interactionText;
    public float textDuration = 2f;

    private RaycastHit hit;


    // Start is called before the first frame update
    void Start()
    {
        interactable.SetActive(false);
        InvokeRepeating("RemoveInteractionText", 1f, textDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(SceneManager.GetActiveScene().name == "HomeMenu") return;

        SendRaycast();


    }




    private void SendRaycast(){

        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out RaycastHit hit)){

            if(hit.distance <= interactionRange) { //when in range

                var comp = hit.transform.gameObject.GetComponent<Interactable>();
                if(comp == null) return; //not interactable

                interactable.SetActive(true);
                GetInteractableInfo(comp);
                this.hit = hit;

                Debug.DrawLine(Camera.main.transform.position, hit.transform.position, Color.cyan, Time.deltaTime);

            }

        }

        
        

    }

    private void GetInteractableInfo(Interactable interactableComp){

        if(!interactableComp.canBeInteractedWith) return;

        Debug.Log(interactableComp.interactionText);
        interactionText.SetText(interactableComp.interactionText);
        Debug.Log("Interaction text: " + interactionText.text);
        interactable.SetActive(true);


    }

    private void RemoveInteractionText() { 
        interactionText.SetText(""); 
    }

}
