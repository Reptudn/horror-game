using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class InteractionHandler : MonoBehaviour
{

    public float interactionRange = 10f;
    public GameObject interactable;
    public TextMeshProUGUI interactionText;
    public TextMeshProUGUI interactionKeyText;
    public GameObject interactionKeyObject;

    public KeyCode interactionKey = KeyCode.F;
    public KeyCode pickupKey = KeyCode.E;

    void Start()
    {
        interactionKeyObject.SetActive(false);
    }

    void Update()
    {
        
        if(SceneManager.GetActiveScene().name == "HomeMenu") return;

        SendRaycast();


    }




    private void SendRaycast(){

        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out RaycastHit hit)){

            if(hit.distance <= interactionRange) { //when in range

                string tag = hit.transform.gameObject.tag;

                if(tag == "Interactable" || tag == "Pickup") {
        
                    if(Input.GetKeyDown(interactionKey) && tag == "Interactable") Interact(hit.transform.gameObject);

                    var interactionInfo = hit.transform.gameObject.GetComponent<InteractionInfo>();

                    if(interactionInfo != null){ 
                        if(tag == "Interactable") GetInteractableInfo(interactionInfo, interactionKey.ToString());
                        else GetInteractableInfo(interactionInfo, pickupKey.ToString());
                    } 

                    Debug.DrawLine(Camera.main.transform.position, hit.transform.position, Color.cyan, Time.deltaTime);

                } else RemoveInteractionText();


            } else RemoveInteractionText();

            
        } else RemoveInteractionText();

        
        

    }

    private void GetInteractableInfo(InteractionInfo interactableComp, string key){

        if(!interactableComp.canBeInteractedWith) return;

        interactionKeyObject.SetActive(true);
        interactionText.SetText("<align=\"left\"><b>" + interactableComp.interactionText + "</b>");
        interactionKeyText.SetText(key);

    }

    private void RemoveInteractionText() { 
        interactionKeyObject.SetActive(false);
        interactionText.SetText(""); 
    }

    private void Interact(GameObject interact){
        if(!interact.GetComponent<InteractionInfo>().canBeInteractedWith) return;
        interact.SendMessage("Interaction");
    }

}
