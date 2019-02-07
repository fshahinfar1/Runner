using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCollisionDetector : MonoBehaviour
{
    private System.Action collideAction;
    private int collisionExists;

    private void Awake()
    {
        // ignore colliding with colliders in default layer
        int defaultLayer = LayerMask.NameToLayer("Default");
        Physics.IgnoreLayerCollision(defaultLayer, defaultLayer);
        collisionExists = 0;
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
            //Vector3 dist = other.transform.position - transform.position;

            //if (dist.z < 0.95f && dist.z > 0.0f && dist.y > -0.91f)
            {
                Debug.Log("Face enter");
                collisionExists++;
                if (collideAction != null)
                {
                    collideAction.Invoke();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Debug.Log("Face exit");
            collisionExists--;
        }
    }

    public bool HasCollision()
    {
        return collisionExists != 0;
    }
}
