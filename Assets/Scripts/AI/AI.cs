using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIHandler))]
public class AI : MonoBehaviour
{

    public bool canSeeGoal = false;
    public float maxTrackDistance = 10f;
    public GameObject playerToFollow;
    public GameObject[] wanderPoints;

    [SerializeField] private GameObject selectedWanderPoint;

    private Vector3 lastSeenPosition;
    private bool searchSucceeded = true;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoRepath = true;
        if(playerToFollow == null) playerToFollow = GameObject.Find("LocalGamePlayer");
        GetNearestWanderPoint();
        agent.destination = selectedWanderPoint.transform.position;
    }

    void Update()
    {

        if(hasReachedWanderPoint()) agent.destination = RandomWanderPoint().transform.position;

        if(Physics.Linecast(transform.position, playerToFollow.transform.position, out RaycastHit hit)){

            Debug.DrawLine(transform.position, new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z), Color.white, Time.deltaTime);

            if(hit.transform.gameObject == playerToFollow && hit.distance <= maxTrackDistance){

                canSeeGoal = true;
                lastSeenPosition = hit.transform.position;
                SetGoal(hit.transform);
                
            } else if(!canSeeGoal || agent.pathStatus == NavMeshPathStatus.PathComplete){

                Wander();

            } else {

                canSeeGoal = false;
                Search(lastSeenPosition);

            }
        } 

    }

    void Wander(){
        //Debug.Log("Wandering around");
        float dist=agent.remainingDistance;
        if (hasReachedWanderPoint()){
            agent.destination = RandomWanderPoint().transform.position;
        } else agent.destination = selectedWanderPoint.transform.position;
        
    }

    private Vector3 GetNearestWanderPoint(){

        Vector3 closestPos = wanderPoints[0].transform.position;
        float clostestDistance = 100000000000f;

        foreach(var obj in wanderPoints){
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if(dist < clostestDistance && obj != selectedWanderPoint){
                clostestDistance = dist;
                closestPos = obj.transform.position;
                selectedWanderPoint = obj;
            }
        }

        Debug.Log("New Wanderpoint: " + selectedWanderPoint.name);

        return closestPos;
    }

    GameObject RandomWanderPoint(){
        return wanderPoints[Random.Range(0, wanderPoints.Length - 1)];
    }

    void Hunt(GameObject track){
        Debug.Log("Hunting: " + track.name);
        //agent.speed = 3f;
    }

    void Search(Vector3 lastSeenPosition){
        if(lastSeenPosition == null) return;
        Debug.Log("Searching at last seen position");
        agent.destination = lastSeenPosition;
        Wander();
    }

    void SetGoal(Transform goal){
        Debug.Log("Tracking");
        agent.destination = goal.position;
        //agent.speed = 1f;
    }

    bool hasReachedWanderPoint(){
        if(selectedWanderPoint.transform.position == transform.position || agent.remainingDistance == 0){
            return true;
        } 
        return false;
    }
}
