using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKBasic : MonoBehaviour
{

    public LayerMask mask;
    public GameObject track;

    // Start is called before the first frame update
    void Start()
    {
        if(track == null) track = transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(track.transform.position, -Vector3.up, out RaycastHit hit)){

            Debug.DrawLine(track.transform.position, -Vector3.up, Color.magenta, Time.deltaTime);
            
            track.transform.rotation = hit.transform.rotation;
            track.transform.position = new Vector3(track.transform.position.x, hit.transform.position.y + transform.localScale.y / 2 + 0.2f, track.transform.position.z);
        }
    }
}
