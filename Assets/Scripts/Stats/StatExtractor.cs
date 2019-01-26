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
        private int heightMax = 2;
        private int distMax = 5;

        private int coins = 0;

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
            Observer.GetInstance().Register(Observer.Event.CoinCollection, OnCoinCollect);
        }

        private void FixedUpdate()
        {
            stat = Extract();
            if (display != null)
            {
                display.Display(stat);
            }
        }

        private void OnCoinCollect()
        {
            coins++;
        }

        private GameStat Extract()
        {
            GameStat newStat = new GameStat();
            newStat.dist = new int[posMax];
            newStat.obstacleType = new ObstType[posMax];
            float[] minDist = new float[posMax];
            for (int i = 0; i < posMax; i++)
            {
                newStat.dist[i] = distMax-1;
                newStat.obstacleType[i] = ObstType.None;
                minDist[i] = float.MaxValue;
            }

            Vector3 playerPos = player.transform.position;

            newStat.pos = GetPos(playerPos.x);
            newStat.height = GetHeight(playerPos.y);
            newStat.zSpeed = playerRigidbody.velocity.z;

            List<Transform> obstacles = roadLooper.GetRoadByIndex(0).GetObstacles();
            obstacles.AddRange(roadLooper.GetRoadByIndex(1).GetObstacles());

            foreach (Transform obs in obstacles)
            {
                int pos = GetPos(obs.position.x);
                ObstacleStat obsStat = obs.GetComponent<ObstacleStat>();

                Vector3 dist = obs.position - playerPos;

                if (dist.z > 0)
                {
                    // int lastDist = newStat.dist[pos];
                    int intDist = GetDist(dist.z);
                    if (dist.z < minDist[pos])
                    {
                        minDist[pos] = dist.z;
                        newStat.dist[pos] = intDist;
                        newStat.obstacleType[pos] = obsStat.GetObsType();
                        obsStat.SetText(pos.ToString());
                    }
                }
            }

            newStat.coins = coins;
            coins = 0;

            return newStat;
        }

        private int GetPos(float x)
        {
            float tmp = x + 5f;
            return Mathf.FloorToInt(tmp);
        }

        private int GetHeight(float y)
        {
            return Mathf.Clamp(Mathf.FloorToInt(y), 0, heightMax - 1);
        }

        private int GetDist(float z)
        {
            return Mathf.Clamp(Mathf.FloorToInt(z), 0, distMax - 1);
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