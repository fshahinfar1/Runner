using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obstacles;

public class RoadUpdateTrigger : MonoBehaviour {

    private RoadLooper roadLooper;
    private ObstaclePlacer obstaclePlacer;

    private ObstacleGenretor obstacleGenretor;

    private Vector3 position;
    private Vector3 startPosition;

    public float ResetOriginThreshhold=80.0f;

    private void Awake()
    {
        startPosition = transform.position;
        roadLooper = Object.FindObjectOfType<RoadLooper>();
        obstaclePlacer = Object.FindObjectOfType<ObstaclePlacer>();
        obstacleGenretor = new ObstacleGenretor(10, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!other.isTrigger)
            {
                if (ShouldResetOrigin())
                {
                    Vector3 diff = roadLooper.ResetOrigin();
                    Vector3 pos = other.transform.position - diff;
                    other.transform.position = pos;
                }
                UpdateRoad();
            }
        }
    }

    private bool ShouldResetOrigin()
    {
        return position.z > ResetOriginThreshhold;
    }

    private void UpdateRoad()
    {
        // place a road
        roadLooper.Place(RoadPlacerLogic.nextRoad());

        position = roadLooper.GetRoadByIndex(1).GetPosition(); // next road;
        // move trigger to a new position
        position.z += 2;
        transform.position = position;

        // design how obstacles should be
        // ObstacleMap map = obstacleGenretor.Generate();
        // place obstacles on the new road
        // GameObject road = roadLooper.GetLastRoad().gameObject;
        //obstaclePlacer.Place(road, map);
    }

    public void _Reset() {
        transform.position = startPosition;
        position = startPosition;
    }
}
