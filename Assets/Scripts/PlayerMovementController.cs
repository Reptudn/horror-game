using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerMovementController : NetworkBehaviour
{

    [Header("Movement Settings")]
    public float Speed = 0.1f;
    public float JumpForce = 2f;

    [Header("Ground Check")]
    public LayerMask GroundMask;
    public bool Grounded;

    [Header("Objects")]
    public GameObject PlayerModel;
    public CharacterController Controller;

    private void Start()
    {
        PlayerModel.SetActive(false);

    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (!PlayerModel.activeSelf){ Spawn(); PlayerModel.SetActive(true); Cursor.lockState = CursorLockMode.Locked; }
        }

        if (hasAuthority) { 

            Movement();

            Grounded = Physics.Raycast(transform.position, Vector3.down, transform.GetComponent<Collider>().bounds.size.y * 0.5f + 0.2f, GroundMask);

            if (transform.position.y < -10)
            {
                Spawn();
            }

        }

        
    }

    public void Spawn()
    {
        transform.position = new Vector3(0f, 8f, 0f);
    }

    public void Movement()
    {

        Transform Orientation = Camera.main.transform;

        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 MoveDirection = Orientation.forward * zDirection + Orientation.right * xDirection;
        Controller.Move(MoveDirection * Speed * Time.deltaTime);

    }
}
