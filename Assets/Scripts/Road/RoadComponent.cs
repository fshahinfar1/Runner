using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadComponent : MonoBehaviour {

    private RoadType type;
    public RoadType roadType;

    private void Awake()
    {
        type = roadType;
    }

    public RoadType GetRoadType()
    {
        return type;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}
