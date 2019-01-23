using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class FeatureExtractor
    {
        public static float[] Extract(Stat.GameStat stat)
        {
            float[] feature = new float[3] 
            {stat.leftDanger, stat.frontDanger, stat.rightDanger };

            return feature;
        }
    }
}