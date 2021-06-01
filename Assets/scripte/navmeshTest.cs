using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class navmeshTest : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;
    public List<Transform> Destination;
    private Transform _setdestination;


    public void Start()
    {
        _setdestination = Destination[Random.Range(0, Destination.Count)];
    }

    public void Update()
    {
        if ((transform.position - _setdestination.position).magnitude < 2)
        {
            _setdestination = Destination[Random.Range(0, Destination.Count)];
        }
        
        
        NavMeshAgent.destination = _setdestination.position;
    }
}
