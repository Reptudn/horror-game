using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPages : MonoBehaviour
{
    
    public GameObject spawnPointsContainer;
    public GameObject[] pagesPrefabs;
    public int pagesAmount = 7;
    private Transform[] spawnPoints;


    public string interactableHintText = "Collectable Page";

    void Awake()
    {
        int children = spawnPointsContainer.transform.childCount;
        spawnPoints = new Transform[children];
        for(int i = 0; i < children; i++) spawnPoints[i] = spawnPointsContainer.transform.GetChild(i);

        //pagesPrefabs = new GameObject[pagesAmount];
    }

    void Start(){

        Spawn();

    }

    void Spawn(){

        int pagesSpawned = 0;

        while(pagesSpawned < pagesAmount){

            int random = Random.Range(0, spawnPoints.Length - 1);
            int pageRand = Random.Range(0, pagesPrefabs.Length - 1);

            if(spawnPoints[random].childCount == 0){
                Debug.Log("Spawning Page");
                Instantiate(pagesPrefabs[pageRand]);
                pagesPrefabs[pageRand].transform.SetParent(spawnPoints[random]);
                pagesPrefabs[pageRand].transform.position = spawnPoints[random].position;
                pagesSpawned++;
            }

        }

        Debug.Log("All pages have been spawned. -> " + pagesSpawned);

    }

}
