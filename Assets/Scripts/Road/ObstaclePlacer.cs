using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obstacles;

public class ObstaclePlacer: MonoBehaviour
{

    private static Vector3 planeOrigin = new Vector3(-5, 0, -5);

    public void Place(GameObject road, ObstacleMap map)
    {
        //Vector3 origin = road.transform.position;
        //origin -= planeOffset;
        //Debug.Log("Origin " + origin.ToString());

        foreach (ObstacleFigure obs in map)
        {
            float z = obs.Row + (obs.Width / 2.0f);
            float x = obs.Column + (obs.Length / 2.0f);
            float y = obs.Height / 2.0f;

            Vector3 pos = planeOrigin + new Vector3(x, y, z);

            GameObject tmp = ObstacleTemplateFetcher.Fetch(obs.Type);
            tmp.transform.position = pos;

            GameObject instance = Instantiate(tmp, road.transform);
        }
    }
}
