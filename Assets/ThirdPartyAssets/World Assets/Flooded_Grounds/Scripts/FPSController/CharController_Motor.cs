using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class CharController_Motor : NetworkBehaviour {

	[Header("Movement and Mouse Attributes")]
	public float speed = 10.0f;
	public float sensitivity = 60.0f;
	public float WaterHeight = 15.5f;
	public float sprintMultiplier = 1.4f;
	CharacterController character;
	public GameObject cam;
	float moveFB, moveLR;
	float rotX, rotY;
	float gravity = -9.8f;

	private int Denominator = 5;

	
    private float xRotation;
    private float yRotation;

	[Header("Animation Attributes")]
	public Animator animator;

	void Start(){
		character = GetComponent<CharacterController> ();
	}

	void SetCharacterPosition(Vector3 Position)
	{
		character.enabled = false;
		character.transform.position = Position;
		character.enabled = true;
	}

	void SetCharacterRotation(Quaternion Rotation)
	{
		character.enabled = false;
		character.transform.rotation = Rotation;
		character.enabled = true;
	}

	public void Spawn()
    {
        GameObject[] spawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint");
        if(spawnPoint[0] == null) { SetCharacterPosition(new Vector3(0f, 8f, 0f)); return; }
		SetCharacterPosition(spawnPoint[0].transform.position);
    }

	void CheckForWaterHeight(){
		if (transform.position.y < WaterHeight) {
			gravity = 0f;			
		} else {
			gravity = -9.8f;
		}
	}

	void Update(){

		if(SceneManager.GetActiveScene().name == "MainMenu") return;

		if (  hasAuthority && !transform.Find("Character").gameObject.activeSelf){ transform.Find("Character").gameObject.SetActive(true); Cursor.lockState = CursorLockMode.Locked; Spawn(); };
		if ( !hasAuthority && !transform.Find("Character").gameObject.activeSelf){ transform.Find("Character").gameObject.SetActive(true); };

		if( !hasAuthority ){ return; }
		
		if (transform.position.y < -10f) { Spawn(); }

		float MoveX = Input.GetAxis ("Horizontal");
		float MoveY = Input.GetAxis ("Vertical");


		bool Strafe = (Mathf.Abs(MoveX) > 0 && Mathf.Abs(MoveY) > 0);

		moveFB = MoveX * (speed / Denominator) / (Strafe ? 1.75f : 1f);
		moveLR = MoveY * (speed / Denominator) / (Strafe ? 1.75f : 1f);

		rotX = Input.GetAxis ("Mouse X") * sensitivity;
		rotY = Input.GetAxis ("Mouse Y") * sensitivity;

		float MouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

		yRotation += MouseX;
		xRotation -= MouseY;
		xRotation  = Mathf.Clamp(xRotation, -90f, 90f);

		CheckForWaterHeight();
		CameraRotation();

		Vector3 movement = new Vector3 (moveFB, gravity, moveLR);

		movement = transform.rotation * movement;

		if(Input.GetKey(KeyCode.LeftShift)){
			movement *= sprintMultiplier;
		}

		if(movement == Vector3.zero){

			animator.SetBool("idle", true);

		} else {
			
			animator.SetBool("idle", false);
			animator.SetInteger("VelocityForward", (int)movement.z * Denominator);
			animator.SetInteger("VelocityLeft", (int)(-movement.x) * Denominator);
			animator.SetInteger("VelocityUp", (int)movement.y * Denominator);

		}

		character.Move (movement * Time.deltaTime);
	}


	void CameraRotation(){	

		//transform.Find("Character").transform.rotation = Quaternion.Euler(0f, xRotation, 0f);

		//transform.rotation = Quaternion.Euler(0f, xRotation, 0f);
		transform.Rotate(0f, rotX * Time.deltaTime, 0f);

		Camera.main.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
		Camera.main.transform.position = cam.transform.position;
	}


	void Jump(){
		
	}


}
