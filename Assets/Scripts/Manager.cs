using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Player.PlayerCharacter player;
    public RoadLooper roadLooper;

    public float maxPlayerSpeed;
    public float speedIncreaseSteps = 0.25f;

    ObserverBroker observerBroker;

    private void Awake()
    {
        DelayCall.Call(this, MakePlayerFaster, 5);
        observerBroker = new ObserverBroker();
        observerBroker.Register(Observer.Event.PlayerFaceHit, 
            PlayerFaceHit, "FaceHit");
    }

    private void OnDestroy() {
        if (observerBroker != null) {
            observerBroker.Unregister("FaceHit");
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

    private void PlayerFaceHit() 
    {
        player.ResetPlayer();
        roadLooper.ResetOrigin();
    }
}
