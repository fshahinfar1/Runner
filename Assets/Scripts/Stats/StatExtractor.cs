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

        private float distThreshold = 1.0f;


        //private void Awake()
        //{
        //    obstacles = new List<ObstacleStat>(GameObject.FindObjectsOfType<ObstacleStat>());
        //}

        private void FixedUpdate()
        {
            GameStat newStat = new GameStat();

            Vector3 playerPos = player.transform.position;

            RoadComponent road = roadLooper.GetRoadByIndex(1); // get next up comming road
            List<Transform> obstacles = road.GetObstacles();
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
                if (dist.x > distThreshold)
                {
                    // right
                    newStat.rightDanger = Mathf.Max(newStat.rightDanger, danger);
                }
                else if (dist.x < -distThreshold)
                {
                    // left
                    newStat.leftDanger = Mathf.Max(newStat.leftDanger, danger);
                }
                else
                {
                    newStat.frontDanger = Mathf.Max(newStat.frontDanger, danger);
                }
                Debug.Log("Dist: " + dist);
                Debug.Log("Danger: " + danger);
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
                return 15;
            }
            dist = Mathf.Abs(dist);
            float danger = Mathf.Clamp(1 / dist, 0, 1);
            return danger;
        }

    }
}