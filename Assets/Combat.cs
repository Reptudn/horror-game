using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GetItemInMainHand() == null) return;
            GameObject anchor = GetItemInMainHand();
            var child = anchor.transform.GetChild(0);
            if (child == null) return;
            child.BroadcastMessage("Attack");
        }
    }

    private GameObject GetItemInMainHand()
    {

        GameObject obj = GameObject.Find("ItemAnchor");

        return obj != null ? obj : null;

    }
}
