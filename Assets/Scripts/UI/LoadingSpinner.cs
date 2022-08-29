using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSpinner : MonoBehaviour
{
    
    public float RotateSpeed = 200f;
    private Image Component;

    void Start()
    {
        Component = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Component.rectTransform.Rotate(0f, 0f, -(RotateSpeed * 5) * Time.deltaTime);
    }
}
