using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLooper : MonoBehaviour
{
    private Queue<Transform> planeQueue = new Queue<Transform>();

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
        Transform nextPlane = planeQueue.Peek();
        position = nextPlane.position;
        position.z += planeWidth - 1;
        plane.position = position;  // place current plane after next plane
        planeQueue.Enqueue(plane);  // put current plane back to queue
        return position;
    }
}
