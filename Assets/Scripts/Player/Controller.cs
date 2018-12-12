using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    [RequireComponent(typeof(PlayerPhysicsConfig))]
    public class Controller : MonoBehaviour
    {

        private new Rigidbody rigidbody;
        private PlayerState state;

        private void Awake()
        {
            rigidbody = transform.GetComponent<Rigidbody>();
            state = PlayerState.GetInstance();
        }

        void Update()
        {
            state.SetVelocity(rigidbody.velocity);

            float horizontalNormal = Input.GetAxis("Horizontal");
            float verticalNormal = Input.GetAxis("Vertical");
            float jumpNormal = Input.GetAxis("Jump");


            state.updateHorizontalSpeed(horizontalNormal);
            state.updateVerticalSpeed(verticalNormal);
            state.Jump(jumpNormal);

            rigidbody.velocity = state.GetVelocity();
        }
    }
}