using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Stat
{
    public enum ObstType
    {
        cube,
        Tall,
        None,
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

        public void SetText(string text)
        {
            Transform t = transform.Find("Text");
            if (t != null)
            {
                t.GetComponent<TextMeshPro>().SetText(text);
            }
        }
    }
}