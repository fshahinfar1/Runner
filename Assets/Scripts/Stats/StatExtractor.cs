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
        private Rigidbody playerRigidbody;

        private GameStat stat;

        private int posMax = 10;
        private int distMax = 5;

        private void Awake()
        {
            if (player != null)
            {
                playerRigidbody = player.GetComponent<Rigidbody>();
            }
            else
            {
                Debug.LogError("Player is null!");
            }
        }

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
            newStat.dist = new int[posMax];
            for (int i=0; i<posMax; i++)
            {
                newStat.dist[i] = distMax-1;
            }

            Vector3 playerPos = player.transform.position;

            newStat.pos = GetPos(playerPos.x);
            newStat.zSpeed = playerRigidbody.velocity.z;

            List<Transform> obstacles = roadLooper.GetRoadByIndex(0).GetObstacles();
            obstacles.AddRange(roadLooper.GetRoadByIndex(1).GetObstacles());

            foreach (Transform obs in obstacles)
            {
                int pos = GetPos(obs.position.x);

                Vector3 dist = obs.position - playerPos;

                if (dist.z > 0)
                {
                    int lastDist = newStat.dist[pos];
                    int intDist = GetDist(dist.z);
                    if (intDist < lastDist)
                        newStat.dist[pos] = intDist; 
                }
            }

            return newStat;
        }

        private int GetPos(float x)
        {
            float tmp = x + 5;
            return Mathf.FloorToInt(tmp) % posMax;
        }

        private int GetDist(float z)
        {
            return Mathf.FloorToInt(z) % distMax;
        }

        public GameStat GetStat()
        {
            return stat;
        }

        public static float CalcObstDanger(float dist, Pos pos)
        {
            if (dist < 0.5 && pos != Pos.Front)
            {
                return 0;
            }
            dist = Mathf.Abs(dist);
            float danger = Mathf.Clamp(1 / dist, 0, 1);
            return danger;
        }

        public enum Pos
        {
            Left,
            Front,
            Right
        }
    }
}