using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class GroundDetector : MonoBehaviour
    {

        private bool hitGround;

        private System.Action hit;
        private System.Action leave;
        

        private void Awake()
        {
            hitGround = false;
        }

        private void Hit()
        {
            Debug.Log("Hit Ground!");
            hitGround = true;
            if (hit != null)
            {
                hit();
            }
        }

        private void Leave()
        {
            Debug.Log("Leave Ground!");
            hitGround = false;
            if (leave != null)
            {
                leave();
            }
        }

        private void FixedUpdate()
        {
            if (Physics.Raycast(transform.position, Vector3.down, 0.6f))
            {
                if (!hitGround)
                {
                    Hit();
                }
            }
            else
            {
                if (hitGround)
                {
                    Leave();
                }
            }
        }

        public bool GroundState()
        {
            return hitGround;
        }

        public void RegisterOnGroundHit(System.Action hitAction)
        {
            hit += hitAction;
        }

        public void UnregisterOnGroundHit(System.Action hitAction)
        {
            hit -= hitAction;
        }

        public void RegisterOnGroundLeave(System.Action leaveAction)
        {
            leave += leaveAction;
        }

        public void UnregisterOnGroundLeave(System.Action leaveAction)
        {
            leave -= leaveAction;
        }
    }
}