using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum GenerationType{
    Small,
    Medium,
    Big,
    Huge,
    Infinite //Not infinite but nobody is going to know o.o
}

public class DungeonGenerator : MonoBehaviour
{

    [Header("Level Generation")]
    public GenerationType generationType;
    public GameObject[] rooms;
    public GameObject[] eventRooms;

    public GameObject baseSnapPoint;

    public List<GameObject> snappingPoints;

    [Header("NavMesh Generation")]
    public GameObject worldContainer;
    public NavMeshBuildSettings buildSettings;
    public Bounds bounds;
    public NavMeshBuildSourceShape navMeshShape = NavMeshBuildSourceShape.Box;

    void Start(){

        lastSpawnedRoom = transform.gameObject;

        if(worldContainer == null) worldContainer = transform.gameObject;

        Debug.Log("Generating Map with size: " + generationType);

        switch(generationType){

            case GenerationType.Small:
                Generate(10, 30);
                break;

            case GenerationType.Medium:
                Generate(40, 70);
                break;

            case GenerationType.Big:
                Generate(80, 120);
                break;

            case GenerationType.Huge: 
                Generate(150, 200);
                break;

            default:
                Generate(400, 1000);
                break;

        }

    }

    void Generate(int minRooms, int maxRooms){

        Debug.Log("Generation started.");   

        int eventRoomsPlaced = 0;
        int roomsPlaced = 0;

        while(eventRoomsPlaced < eventRooms.Length){

            if(roomsPlaced > maxRooms) break;

            int random = UnityEngine.Random.Range(1, 10);
            if (random < 3) { SpawnEventRoom(eventRooms[eventRoomsPlaced]); eventRoomsPlaced++; }
            else SpawnRoom(rooms[UnityEngine.Random.Range(0, rooms.Length -1)]);

            Debug.Log("Rooms placed: " + ( roomsPlaced + 1 ));
            roomsPlaced++;

        }

        GenerateNavMesh(worldContainer);

        Debug.Log("Level generation done.");
        
    }

    void SpawnEventRoom(GameObject room){

        Debug.Log("Spawning Event Room");

        //des muss später entfernt werden
        SpawnRoom(room);
    }

    GameObject lastSpawnedRoom;
    void SpawnRoom(GameObject room){
        //if(room == lastSpawnedRoom) { Debug.Log("Recursion"); SpawnRoom(rooms[UnityEngine.Random.Range(0, rooms.Length -1)]); return; }

        Debug.Log("Spawning Normal Room");

        RoomInfo roomInfo = room.GetComponent<RoomInfo>();

        if(roomInfo == null){
            Debug.Log("The Room: " + room + " doesnt have a RoomInfo script");
            return;
        }

        if(AvailableSnappingPoints() == null) { 
            //hier machen wa was wenn man des ned do hinsetzen kann... for now mucho recursiono
            //SpawnRoom(rooms[UnityEngine.Random.Range(0, rooms.Length -1)]); return;

            Debug.Log("Instantiate (no available snipping points)");
            Instantiate(room);
            SetPosition(room);

        } else {

            //da machen wa auch was wenn man des da hinsetzen kann
            Debug.Log("Instantiate");
            Instantiate(room);
            SetPosition(room);

        }
        
        Debug.Log("Room spawned: " + roomInfo.name + " (" + room.name + ")");
        snappingPoints = roomInfo.snappingPoints.ToList();
        lastSpawnedRoom = room; 

    }

    [Obsolete("Use AvailableSnappingPoints instead")]
    bool CanSpawnRoom(GameObject room){
        foreach(GameObject o in snappingPoints){
            if(room.transform.position == o.transform.position) return false;
        }
        return true;
    }

    GameObject[] AvailableSnappingPoints(){
        
        List<GameObject> points = new List<GameObject>();

        foreach(GameObject snap in snappingPoints){

            if(snap.GetComponent<SnapPoint>().available) points.Add(snap); 

        }

        if(points.Count == 0) return null;

        return points.ToArray();
    }

    void SetPosition(GameObject room){

        GameObject[] snap = AvailableSnappingPoints();

        int rand = UnityEngine.Random.Range(0, snap.Length -1);
        room.transform.position = snap[rand].transform.position + room.transform.position;

        snap[rand].GetComponent<SnapPoint>().available = false;

        room.transform.SetParent(worldContainer.transform); //set room as child of world container

        Debug.Log("Position set");
    }

    void GenerateNavMesh(GameObject world){
        Debug.Log("Generating NavMesh");

        List<NavMeshBuildSource> source = new List<NavMeshBuildSource>(); //https://docs.unity3d.com/ScriptReference/AI.NavMeshBuildSource.html
        source.Add(BuildSource());

        NavMeshBuilder.BuildNavMeshData(buildSettings, source, bounds, world.transform.position, world.transform.localRotation);

        Debug.Log("NavMesh generation done");
    }

    NavMeshBuildSource BuildSource(){
        var s = new NavMeshBuildSource();

        s.transform = transform.localToWorldMatrix;
        s.shape     = navMeshShape;
        //s.size      = 

        return s;
    }

    IEnumerator Wait(float seconds) { yield return new WaitForSeconds(seconds); }
}
