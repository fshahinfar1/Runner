using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class FeatureExtractor
    {
        public static float[] Extract(Stat.GameStat stat)
        {
            int idxAfterDist = stat.dist.Length;

            float[] feature = new float[idxAfterDist+2];

            System.Array.Copy(stat.dist, 0, feature, 0, stat.dist.Length);
            feature[idxAfterDist] = stat.pos;
            feature[idxAfterDist + 1] = stat.offset;

            return feature;
        }
    }
}