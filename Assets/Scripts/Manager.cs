using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Player.PlayerCharacter player;
    public AI.AIController aiController;
    public RoadLooper roadLooper;

    private void FixedUpdate()
    {
        if (aiController.HasLost())
        {
            Debug.Log("Refresh!");
            player.Refresh();
            aiController.Refresh();
        }
    }
}
