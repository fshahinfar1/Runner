using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class PlayerCharacter : MonoBehaviour, CharacterInput
    {
        public PlayerState state;
        private new Rigidbody rigidbody;
        private new Collider collider;
        private Material material;

        public float forwardSpeed = 1.0f;
        public float horizontalSpeed = 1.0f;

        private bool outOfControl = false;
        private bool ignoreCollision = false;

        //private LayerMask layerMask;

        private void Awake()
        {
            state = new PlayerState();
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
            material =  GetComponent<Renderer>().material;

            if (collider == null)
            {
                Debug.LogError("Player collider is null!");
            }

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

            //layerMask = LayerMask.GetMask("Obstacle");

            // register to face collision detection
            FaceCollisionDetector fcd = transform.Find("FaceCollisionDetector")
                .GetComponent<FaceCollisionDetector>();
            fcd.Register(FaceHit);
        }

        private void FixedUpdate()
        {
            //state.SetVelocity(rigidbody.velocity);
            //state.updateVerticalSpeed(1);
            //rigidbody.velocity = state.GetVelocity();
            if (!outOfControl)
            {
                //if (!ignoreCollision &&
                //    Physics.Raycast(transform.position, Vector3.forward, 0.6f, layerMast))
                //{
                //    Debug.LogError("Out of control!");
                //    outOfControl = true;
                //    return;
                //}

                float f = Mathf.Max(forwardSpeed - rigidbody.velocity.z, 0);
                rigidbody.AddRelativeForce(Vector3.forward * f, ForceMode.VelocityChange);
            }
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

        // Face collision detection
        private void FaceHit()
        {
            if (ignoreCollision)
                return;

            Debug.LogError("Out of control!");
            outOfControl = true;
        }

        // Character Input Interface
        public void HorizontalMove(float magnitude)
        {
            if (outOfControl)
                return;

            Vector3 direction = Vector3.left;
            if (magnitude > 0)
            {
                direction = Vector3.right;
            }

            if (!Physics.Raycast(transform.position, direction, 0.6f))
            {
                //state.updateHorizontalSpeed(magnitude);
                ////rigidbody.velocity = state.GetVelocity();
                //float fMagnitude = horizontalSpeed - Mathf.Abs(rigidbody.velocity.x) * Time.deltaTime;
                //rigidbody.AddRelativeForce(direction * fMagnitude, ForceMode.VelocityChange);

                Vector3 pos = transform.position + direction;
                rigidbody.MovePosition(pos);
            }
        }

        public void VerticalMove(float magnitude)
        {
            if (outOfControl)
                return;

            // not implement because player is moved forward constantly

            //state.updateVerticalSpeed(magnitude);
            //rigidbody.velocity = state.GetVelocity();
        }

        public void Jump()
        {
            if (outOfControl)
                return;

            if (state.CanJump())
            {
                state.Jump();
                state.UpdateCanJump(false);
                state.SetMode(Mode.Air);

                rigidbody.velocity = state.GetVelocity();
            }
        }

        public bool IsOutOfControl()
        {
            return outOfControl;
        }

        // Player Chararcter
        public void Refresh()
        {
            // deactivate collider
            int playerLayer = LayerMask.NameToLayer("Player");
            int obstacleLayer = LayerMask.NameToLayer("Obstacle");
            ignoreCollision = true;
            Physics.IgnoreLayerCollision(playerLayer, obstacleLayer);
            Color c = material.GetColor("_Color");
            c.a = 0.1f;
            c.b = 0.5f;
            material.SetColor("_Color", c);
            c.a = 1;
            c.b = 0;
            
            // after one second set collider 
            DelayCall.Call(this, () => {
                Debug.Log("Set collision active");
                ignoreCollision = false;
                Physics.IgnoreLayerCollision(playerLayer, obstacleLayer, false);
                material.SetColor("_Color", c);
            }, 2.0f);

            outOfControl = false;
        }
    }
}