using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stat
{
    public struct GameStat
    {
        public bool lose;
        public int points;
        public float frontDanger;
        public float leftDanger;
        public float rightDanger;
        public float zSpeed;

        public GameStat(GameStat s)
        {
            lose = s.lose;
            points = s.points;
            frontDanger = s.frontDanger;
            leftDanger = s.leftDanger;
            rightDanger = s.rightDanger;
            zSpeed = s.zSpeed;
        }
    }
}