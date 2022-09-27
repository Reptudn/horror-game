using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GenerationType{
    Small,
    Medium,
    Big,
    Huge,
    Infinite
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



    }
}
