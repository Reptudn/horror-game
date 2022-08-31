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

    public KeyCode interactionKey = KeyCode.F;


    void Start()
    {
        
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

                    if(interactionInfo != null) GetInteractableInfo(interactionInfo);

                    Debug.DrawLine(Camera.main.transform.position, hit.transform.position, Color.cyan, Time.deltaTime);
                    Debug.Log("Hit: " + hit.transform.gameObject.name);

                } else RemoveInteractionText();


            } else RemoveInteractionText();

            
        } else RemoveInteractionText();

        
        

    }

    private void GetInteractableInfo(InteractionInfo interactableComp){

        if(!interactableComp.canBeInteractedWith) return;

        //Debug.Log(interactableComp.interactionText);
        interactionText.SetText(interactableComp.interactionText);
        //Debug.Log("Interaction text: " + interactionText.text);
        interactable.SetActive(true);

    }

    private void RemoveInteractionText() { 
        interactionText.SetText(""); 
    }

    private void Interact(GameObject interact){
        interact.SendMessage("Interaction");
    }

}
