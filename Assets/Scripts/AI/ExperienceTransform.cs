using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class ExperienceTransform: MonoBehaviour
{
    private void Awake()
    {
        string filename = "QMat";
        float[,,,,] oldKnowledge = DataStore.Load(filename); // 10,2,5,4,3
        float[,,,,] newKnowledge = new float[10, 2, 5, 4, 4];
        for (int pos = 0; pos < 10; pos++)
        {
            for (int height = 0; height < 2; height++)
            {
                for (int dist = 0; dist < 5; dist++)
                {
                    for (int move = 0; move < 4; move++)
                    {
                        for (int obst = 0; obst < 3; obst++)
                        {
                            newKnowledge[pos, height, dist, move, obst] = 
                                oldKnowledge[pos, height, dist, move, obst];
                        }
                        newKnowledge[pos, height, dist, move, 3] = 0;
                    }
                }
            }
        }
        DataStore.Store(newKnowledge, filename);
        Debug.Log("Done!");
    }
}
