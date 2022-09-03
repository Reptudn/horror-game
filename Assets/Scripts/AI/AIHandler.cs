using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AI))]
[RequireComponent(typeof(NavMeshAgent))]
public class AIHandler : MonoBehaviour
{

    public float velocity = 0f;
    private Vector3 previousPosition;

    public Animator ai_animator;

    private NavMeshAgent agent;

    public int SpawnAfterSeconds = 1;

    void Start()
    {
        SetAIasActive();
        previousPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update(){
        Animation();
    }

    void SetAIasActive(){

        Wait(SpawnAfterSeconds);
        transform.gameObject.SetActive(true);
        Debug.Log("AI active");
    }

    private void Animation(){
        ai_animator.SetFloat("ForwardVelocity", GetVelocity());
    }

    public float GetVelocity(){
        
        Vector3 curMove = transform.position - previousPosition;
        velocity = curMove.magnitude / Time.deltaTime;
        previousPosition = transform.position;
        return velocity;

    }

    private IEnumerator Wait(int seconds) {
        yield return new WaitForSeconds(seconds);
    }

    private GameObject GetClosestPlayer(){
        
        if(SceneManager.GetActiveScene().name == "MainMenu") return null;

        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        GameObject closestPlayer = null;
        float closestDistance = Vector3.Distance(transform.position, new Vector3(100000000f, 100000000f, 100000000f));

        foreach(var o in rootObjects){
            
            var controller = o.GetComponent<CharacterController>();
            if(controller != null && controller.gameObject.name != transform.gameObject.name){

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

