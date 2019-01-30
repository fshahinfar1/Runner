using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AI
{
    public class StateMoveValueDisplay : MonoBehaviour
    {
        public Transform valuesPanel;
        public Transform rewardObj;

        private int size;
        Text[] txts;
        Text reward;

        private void Awake()
        {
            size = valuesPanel.childCount;
            txts = new Text[size];
            int k = 0;
            foreach (Transform t in valuesPanel)
            {
                txts[k] = t.GetChild(0).GetComponent<Text>();
                k++;
            }

            reward = rewardObj.GetChild(0).GetComponent<Text>();
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

        public void SetReward(float r)
        {
            reward.text = r.ToString();
        }

    }
}