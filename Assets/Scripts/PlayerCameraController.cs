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

    private void Start()
    {

        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (!PlayerModel.activeSelf){ Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
        }

    }

    private void Update()
    {

        if (hasAuthority){

            float MouseX = Input.GetAxis("Mouse X") * Time.deltaTime * (MouseSensitivity * 100);
            float MouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * (MouseSensitivity * 100);

            yRotation += MouseX;
            xRotation -= MouseY;
            xRotation  = Mathf.Clamp(xRotation, -90f, 90f);


            Camera.main.transform.position = CameraAnchor.transform.position;
            Camera.main.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
            transform.rotation = Quaternion.Euler(0, yRotation, 0);

        }
    }   
}
