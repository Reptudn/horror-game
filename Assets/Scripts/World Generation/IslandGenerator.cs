using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerator : MonoBehaviour
{

    public GameObject[] rooms;
    public GameObject[] eventRooms;

    public float waterLevel = 0.4f;
    public float scale = 0.1f;

    [Range(1, 200)]
    public int mapSize = 100;

    Cell[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        float[,] noiseMap = new float[mapSize, mapSize];
        float xOffset = Random.Range(-1000000f, 1000000f);
        float yOffset = Random.Range(-1000000f, 1000000f);
        for(int y = 0; y < mapSize; y++){
            for(int x = 0; x < mapSize; x++){
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                noiseMap[x, y] = noiseValue;
            }    
        }

        float[,] falloffMap = new float[mapSize, mapSize];
        for(int y = 0; y < mapSize; y++){
            for(int x = 0; x < mapSize; x++){
                float xv = x / (float)mapSize * 2 - 1;
                float yv = y / (float)mapSize * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }    
        }

        grid = new Cell[mapSize, mapSize];
        for(int y = 0; y < mapSize; y++){
            for(int x = 0; x < mapSize; x++){
                Cell cell = new Cell();
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloffMap[x, y];
                cell.isWater = noiseValue < waterLevel;
                grid[x, y] = cell;
            }    
        }
    }

    void OnDrawGizmos(){

        if(!Application.isPlaying) return;
        for(int y = 0; y < mapSize; y++){
            for(int x = 0; x < mapSize; x++){
                Cell cell = grid[x, y];
                if(cell.isWater){
                    Gizmos.color = Color.blue;
                } else {
                    Gizmos.color = Color.green;
                }
                Vector3 pos = new Vector3(x, 0, y);

                int rand = Random.Range(0, 10);
                Instantiate(rooms[Random.Range(0, rooms.Length - 1)]);

                //if(rand < 2) { Instantiate(eventRooms[Random.Range(0, eventRooms.Length - 1)]) as GameObject; }
                //else Instantiate(rooms[Random.Range(0, rooms.Lenght - 1)]) as GameObject;
            }
        }    

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
