using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementContoller : MonoBehaviour
{

    [Header("Basic Components")]
    public Transform playerTransform;
    public CharacterController characterController;
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    bool isGround;
    Vector3 velocity;
    
    [Header("Movement Settings")]
    public float speed = 10f;
    public float jumpForce = 2f;
    public float gravity = -1f;

    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
        isGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGround && velocity.y < 0){
            velocity.y = -2f;
        }

        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * xMove + transform.forward * zMove;
        characterController.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGround){
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
        
    
    }

}
