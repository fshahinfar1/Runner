using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    [RequireComponent(typeof(PlayerPhysicsConfig))]
    public class Controller : MonoBehaviour
    {

        private new Rigidbody rigidbody;
        private Vector3 velocity;

        private float maxPartialSpeed = 3;

        private void Awake()
        {
            rigidbody = transform.GetComponent<Rigidbody>();
        }

        void Update()
        {

            float verticalNormal = Input.GetAxis("Vertical");
            float horizontalNormal = Input.GetAxis("Horizontal");

            velocity = rigidbody.velocity;
            velocity.z = maxPartialSpeed * verticalNormal;
            velocity.x = maxPartialSpeed * horizontalNormal;
            rigidbody.velocity = velocity;
        }
    }
}