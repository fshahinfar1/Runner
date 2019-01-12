using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {
    public Transform leader;

    public bool followX;
    public bool followY;
    public bool followZ;

    public float distX;
    public float distY;
    public float distZ;


    private void Update()
    {
        Vector3 leaderPos = leader.position;
        Vector3 nextPos = transform.position;

        if (followX)
        {
            nextPos.x = leaderPos.x - distX;
        }

        if (followY)
        {
            nextPos.y = leaderPos.y - distY;
        }

        if (followZ)
        {
            nextPos.z = leaderPos.z - distZ;
        }

        transform.position = nextPos;
    }
}
