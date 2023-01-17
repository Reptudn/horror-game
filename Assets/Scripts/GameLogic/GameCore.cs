using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : MonoBehaviour
{

    [SerializeField] GameObject[] players;

    public int money
    {
        get { return money; }
        set
        {
            if (value > 0) money = value;
            else UnityEngine.Debug.Log("Money set Value must be bigger than 0");
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameFinish()
    {

    }
}
