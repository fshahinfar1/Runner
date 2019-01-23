using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayCall
{
    public static Coroutine Call(MonoBehaviour mono, System.Action function, float delay)
    {
        return mono.StartCoroutine(Delay(delay, function));
    }

    public static void Discard(MonoBehaviour mono, Coroutine c)
    {
        mono.StopCoroutine(c);
    }

    private static IEnumerator Delay(float delay, System.Action function)
    {
        yield return new WaitForSeconds(delay);
        if (function != null)
            function.Invoke();
    }
}
