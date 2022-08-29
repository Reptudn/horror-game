using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [Range(0f,1f)]
    public float Progress;
    private GameObject Infill;

    void Start()
    {
        Infill = transform.Find("Infill").gameObject;
    }

    void Update()
    {
        Infill.transform.localScale = new Vector3(Progress, 1, 1);
    }
}
