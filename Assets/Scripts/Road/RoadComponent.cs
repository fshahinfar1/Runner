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

    public List<Transform> GetObstacles()
    {
        Transform obstacles = transform.Find("Obstacles");
        List<Transform> result = new List<Transform>(obstacles.childCount);
        foreach (Transform obs in obstacles)
        {
            result.Add(obs);
        }
        return result;
    }
}
