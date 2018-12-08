using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    public GameObject followed;

    public float distance;
    public float height;

    private Vector3 position;

    private void Update()
    {
        position = followed.transform.position;
        // keep the height
        position.y += height;
        position.z -= distance;
        transform.position = position;
    }
}
