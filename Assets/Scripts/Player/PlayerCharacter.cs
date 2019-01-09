using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class PlayerCharacter : MonoBehaviour, Character
    {
        public PlayerState state;
        private Rigidbody rigidbody;

        private void Awake()
        {
            state = new PlayerState();
            rigidbody = GetComponent<Rigidbody>();

            // register player to controller
            GetComponent<Controller>().player = this;
        }

        private void Update()
        {
            // update state velocity
            state.SetVelocity(rigidbody.velocity);
            rigidbody.velocity = state.GetVelocity();
        }

        public void HorizontalMove(float magnitude)
        {
            state.updateHorizontalSpeed(magnitude);
            rigidbody.velocity = state.GetVelocity();
        }

        public void VerticalMove(float magnitude)
        {
            state.updateVerticalSpeed(magnitude);
            rigidbody.velocity = state.GetVelocity();
        }

        public void Jump()
        {
            state.Jump(1);
            rigidbody.velocity = state.GetVelocity();
        }
    }
}