using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerState
    {
        //private static PlayerState instance = new PlayerState();
        //public static PlayerState GetInstance()
        //{
        //    if (instance == null)
        //    {
        //        instance = new PlayerState();
        //    }
        //    return instance;
        //}

        //public static void DestroyInstance()
        //{
        //    instance = null;
        //}

        private Vector3 velocity;

        private float maxPartialSpeed;
        private float jumpForce;

        private bool canJump;

        private Mode mode;

        public PlayerState()
        {
            velocity = new Vector3();

            maxPartialSpeed = 3;
            jumpForce = 7;
            canJump = true;
            mode = Mode.Ground;
        }

        public void updateHorizontalSpeed (float horizontalNormal)
        {
            velocity.x = maxPartialSpeed * horizontalNormal;
        }

        public void updateVerticalSpeed(float verticalNormal)
        {
            //velocity.z = maxPartialSpeed * verticalNormal;
            velocity.z = maxPartialSpeed;
        }

        public void Jump()
        {
            velocity.y = jumpForce;
        }

        public void SetVelocity(Vector3 velocity)
        {
            this.velocity = velocity;
        }

        public Vector3 GetVelocity()
        {
            return velocity;
        }

        public bool CanJump()
        {
            return canJump;
        }

        public void UpdateCanJump(bool state)
        {
            canJump = state;
        }

        public Mode GetMode()
        {
            return mode;
        }

        public void SetMode(Mode mode)
        {
            this.mode = mode;
        }
    }

    public enum Mode
    {
        Ground,
        Air,
    }
}