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

        private PoolDealer poolDealer;

        private int posMax = 10;
        private int heightMax = 2;
        private int distMax = 5;

        private int coins = 0;

        // cache
        private GameStat newStat;
        float[] minDist;


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

            // cache
            poolDealer = PoolDealer.Instance;
            poolDealer.CreatePool<int>("dist", 2, true, posMax);
            poolDealer.CreatePool<ObstType>("obstacles", 2, true, posMax);
            minDist = new float[posMax];
            newStat = new GameStat();
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
            newStat.dist = (int [])poolDealer.Get("dist");
            newStat.obstacleType = (ObstType[]) poolDealer.Get("obstacles");
            
            for (int i = 0; i < posMax; i++)
            {
                newStat.dist[i] = distMax-1;
                newStat.obstacleType[i] = ObstType.None;
                minDist[i] = float.MaxValue;
            }

            Vector3 playerPos = player.transform.position;

            newStat.pos = GetPos(playerPos.x);
            //Debug.Log("GetPos: " + newStat.pos);
            newStat.height = GetHeight(playerPos.y);
            newStat.zSpeed = playerRigidbody.velocity.z;

            List<Transform> tmp = roadLooper.GetRoadByIndex(0).GetObstacles();
            List<Transform> tmp2 = roadLooper.GetRoadByIndex(1).GetObstacles();
            List<Transform> obstacles = new List<Transform>(tmp.Count + tmp2.Count);
            obstacles.AddRange(tmp);
            obstacles.AddRange(tmp2);
            

            foreach (Transform obs in obstacles)
            {
                int pos = GetPos(obs.position.x);
                ObstacleStat obsStat = obs.GetComponent<ObstacleStat>();

                Vector3 dist = obs.position - playerPos;

                if (dist.z > 0)
                {
                    float velocity = playerRigidbody.velocity.z;
                    int intDist = GetDist(dist.z, velocity);
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

        private int GetDist(float z, float v)
        {
            int tmp = Mathf.FloorToInt(z / v * 2);
            return Mathf.Clamp(tmp, 0, distMax - 1);
        }

        public GameStat GetStat()
        {
            coins = 0;
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