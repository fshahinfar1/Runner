using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class PlayerCharacter : MonoBehaviour, CharacterInput
    {
        public PlayerState state;
        private Rigidbody rigidbody;

        private void Awake()
        {
            state = new PlayerState();
            rigidbody = GetComponent<Rigidbody>();

            // register player to controller
            GetComponent<Controller>().player = this;

            // register to ground detector
            GroundDetector gd = GetComponent<GroundDetector>();
            if (gd != null)
            {
                gd.RegisterOnGroundHit(HitGround);
                gd.RegisterOnGroundLeave(LeaveGround);
            }
            else
            {
                Debug.LogError("Ground detector not found!");
            }

            // check player Mode
            if (gd.GroundState())
            {
                state.SetMode(Mode.Ground);
            }
            else
            {
                state.SetMode(Mode.Air);
            }
        }

        private void FixedUpdate()
        {
            state.SetVelocity(rigidbody.velocity);
            state.updateVerticalSpeed(1);
            rigidbody.velocity = state.GetVelocity();
        }

        // Ground detection
        private void HitGround()
        {
            state.UpdateCanJump(true);
            state.SetMode(Mode.Ground);
        }

        private void LeaveGround()
        {
            state.UpdateCanJump(false);
            state.SetMode(Mode.Air);
        }

        // Character Input Interface
        public void HorizontalMove(float magnitude)
        {
            Vector3 direction = Vector3.left;
            if (magnitude > 0)
            {
                direction = Vector3.right;
            }

            if (!Physics.Raycast(transform.position, direction, 0.6f))
            {
                state.updateHorizontalSpeed(magnitude);
                //rigidbody.velocity = state.GetVelocity();
                rigidbody.AddRelativeForce(direction * (10 - Mathf.Abs(rigidbody.velocity.x)));
            }
        }

        public void VerticalMove(float magnitude)
        {
            // not implement because player is moved forward constantly

            //state.updateVerticalSpeed(magnitude);
            //rigidbody.velocity = state.GetVelocity();
        }

        public void Jump()
        {
            if (state.CanJump())
            {
                state.Jump();
                state.UpdateCanJump(false);
                state.SetMode(Mode.Air);

                rigidbody.velocity = state.GetVelocity();
            }
        }
    }
}