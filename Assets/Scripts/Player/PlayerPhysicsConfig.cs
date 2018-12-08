using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerPhysicsConfig : MonoBehaviour
    {

        private new Rigidbody rigidbody;


        private void Awake()
        {
            rigidbody = transform.GetComponent<Rigidbody>();
            rigidbody.mass = 10;
            rigidbody.useGravity = true;
            rigidbody.drag = 0.1f;

            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}