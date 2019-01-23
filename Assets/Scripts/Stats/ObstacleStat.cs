using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stat
{
    public class ObstacleStat : MonoBehaviour
    {

        public Vector3 DistanceTo(Vector3 pos)
        {
            Vector3 obPos = this.transform.position;
            return (pos - obPos);
        }
    }
}