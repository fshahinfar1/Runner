using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Player.PlayerCharacter player;
    public AI.AIController aiController;
    public RoadLooper roadLooper;

    public float maxPlayerSpeed;
    public float speedIncreaseSteps = 0.25f;

    private void Awake()
    {
        DelayCall.Call(this, MakePlayerFaster, 5);
    }

    private void FixedUpdate()
    {
        if (aiController.HasLost())
        {
            Debug.Log("Refresh!");
            player.Refresh();
            aiController.Refresh();
        }
    }

    private void MakePlayerFaster()
    {
        if (player.forwardSpeed < maxPlayerSpeed)
        {
            player.forwardSpeed += speedIncreaseSteps;
            DelayCall.Call(this, MakePlayerFaster, 5);
        }
    }
}
