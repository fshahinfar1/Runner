﻿using System.Collections;
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
        }

        private void Update()
        {
            // update state velocity
            state.SetVelocity(rigidbody.velocity);
        }

        private void FixedUpdate()
        {
            state.updateVerticalSpeed(1);
            rigidbody.velocity = state.GetVelocity();
        }

        // Ground detection
        private void HitGround()
        {
            state.UpdateCanJump(false);
        }

        private void LeaveGround()
        {
            state.UpdateCanJump(true);
        }

        // Character Input Interface
        public void HorizontalMove(float magnitude)
        {
            state.updateHorizontalSpeed(magnitude);
            rigidbody.velocity = state.GetVelocity();
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

                rigidbody.velocity = state.GetVelocity();
            }
        }
    }
}