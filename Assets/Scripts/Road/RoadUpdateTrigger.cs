using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obstacles;

public class RoadUpdateTrigger : MonoBehaviour {

    private RoadLooper roadLooper;
    private ObstaclePlacer obstaclePlacer;

    private ObstacleGenretor obstacleGenretor;

    private Vector3 position;

    private void Awake()
    {
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
                UpdateRoad();
            }
        }
    }

    private void UpdateRoad()
    {
        // place a road
        position = roadLooper.Place(RoadPlacerLogic.nextRoad());
        
        // move trigger to a new position
        position.z -= 2;
        transform.position = position;

        // design how obstacles should be
        ObstacleMap map = obstacleGenretor.Generate();
        // place obstacles on the new road
        GameObject road = roadLooper.GetLastRoad().gameObject;
        //obstaclePlacer.Place(road, map);
    }
}
