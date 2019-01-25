using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AI
{
    public class StateMoveValueDisplay : MonoBehaviour
    {
        private int size;
        Text[] txts;
        private void Awake()
        {
            size = transform.childCount;
            txts = new Text[size];
            int k = 0;
            foreach (Transform t in transform)
            {
                txts[k] = t.GetChild(0).GetComponent<Text>();
                k++;
            }
        }

        public void Set(float[] vals)
        {
            if (vals.Length < size)
            {
                throw new System.Exception("size is smaller!");
            }
            for (int i = 0; i < size; i++)
            {
                txts[i].text = vals[i].ToString();
            }
        }

    }
}