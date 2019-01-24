using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

namespace AI {
    public enum Moves
    {
        Nothing,
        Left,
        Right,
        //Jump,
    }

    public class AIController : MonoBehaviour {

        private int countMoves = 3;

        public float responseDelay = 0.2f; // seconds
        private float timer = 0;

        public GameObject player;
        private Player.CharacterInput inputInterface;

        public StatExtractor consultor;

        public AIKeyboard keyboard;

        private GameStat lastStat;
        private Moves lastMove = Moves.Nothing;
        private float lastMoveValue;
        private float[] lastMoveFeatures;

        public float[] weigths;

        public float learningRate = 0.01f;
        public float discount = 0.80f;
        public float epsilon = 0.20f;

        private bool lost = false;

        private void Awake()
        {
            inputInterface = player.GetComponent<Player.PlayerCharacter>();
            if (inputInterface == null)
            {
                Debug.LogError("Input interface not found");
            }

            weigths = new float[3] { 0, 0, 0 };

            // initialize random seed
            Random.InitState((int) System.DateTime.Now.TimeOfDay.Ticks);

            if (keyboard != null)
            {
                keyboard.SetInputInterface(inputInterface);
                keyboard.SetAction(Moves.Nothing);
            }
            else
            {
                Debug.LogError("Ai keyboard not found!");
            }
        }

        private void Update()
        {
            if (lost)
                return;

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
            //Debug.Log("AI responds!");

            // take your hand of the keyboard
            keyboard.SetAction(Moves.Nothing);

            // get the current state
            GameStat stat = consultor.GetStat();
            stat.lose = inputInterface.IsOutOfControl(); // todo: this should be in GameStat extractor

            // calculate last action reward
            float reward = GetReward(lastStat, stat);

            // choose an action for this situation
            float value;
            float[] features;
            Moves move = ChooseAction(stat, out value, out features);

            // learn from last action
            Feedback(reward, lastMoveValue, lastStat, lastMove, stat, value, lastMoveFeatures);

            // performe the action
            Act(move);

            // remember action so you can evaluate and learn
            lastStat = stat;
            lastMoveValue = value;
            lastMoveFeatures = features;

            // if  Ai lost the game
            if (stat.lose)
                lost = true;

            Debug.Log(string.Format("w: {0}, {1}, {2}", weigths[0], weigths[1], weigths[2]));
        }

        private float QValue(GameStat stat, Moves move, out float[] features)
        {
            GameStat predictedStat = Predict(stat, (Moves)move);
            float value = 0;
            features = FeatureExtractor.Extract(predictedStat);
            value = Vectors.Dot(features, weigths);
            return value;
        }

        private Moves ChooseAction(GameStat stat, out float value, out float[] features)
        {
            float maxVal = -int.MaxValue;
            Moves action = Moves.Nothing;

            float chance = Random.Range(0, 1);
            if (chance < epsilon)
            {
                action = (Moves)Mathf.Floor(Random.Range(0, countMoves + 0.99f));
                value = QValue(stat, action, out features);
                return action;
            }

            features = null;
            for (int move=0; move < countMoves; move++)
            {
                float[] feats;
                float qValue = QValue(stat, (Moves)move, out feats);   
                if (qValue > maxVal)
                {
                    maxVal = qValue;
                    action = (Moves)move;
                    features = feats;
                }
            }
            Debug.Log("Action: " + action.ToString());
            Debug.Log("Value: " + maxVal);

            value = maxVal;
            return action;
        }

        private void Act(Moves move)
        {
            keyboard.SetAction(move);
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
                    if (predict.leftDanger < 0.9)
                    {
                        predict.frontDanger = predict.leftDanger;
                        predict.leftDanger *= 0.5f;
                        predict.points += 1;
                    }
                    break;
                case Moves.Right:
                    if (predict.rightDanger < 0.9)
                    {
                        predict.frontDanger = predict.rightDanger;
                        predict.rightDanger *= 0.5f;
                        predict.points += 1;
                    }
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

        private void Feedback(float reward, float lastStatePredictedValue, GameStat last,
            Moves move, GameStat current, float currentStatePredictedValue, float[] lastFeatures)
        {
            if (lastFeatures == null)
                return;

            float difference = (reward + discount * currentStatePredictedValue) - lastStatePredictedValue;

            for (int i=0; i < weigths.Length; i++)
            {
                weigths[i] += learningRate * difference * lastFeatures[i];
            }
        }

        public bool HasLost()
        {
            return lost;
        }

        public void Refresh()
        {
            lost = false;
        }
    }
}

