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

    private GameStat lastStat;
    private Moves lastMove;


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

        // get the current state
        GameStat stat = consultor.GetStat();

        // calculate last action reward
        float reward = GetReward(lastStat, stat);
        
        // learn from last action
        Feedback(reward, lastStat, lastMove, stat);
        
        // choose an action for this situation
        Moves move = ChooseAction(stat);
        
        // performe the action
        Act(move);
        
        // remember action so you can evaluate and learn
        lastStat = stat;
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

    private float GetReward(GameStat last, GameStat current)
    {
        return 0;
    }

    private void Feedback(float reward, GameStat last, Moves move, GameStat current)
    {

    }
}


