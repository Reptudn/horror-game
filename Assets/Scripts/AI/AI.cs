using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AI : MonoBehaviour
{

    /*
    Ray Colors: 
        Closest Player = Yellow;
        Environment = Blue;
    */

    [Header("AI Settings")]
    public int rayCount = 360;
    public float rayDistance = 10f;
    public float minDistanceToObject = 2f;
    public float walkSpeed = 5f;
    public float chaseSpeed = 8f;


    [Header("Components")]
    public CharacterController ai_controller;
    public Animator ai_animator;
    public Transform rayOrigin;



    private RaycastHit hit;


    [Header("Information")]
    public string state = "Idle";


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveTowardsClosestPlayer();
    }

    private void MoveTowardsClosestPlayer(){

        GameObject track = GetClosestPlayer();

        if(track == null) { Debug.Log("No Closest Player found!"); return; }

        Vector3 moveTo = Vector3.MoveTowards(transform.position, track.transform.position, walkSpeed * 10);
        Vector3 walk = new Vector3(moveTo.x, 0f, moveTo.z);
        ai_controller.Move(moveTo);

        Debug.Log("Targetting: " + track.name);

    }

    private void Move(){

        Vector3 movement = new Vector3 (1f, 0f, 1f);

		movement = transform.rotation * movement;

        if(CheckFront()){
            
            ai_controller.Move(transform.forward / 10f);
            return;
        }
        
        //Vector3 newDir = new Vector3(transform.forward.x * Random.Range(-1f, 1f), transform.forward.y, transform.forward.z);

        int rand = (int)Random.Range(0f,10f);

        if(rand < 5) ai_controller.Move(transform.right);
        else ai_controller.Move(-transform.right);
        
    }

    private bool CheckFront(){

        Ray ray = new Ray(rayOrigin.position, transform.forward);

        if(Physics.Raycast(ray, out RaycastHit hit)){

            Debug.DrawLine(transform.position, transform.forward, Color.blue, 3f);

            Debug.Log(hit.distance);

            if(hit.distance > minDistanceToObject) {
                Debug.Log("Can walk");
                return true; 
            }

        }

        Debug.Log("Cannot walk");
        return false;
    }


    private void CheckRoundings(){

        /*

        float distToNextRay = 360 / rayCount;
        float dist = 0f;

        for(int i = 0; i < rayCount; i++){

            SendRaycast();
            dist += distToNextRay;
            //Debug.Log(dist);

        }

        Ray ray = new Ray(rayOrigin.position, dir);
        Debug.DrawLine(transform.position, dir, Color.blue, Time.deltaTime * 1.2f);

        if(Physics.SphereCast(ray, rayDistance, out RaycastHit hitInfo)){

        }
        */

    }

    void TrackPlayer(GameObject player){

        Debug.Log("Tracking Player");
        state = "Tracking";

    }

    private GameObject GetClosestPlayer(){
        
        if(SceneManager.GetActiveScene().name == "MainMenu") return null;

        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        GameObject closestPlayer = null;
        float closestDistance = Vector3.Distance(transform.position, new Vector3(100000000f, 100000000f, 100000000f));

        foreach(var o in rootObjects){
            
            var controller = o.GetComponent<CharacterController>();
            if(controller != null && controller.gameObject.name != transform.gameObject.name){
                
                Debug.Log("pog");

                float temp_distance = Vector3.Distance(transform.position, o.transform.position);
                if(temp_distance < closestDistance){
                    
                    closestDistance = temp_distance;
                    closestPlayer = o;

                }

            }

        }

        Debug.Log("Closest Player is " + closestPlayer);
        Debug.Log("Distance: " + closestDistance);
        //Debug.DrawLine(this.gameObject.transform.position, closestPlayer.transform.position, Color.yellow, 10f);

        return closestPlayer;

    }

}
