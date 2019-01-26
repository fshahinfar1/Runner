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
        if (other.gameObject.tag == "Obstacle" && transform.position.y - other.transform.position.y < 0.9f)
        {
            if (collideAction != null)
            {
                collideAction.Invoke();
            }
        }
    }
}
