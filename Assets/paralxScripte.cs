using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paralxScripte : MonoBehaviour
{
    [Range(0, 100)] public float mouseMove; 
    public GameObject Plan1;
    public GameObject Plan2;
    public GameObject Plan3;
    public GameObject Plan4;

    public void Update()
    {
        Plan1.transform.position = new Vector3(0, mouseMove);
        Plan2.transform.position = new Vector3(0, -mouseMove);
        Plan3.transform.position = new Vector3(0, mouseMove/0.5f);
        Plan4.transform.position = new Vector3(0, mouseMove/0.25f);
    }
}
