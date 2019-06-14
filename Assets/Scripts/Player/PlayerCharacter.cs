using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class PlayerCharacter : MonoBehaviour, CharacterInput
    {
        public PlayerState state;
        private new Rigidbody rigidbody;
        private FaceCollisionDetector fcd;
        private Material material;

        public float forwardSpeed = 1.0f;
        private float startForwardSpeed;
        public float horizontalSpeed = 1.0f;
        public float bodyWidth;

        private bool outOfControl = false;
        private bool ignoreCollision = false;

        private LayerMask layerMask;

        private Vector3 startGlobalPosition;

        private void Awake()
        {
            startForwardSpeed = forwardSpeed;
            state = new PlayerState();
            rigidbody = GetComponent<Rigidbody>();
            material =  GetComponent<Renderer>().material;

            startGlobalPosition = transform.position;  // position player starts the game from

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

            layerMask = LayerMask.GetMask(new string[]{"Obstacle", "Default"});

            // register to face collision detection
            fcd = transform.Find("FaceCollisionDetector")
                .GetComponent<FaceCollisionDetector>();
            fcd.Register(FaceHit);
        }

        private void FixedUpdate()
        {
            if (!outOfControl)
            {
                // constant add force to maintain constant speed !
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

            var observer = Observer.GetInstance();
            observer.Trigger(Observer.Event.PlayerFaceHit);
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

            if (!Physics.Raycast(transform.position, direction, bodyWidth))
            {
                // if not blocked by obstacle

                // float fMagnitude = horizontalSpeed - Mathf.Abs(rigidbody.velocity.x) * Time.deltaTime;
                // rigidbody.AddRelativeForce(direction * fMagnitude, ForceMode.VelocityChange);

                Vector3 pos = transform.position + direction;
                rigidbody.MovePosition(pos);
            }
        }

        public void VerticalMove(float magnitude)
        {
            if (outOfControl)
                return;

            // not implement because player is moved forward constantly
        }

        public void Jump()
        {
            if (outOfControl)
                return;

            if (Physics.Raycast(transform.position, Vector3.down, 0.6f, layerMask))
            {
                state.Jump();
                state.UpdateCanJump(false);
                state.SetMode(Mode.Air);

                //rigidbody.velocity = state.GetVelocity();
                rigidbody.AddRelativeForce(Vector3.up * 5, ForceMode.Impulse);
            }
        }

        public bool IsOutOfControl()
        {
            return outOfControl;
        }

        public bool IsIgnoringCollision()
        {
            return ignoreCollision;
        }

        public void ResetPlayer() {
            outOfControl = false;
            ignoreCollision = false;
            transform.position = startGlobalPosition;
            rigidbody.velocity = new Vector3();
            forwardSpeed = startForwardSpeed;
            // TODO: remove all forces applied to rigidbody
        }

        // Player Chararcter
        // public void Refresh()
        // {
        //     // deactivate collider
        //     int playerLayer = LayerMask.NameToLayer("Player");
        //     int obstacleLayer = LayerMask.NameToLayer("Obstacle");
        //     ignoreCollision = true;
        //     Physics.IgnoreLayerCollision(playerLayer, obstacleLayer);
        //     Color c = material.GetColor("_Color");
        //     c.a = 0.1f;
        //     c.b = 0.5f;
        //     material.SetColor("_Color", c);


        //     float delayTime = 2 / forwardSpeed;
        //     // after one second set collider 
        //     DelayCall.Call(this, ActiveCollision, delayTime);

        //     outOfControl = false;
        // }

        // private void ActiveCollision()
        // {
        //     if (fcd.HasCollision())
        //     {
        //         Debug.Log("Wait for cube to exit danger zone");
        //         // if there is no more collision
        //         DelayCall.Call(this, ActiveCollision, 1);
        //         return;
        //     }
        //     int playerLayer = LayerMask.NameToLayer("Player");
        //     int obstacleLayer = LayerMask.NameToLayer("Obstacle");
        //     Physics.IgnoreLayerCollision(playerLayer, obstacleLayer, false);
        //     ignoreCollision = false;

        //     Color c = material.GetColor("_Color");
        //     c.a = 1;
        //     c.b = 0;
        //     material.SetColor("_Color", c);
        // }
    }
}