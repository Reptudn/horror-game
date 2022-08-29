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
    public float Gravity = .981f;

    [Header("Ground Check")]
    public LayerMask GroundMask;
    public bool Grounded;

    [Header("Objects")]
    public GameObject PlayerModel;
    private Rigidbody RigidBody;

    //[Header("Pause Menu")]
    //public GameObject pauseMenuContainer;

    private void Start()
    {
        PlayerModel.SetActive(false);
        RigidBody = transform.GetComponent<Rigidbody>();
        RigidBody.freezeRotation = true;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (!PlayerModel.activeSelf){ Spawn(); PlayerModel.SetActive(true); Cursor.lockState = CursorLockMode.Locked; }
        }

        if (hasAuthority) { 

            //if(!pauseMenuContainer.activeSelf) 
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
        GameObject[] spawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint");
        if(spawnPoint[0] == null) { transform.position = new Vector3(0f, 8f, 0f); return; }
        transform.position = spawnPoint[0].transform.position;
    }

    public void Movement()
    {

        Transform Orientation = Camera.main.transform;

        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 MoveDirection = Orientation.forward * zDirection + Orientation.right * xDirection;
        MoveDirection = new Vector3(Mathf.Clamp(MoveDirection.x * Speed * Time.deltaTime, -Speed, Speed), 0, Mathf.Clamp(MoveDirection.z * Speed * Time.deltaTime, -Speed, Speed));

        if(Input.GetButtonDown("Jump") && Grounded){
            MoveDirection.y = Mathf.Sqrt(JumpForce);
        }

        RigidBody.velocity += MoveDirection;

    }
}
