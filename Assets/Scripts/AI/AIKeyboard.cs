using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public class AIKeyboard : MonoBehaviour
    {
        private Player.CharacterInput inputInterface;
        private Moves move;

        private void FixedUpdate()
        {
            switch (move)
            {
                case Moves.Left:
                    inputInterface.HorizontalMove(-1);
                    break;
                case Moves.Right:
                    inputInterface.HorizontalMove(1);
                    break;
                    //case Moves.Jump:
                    //    inputInterface.Jump();
                    //    break;
            }
        }

        public void SetInputInterface(Player.CharacterInput input)
        {
            inputInterface = input;
        }

        public void SetAction(Moves m)
        {
            move = m;
        }
    }
}