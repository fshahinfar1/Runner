﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

namespace AI {
    public class AIController : MonoBehaviour {

        private enum Moves
        {
            Nothing,
            Left,
            Right,
            //Jump,
        }
        private int countMoves = 3;

        private float responseDelay = 0.3f; // seconds
        private float timer = 0;

        public GameObject player;
        private Player.CharacterInput inputInterface;

        public StatExtractor consultor;

        private GameStat lastStat;
        private Moves lastMove = Moves.Nothing;
        private float lastMoveValue;

        private float[] weigths;

        private float learningRate = 0.01f;
        private float discount = 0.80f;
        private float epsilon = 0.20f;

        private void Awake()
        {
            inputInterface = player.GetComponent<Player.PlayerCharacter>();
            weigths = new float[3] { 0, 0, 0 };

            // initialize random seed
            Random.InitState((int) System.DateTime.Now.TimeOfDay.Ticks);
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

        private void Respond()
        {
            Debug.Log("AI responds!");

            // get the current state
            GameStat stat = consultor.GetStat();

            // calculate last action reward
            float reward = GetReward(lastStat, stat);

            // choose an action for this situation
            float value;
            Moves move = ChooseAction(stat, out value);

            // learn from last action
            Feedback(reward, lastMoveValue, lastStat, lastMove, stat, value);

            // performe the action
            Act(move);

            // remember action so you can evaluate and learn
            lastStat = stat;
            lastMoveValue = value;
        }

        private float QValue(GameStat stat, Moves move)
        {
            GameStat predictedStat = Predict(stat, (Moves)move);
            float value = 0;
            float[] features = FeatureExtractor.Extract(predictedStat);
            value = Vectors.Dot(features, weigths);
            return value;
        }

        private Moves ChooseAction(GameStat stat, out float value)
        {
            float maxVal = -int.MaxValue;
            Moves action = Moves.Nothing;
            for (int move=0; move < countMoves; move++)
            {
                float qValue = QValue(stat, (Moves)move);   
                if (qValue > maxVal)
                {
                    maxVal = qValue;
                    action = (Moves)move;
                }
            }

            value = maxVal;
            return action;
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
                //case Moves.Jump:
                //    inputInterface.Jump();
                //    break;
            }
        }

        private GameStat Predict(GameStat stat, Moves move)
        {
            // very weak prediction
            GameStat predict = new GameStat(stat);
            float zSpeed = predict.zSpeed;

            switch (move)
            {
                case Moves.Nothing:
                    predict.leftDanger += zSpeed * responseDelay;
                    predict.rightDanger += zSpeed * responseDelay;
                    predict.frontDanger += zSpeed * responseDelay;
                    if (predict.frontDanger > 0.5)
                        predict.lose = true;
                    predict.points += 1;
                    break;
                case Moves.Left:
                    predict.frontDanger = predict.leftDanger;
                    predict.points += 1;
                    break;
                case Moves.Right:
                    predict.frontDanger = predict.rightDanger;
                    predict.points += 1;
                    break;
            }
            return predict;
        }

        private float GetReward(GameStat last, GameStat current)
        {
            if (current.lose)
            {
                return -5; // lose
            }
            return 0;
        }

        private void Feedback(float reward, float lastStatePredictedValue, GameStat last, Moves move, GameStat current, float currentStatePredictedValue)
        {
            float difference = (reward + currentStatePredictedValue) - lastStatePredictedValue;

            for (int i=0; i < weigths.Length; i++)
            {
                weigths[i] += learningRate * discount * difference;
            }
        }
    }
}

