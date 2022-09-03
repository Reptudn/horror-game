using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{

    public bool canSeeGoal = false;
    NavMeshAgent agent;
    public GameObject playerToFollow;
    public GameObject[] wanderPoints;

    private Vector3 lastSeenPosition;
    private bool searchSucceeded = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoRepath = true;
        if(playerToFollow == null) playerToFollow = GameObject.Find("Goal");
    }

    void Update()
    {
        
        if(Physics.Linecast(transform.position, playerToFollow.transform.position, out RaycastHit hit)){

            Debug.DrawLine(transform.position, new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z), Color.white, Time.deltaTime);

            if(hit.transform.gameObject == playerToFollow){

                canSeeGoal = true;
                lastSeenPosition = hit.transform.position;
                SetGoal(hit.transform);
                
            } else {

                canSeeGoal = false;
                Search(lastSeenPosition);

            }
        }

    }

    void Wander(){
        Debug.Log("Wandering around");
        float dist=agent.remainingDistance;
        if (agent.pathStatus == NavMeshPathStatus.PathComplete){
            agent.destination = wanderPoints[Random.Range(0, wanderPoints.Length - 1)].transform.position;
        }

    }

    private Vector3 GetNearestWanderPoint(){

        Vector3 closestPos = wanderPoints[0].transform.position;
        float clostestDistance = 100000000000f;

        foreach(var obj in wanderPoints){
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if(dist < clostestDistance && obj.transform.position != transform.position){
                clostestDistance = dist;
                closestPos = obj.transform.position;
            }
        }

        return closestPos;
    }

    void Hunt(GameObject track){
        Debug.Log("Hunting: " + track.name);
    }

    void Search(Vector3 lastSeenPosition){
        if(lastSeenPosition == null) return;
        Debug.Log("Searching at last seen position");
        agent.destination = lastSeenPosition;
    }

    void SetGoal(Transform goal){
        Debug.Log("Tracking");
        agent.destination = goal.position;
    }
}
