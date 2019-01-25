using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stat
{
    public struct GameStat
    {
        public bool lose;
        public int points;
        public int pos;
        public int height;
        public int[] dist; // [0, 4]
        public float zSpeed;
        public bool canLeft;
        public bool canRight;

        public GameStat(GameStat s)
        {
            lose = s.lose;
            points = s.points;
            pos = s.pos;
            height = s.height;

            int length = s.dist.Length;
            dist = new int[length];
            System.Array.Copy(s.dist, dist, length);

            zSpeed = s.zSpeed;
            canLeft = s.canLeft;
            canRight = s.canRight;
        }
    }
}