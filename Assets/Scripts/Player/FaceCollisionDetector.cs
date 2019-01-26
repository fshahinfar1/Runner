using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCollisionDetector : MonoBehaviour
{
    private System.Action collideAction;

    private void Awake()
    {
        // ignore colliding with colliders in default layer
        int defaultLayer = LayerMask.NameToLayer("Default");
        Physics.IgnoreLayerCollision(defaultLayer, defaultLayer);
    }

    public void Register (System.Action action)
    {
        collideAction += action;
    }

    public void Unregister (System.Action action)
    {
        collideAction -= action;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Vector3 dist = other.transform.position - transform.position;
            Debug.Log(dist.x);
            Debug.Log(dist.y);
            Debug.Log(dist.z);

            if (dist.z < 0.91f && dist.z > 0.0f && dist.y > -0.91f)
            {
                if (collideAction != null)
                {
                    collideAction.Invoke();
                }
            }
        }
    }
}
