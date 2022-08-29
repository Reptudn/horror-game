using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 30f;
    float xRotation = 0f;

    public Transform playerBody;
    public Transform playerHead;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void FixedUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * (mouseSensitivity / 2);
        float mouseY = Input.GetAxis("Mouse Y") * (mouseSensitivity / 2);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -50f, 50f);

        playerBody.Rotate(Vector3.up * mouseX);
        playerHead.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        

    }
}
