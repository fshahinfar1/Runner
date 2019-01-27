using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stat
{
    public class GameStat
    {
        public bool lose;
        public int points;
        public int pos;
        public int height;
        public int[] dist; // [0, 4]
        public ObstType[] obstacleType;  //[0, 1]
        public float zSpeed;
        public bool canLeft;
        public bool canRight;
        public int coins;

        public GameStat()
        {
            lose = false;
            points = 0;
            pos = 0;
            height = 0;
            dist = null;
            obstacleType = null;
            zSpeed = 0;
            canLeft = false;
            canRight = false;
            coins = 0;
        }

        public GameStat(GameStat s, bool deep = false)
        {
            lose = s.lose;
            points = s.points;
            pos = s.pos;
            height = s.height;

            if (deep)
            {
                int length = s.dist.Length;
                dist = new int[length];
                System.Array.Copy(s.dist, dist, length);

                length = s.obstacleType.Length;
                obstacleType = new ObstType[length];
                System.Array.Copy(s.obstacleType, obstacleType, length);
            }
            else
            {
                dist = s.dist;
                obstacleType = s.obstacleType;
            }

            zSpeed = s.zSpeed;
            canLeft = s.canLeft;
            canRight = s.canRight;
            coins = s.coins;
        }
    }
}