using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Stat
{
    public class StatExtractor : MonoBehaviour
    {

        public GameObject player;
        public RoadLooper roadLooper;
        public StatDisplay display;

        //private List<ObstacleStat> obstacles;

        private GameStat stat;

        private float distThreshold = 0.6f;
        private float distMaxThreshold = 1.9f;


        //private void Awake()
        //{
        //    obstacles = new List<ObstacleStat>(GameObject.FindObjectsOfType<ObstacleStat>());
        //}

        private void FixedUpdate()
        {
            GameStat newStat = new GameStat();

            Vector3 playerPos = player.transform.position;

            float leftWallDist = -4.5f - playerPos.x;
            if (-leftWallDist < distMaxThreshold)
            {
                newStat.leftDanger = CalcObstDanger(leftWallDist);
            }

            float rightWallDist = 4.5f - playerPos.x;
            if (rightWallDist < distMaxThreshold)
            {
                newStat.rightDanger = CalcObstDanger(rightWallDist);
            }

            RoadComponent road = roadLooper.GetRoadByIndex(0); // get current road comming road
            List<Transform> obstacles = road.GetObstacles();
            obstacles.AddRange(roadLooper.GetRoadByIndex(1).GetObstacles()); // check next comming road
            foreach (Transform t in obstacles)
            {
                ObstacleStat ob = t.GetComponent<ObstacleStat>();
                if (ob == null)
                {
                    Debug.LogError("obstacle stat not found!");
                    //t.gameObject.SetActive(false);
                    continue;
                }
                Vector3 dist = -ob.DistanceTo(playerPos);
                float danger = CalcObstDanger(dist.z);
                if (dist.x > distThreshold && dist.x < distMaxThreshold)
                {
                    // right
                    newStat.rightDanger = Mathf.Max(newStat.rightDanger, danger);
                }
                else if (-dist.x > distThreshold && -dist.x < distMaxThreshold)
                {
                    // left
                    newStat.leftDanger = Mathf.Max(newStat.leftDanger, danger);
                }
                else if (dist.x < distThreshold && dist.x > -distThreshold)
                {
                    // front
                    newStat.frontDanger = Mathf.Max(newStat.frontDanger, danger);
                }
                //Debug.Log("Dist: " + dist);
                //Debug.Log("Danger: " + danger);
            }

            stat = newStat;
            if (display != null)
            {
                display.Display(newStat);
            }
        }

        public GameStat GetStat()
        {
            return stat;
        }

        public static float CalcObstDanger(float dist)
        {
            if (dist == 0)
            {
                Debug.LogWarning("CalcObstDanger: dist is zero!");
                return 1;
            }
            dist = Mathf.Abs(dist);
            float danger = Mathf.Clamp(1 / dist, 0, 1);
            return danger;
        }

    }
}