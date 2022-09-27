using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GenerationType{
    Small,
    Medium,
    Big,
    Huge,
    Infinite //Not infinite but nobody is going to know o.o
}

public class DungeonGenerator : MonoBehaviour
{

    public GenerationType generationType;
    public GameObject[] rooms;
    public GameObject[] eventRooms;

    void Awake(){

        switch(generationType){

            case GenerationType.Small:
                Generate(10, 35);
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

        int eventRoomsPlaced = 0;
        int roomsPlaced = 0;

        while(eventRoomsPlaced < eventRooms.Length){

            if(roomsPlaced > maxRooms) break;

            int random = Random.Range(1, 10);
            if (random < 3) { SpawnEventRoom(eventRooms[eventRoomsPlaced]); eventRoomsPlaced++; }
            else SpawnRoom(rooms[Random.Range(0, rooms.Length -1)]);

            roomsPlaced++;

        }

    }

    void SpawnEventRoom(GameObject room){

    }

    GameObject lastSpawnedRoom;
    GameObject[] snappingPoints;
    void SpawnRoom(GameObject room){
        if(room == lastSpawnedRoom) { SpawnRoom(rooms[Random.Range(0, rooms.Length -1)]); return; }

        var roomInfo = room.GetComponent<RoomInfo>();

        if(roomInfo == null){
            Debug.Log("The Room: " + room + " doesnt have a RoomInfo script");
            return;
        }

        if(!CanSpawnRoom(room)) { 
            //hier machen wa was wenn man des ned do hinsetzen kann... for now mucho recursiono
            SpawnRoom(rooms[Random.Range(0, rooms.Length -1)]); return;
        } else {

            //da machen wa auch was wenn man des da hinsetzen kann

        }
        
        snappingPoints = roomInfo.snappingPoints;
        lastSpawnedRoom = room;
    }

    bool CanSpawnRoom(GameObject room){
        foreach(GameObject o in snappingPoints){
            if(room.transform.position == o.transform.position) return false;
        }
        return true;
    }

    void SetPosition(){

    }
}
