using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadUpdateTrigger : MonoBehaviour {

    private RoadLooper roadLooper;

    private Vector3 position;

    private void Awake()
    {
        roadLooper = Object.FindObjectOfType<RoadLooper>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!other.isTrigger)
            {
                position = roadLooper.Place(RoadPlacerLogic.nextRoad());
                position.z -= 2;
                transform.position = position;
            }
        }
    }
}
