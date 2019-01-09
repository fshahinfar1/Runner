using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class AIController : MonoBehaviour {

    private enum Moves
    {
        Left,
        Right,
        Jump,
    }

    private int countMoves = 3;
    private float responseDelay = 0.3f; // seconds
    private float timer = 0;

    public GameObject player;
    private Player.CharacterInput inputInterface;

    public StatExtractor consultor;


    private void Awake()
    {
        inputInterface = player.GetComponent<Player.PlayerCharacter>();
    }

    private void Update()
    {
        if (timer >= responseDelay)
        {
            timer = 0;
            Respond();
            return;
        }

        timer += Time.deltaTime;
    }

    private void Respond ()
    {
        Debug.Log("AI responds!");
        GameStat stat = consultor.GetStat();
        Moves move = ChooseAction(stat);
        Act(move);
    }

    private Moves ChooseAction(GameStat stat)
    {
        // choose a random move
        return (Moves) Random.Range(0, countMoves);
    }

    private void Act(Moves move)
    {
        switch (move)
        {
            case Moves.Left:
                inputInterface.HorizontalMove(-1);
                break;
            case Moves.Right:
                inputInterface.HorizontalMove(1);
                break;
            case Moves.Jump:
                inputInterface.Jump();
                break;
        }
    }

    private GameStat Predict(GameStat stat, Moves move)
    {
        return stat;
    }
}


