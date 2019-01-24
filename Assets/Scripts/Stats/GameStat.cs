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
        public float offset;
        public float[] dist;
        public float zSpeed;

        public GameStat(GameStat s)
        {
            lose = s.lose;
            points = s.points;
            pos = s.pos;
            offset = s.offset;
            dist = s.dist;
            zSpeed = s.zSpeed;
        }
    }
}