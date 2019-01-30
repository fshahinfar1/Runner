using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    [RequireComponent(typeof(PlayerPhysicsConfig))]
    public class Controller : MonoBehaviour
    {

        public CharacterInput player;
        public float horizontalSensitivity = 0.3f;
        public float verticalSensitivity = 0.3f;
        public float jumpSensitivity = 0.1f;

        private void Awake()
        {
            if (horizontalSensitivity < 0)
            {
                Debug.LogWarning("Horizontal sensitivity is negative!!!");
            }

            if (verticalSensitivity < 0)
            {
                Debug.Log("Vertival sensitivity is negative!!!");
            }

            if (jumpSensitivity < 0)
            {
                Debug.LogWarning("Jump sensitivity is negative");
            }
        }

        void Update()
        {
            //float horizontalNormal = Input.GetAxis("Horizontal");
            //if (Mathf.Abs(horizontalNormal) > horizontalSensitivity)
            //{
            //    player.HorizontalMove(horizontalNormal);
            //}

            //float verticalNormal = Input.GetAxis("Vertical");
            //if (Mathf.Abs(verticalNormal) > verticalSensitivity)
            //{
            //    player.VerticalMove(verticalNormal);
            //}
            
            //float jumpNormal = Input.GetAxis("Jump");
            if (Input.GetKeyUp(KeyCode.Space))
            {
                player.Jump();
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                player.HorizontalMove(-1);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                player.HorizontalMove(1);
            }
        }
    }
}