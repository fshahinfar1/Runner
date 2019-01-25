using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stat
{
    public enum ObstType
    {
        cube,
        Tall,
    }

    public class ObstacleStat : MonoBehaviour
    {

        public ObstType type;


        public Vector3 DistanceTo(Vector3 pos)
        {
            Vector3 obPos = this.transform.position;
            return (pos - obPos);
        }

        public ObstType GetObsType()
        {
            return type;
        }
    }
}