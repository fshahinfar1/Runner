using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class RemoveNAN : MonoBehaviour
{
    private void Awake()
    {
        string filename = "QMat";
        float[,,,,] oldKnowledge = DataStore.Load(filename); // 10,2,5,4,4
        float[,,,,] newKnowledge = new float[10, 2, 5, 4, 4];
        for (int pos = 0; pos < 10; pos++)
        {
            for (int height = 0; height < 2; height++)
            {
                for (int dist = 0; dist < 5; dist++)
                {
                    for (int move = 0; move < 4; move++)
                    {
                        for (int obst = 0; obst < 4; obst++)
                        {
                            float qval = oldKnowledge[pos, height, dist, move, obst];
                            if (qval == float.NaN)
                            {
                                newKnowledge[pos, height, dist, move, obst] = 0;
                            }
                            else
                            {
                                newKnowledge[pos, height, dist, move, obst] = qval;
                            }
                        }
                    }
                }
            }
        }
        DataStore.Store(newKnowledge, filename);
        Debug.Log("Done!");
    }
}
