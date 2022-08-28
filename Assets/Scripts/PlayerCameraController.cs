using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerCameraController : NetworkBehaviour
{
    [Header("Mouse Settings")]
    [Range(1f, 30f)]
    public float MouseSensitivity = 15f;

    [Header("Objects")]
    public GameObject PlayerModel;
    public GameObject CameraAnchor;

    private float xRotation;
    private float yRotation;

    private LayerMask PlayerMask;
    private LayerMask OwnPlayerMask;

    [Header("MenuContainer")]
    public GameObject menuContainer;

    private void Start()
    {

        PlayerMask = LayerMask.NameToLayer("Player");
        OwnPlayerMask = LayerMask.NameToLayer("OwnPlayer");

        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (!PlayerModel.activeSelf){ 
                Cursor.lockState = CursorLockMode.Locked; 
                Cursor.visible = false; 
            }
        }

        if (hasAuthority)
        {

            PlayerModel.gameObject.layer = OwnPlayerMask;

            Transform[] AllChildren = GetComponentsInChildren<Transform>(true);
            foreach (Transform Child in AllChildren)
            {
                if (Child.gameObject.layer == PlayerMask)
                {
                    Child.gameObject.layer = OwnPlayerMask;
                }
            }
        }
    }

    private void Update()
    {

        if (hasAuthority){

            if(SceneManager.GetActiveScene().name != "MainMenu" && !menuContainer.activeSelf){

                

                GameObject Head = PlayerModel.transform.Find("Head").gameObject;

                float MouseX = Input.GetAxis("Mouse X") * Time.deltaTime * (MouseSensitivity * 100);
                float MouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * (MouseSensitivity * 100);

                yRotation += MouseX;
                xRotation -= MouseY;
                xRotation  = Mathf.Clamp(xRotation, -90f, 90f);

                Camera.main.transform.position = CameraAnchor.transform.position;
                Camera.main.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
                Head.transform.rotation = Camera.main.transform.rotation;

                transform.rotation = Quaternion.Euler(0, yRotation, 0);

            }
        }
    }   
}
