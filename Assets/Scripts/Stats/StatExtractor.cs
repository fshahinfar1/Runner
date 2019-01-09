﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatExtractor : MonoBehaviour {

    public GameObject player;

    private List<ObstacleStat> obstacles;

    private void Awake()
    {
        obstacles = new List<ObstacleStat>(GameObject.FindObjectsOfType<ObstacleStat>());
    }

    private void Update()
    {
        Vector3 playerPos = player.transform.position;
        foreach (ObstacleStat ob in obstacles)
        {
            Vector3 dist = ob.DistanceTo(playerPos);
            Debug.Log(dist.ToString());
        }
    }

}
