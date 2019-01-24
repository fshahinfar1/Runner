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

        private GameStat stat;

        private float distThreshold = 0.5f;
        private float distMaxThreshold = 1f;

        private int posMax = 10;

        private void FixedUpdate()
        {
            stat = Extract();
            if (display != null)
            {
                display.Display(stat);
            }
        }

        private GameStat Extract()
        {
            GameStat newStat = new GameStat();

            Vector3 playerPos = player.transform.position;

            newStat.pos = Mathf.FloorToInt(playerPos.x + 5) % posMax;
            newStat.offset = playerPos.x - (newStat.pos - 4);
            newStat.dist = new float[posMax];

            RoadComponent road = roadLooper.GetRoadByIndex(0); // get current road comming road
            List<Transform> obstacles = road.GetObstacles();
            obstacles.AddRange(roadLooper.GetRoadByIndex(1).GetObstacles()); // check next comming road
            foreach (Transform t in obstacles)
            {
                Vector3 dist = t.position - playerPos;

                if (dist.z > 0)
                {
                    int pos = Mathf.FloorToInt(t.position.x + 5) % posMax;

                    float lastDist = newStat.dist[pos];
                    newStat.dist[pos] = Mathf.Min(lastDist, Mathf.Clamp(dist.z/20, 0, 1));
                }
            }
            return newStat;
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