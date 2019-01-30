using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadComponent : MonoBehaviour {

    private RoadType type;
    public RoadType roadType;

    private List<GameObject> coins;
    private List<Transform> obstacles;

    private void Awake()
    {
        type = roadType;

        Transform tmp = transform.Find("Coins");
        Transform obstacleParent = transform.Find("Obstacles");
        coins = new List<GameObject>(tmp.childCount);
        obstacles = new List<Transform>(obstacleParent.childCount + tmp.childCount);
        foreach(Transform t in tmp)
        {
            coins.Add(t.gameObject);
            obstacles.Add(t);
        }
        foreach(Transform t in obstacleParent)
        {
            obstacles.Add(t);
        }
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
        return obstacles;
    }

    public void Place()
    {
        SetActive(true);
        foreach(GameObject g in coins)
        {
            g.SetActive(true);
        }
    }
}
