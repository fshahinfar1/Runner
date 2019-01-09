using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player {
    public interface CharacterInput {
        void HorizontalMove(float magnitude);
        void VerticalMove(float magnitude);
        void Jump();
    }
}