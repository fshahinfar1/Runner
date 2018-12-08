using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLooper : MonoBehaviour
{

    public GameObject trigger;
    private Transform plane;
    private Transform otherPlane;

    private float planeWidth = 10;

    private Vector3 position;

    private void Awake()
    {
        plane = transform.GetChild(0);
        otherPlane = transform.GetChild(1);
    }

    public Vector3 Place()
    {
        position = otherPlane.position;
        position.z += planeWidth - 1;
        plane.position = position;
        // Swap
        Transform tmp = otherPlane;
        otherPlane = plane;
        plane = tmp;
        return position;
    }
}
