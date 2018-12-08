using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLooper : MonoBehaviour
{
    private Queue<Transform> planeQueue;

    private float planeWidth = 10;

    private Vector3 position;

    private void Awake()
    {
        planeQueue.Enqueue(transform.GetChild(0));
        planeQueue.Enqueue(transform.GetChild(1));
    }

    public Vector3 Place()
    {
        Transform plane = planeQueue.Dequeue();
        position = plane.position;
        position.z += planeWidth - 1;
        plane.position = position;
        planeQueue.Enqueue(plane);
        return position;
    }
}
