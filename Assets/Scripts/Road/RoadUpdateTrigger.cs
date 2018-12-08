using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadUpdateTrigger : MonoBehaviour {

    private RoadLooper roadLooper;

    private Vector3 position;

    private void Awake()
    {
        roadLooper = transform.parent.GetComponent<RoadLooper>();
    }

    private void OnTriggerEnter(Collider other)
    {
        position = roadLooper.Place();
        position.z -= 2;
        transform.position = position; 
    }
}
