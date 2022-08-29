using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharController_Motor : MonoBehaviour {

	[Header("Movement and Mouse Attributes")]
	public float speed = 10.0f;
	public float sensitivity = 30.0f;
	public float WaterHeight = 15.5f;
	public float sprintMultiplier = 1.4f;
	CharacterController character;
	public GameObject cam;
	float moveFB, moveLR;
	float rotX, rotY;
	float gravity = -9.8f;

	[Header("Animation Attributes")]
	public Animator animator;

	void Start(){
		if(SceneManager.GetActiveScene().name != "MainMenu") Cursor.lockState = CursorLockMode.Locked;
		character = GetComponent<CharacterController> ();
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

		moveFB = Input.GetAxis ("Horizontal") * speed;
		moveLR = Input.GetAxis ("Vertical") * speed;

		rotX = Input.GetAxis ("Mouse X") * sensitivity;
		rotY = Input.GetAxis ("Mouse Y") * sensitivity;

		//rotX = Input.GetKey (KeyCode.Joystick1Button4);
		//rotY = Input.GetKey (KeyCode.Joystick1Button5);

		CheckForWaterHeight ();


		Vector3 movement = new Vector3 (moveFB, gravity, moveLR);

		movement = transform.rotation * movement;

		if(Input.GetKey(KeyCode.LeftShift) && moveLR <= -15f){
			movement *= sprintMultiplier;
		}

		Debug.Log(movement.ToString());

		if(movement == Vector3.zero){

			animator.SetBool("idle", true);

		} else {
			
			animator.SetBool("idle", false);
			animator.SetInteger("VelocityForward", (int)movement.z);
			animator.SetInteger("VelocityLeft", (int)(-movement.x));
			animator.SetInteger("VelocityUp", (int)movement.y);

		}

		character.Move (movement * Time.deltaTime);
	}


	void CameraRotation(GameObject cam, float rotX, float rotY){		
		transform.Rotate (0, rotX * Time.deltaTime, 0);
		cam.transform.Rotate (-rotY * Time.deltaTime, 0, 0);
	}


	void Jump(){
		
	}


}
