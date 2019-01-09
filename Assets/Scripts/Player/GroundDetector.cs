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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Ground")
            {
                hitGround = true;
                if (hit != null)
                {
                    hit();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Ground")
            {
                hitGround = false;
                if (leave != null)
                {
                    leave();
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