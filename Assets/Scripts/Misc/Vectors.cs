using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vectors
{
    public static float Dot(float[] a, float[] b)
    {
        if (a.Length != b.Length)
        {
            throw new System.Exception("dot: two vector lenght should be equal!");
        }
        float result = 0;
        for (int i=0; i<a.Length; i++)
        {
            result += a[i] * b[i];
        }
        return result;
    }
}
