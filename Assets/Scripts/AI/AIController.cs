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

        // q learning
        private GameStat lastStat;
        private Moves lastMove = Moves.Nothing;
        private float lastMoveValue;
        private float[] lastMoveFeatures;
        public float[] weights;

        // prediction
        private float[] lastPredictedFeatures;
        private float[] prePredictionFeatures;
        private float[][] predictionWeights;

        //q laerning parameters
        public float learningRate = 0.01f;
        public float discount = 0.80f;
        public float epsilon = 0.20f;
        private int countPrameters = 12;

        private bool lost = false;

        private void Awake()
        {
            inputInterface = player.GetComponent<Player.PlayerCharacter>();
            if (inputInterface == null)
            {
                Debug.LogError("Input interface not found");
            }

            weights = DataStore.Load("Weights");
            if (weights == null || weights.Length != countPrameters)
                weights = new float[countPrameters];

            predictionWeights = new float[countMoves][];
            for (int i = 0; i < countMoves; i++)
            {
                predictionWeights[i] = DataStore.Load(string.Format("PredictionWeights_{0}", i));
                if (predictionWeights[i] == null || predictionWeights[i].Length != countPrameters)
                    predictionWeights[i] = new float[countPrameters];
            }

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
            float[] features; // f(s, a)
            Moves move = ChooseAction(stat, out value, out features);

            // learn from last action
            Feedback(reward, lastMoveValue, lastStat, lastMove, stat, value, lastMoveFeatures);

            // learn how to predict
            LearnPrediction(FeatureExtractor.Extract(stat), lastMove);

            // performe the action
            Act(move);

            // remember action so you can evaluate and learn
            lastStat = stat;
            lastMoveValue = value;
            lastMove = move;
            lastMoveFeatures = features;

            // if  Ai lost the game
            if (stat.lose)
                Lost();
        }

        private float QValue(GameStat stat, Moves move, out float[] features)
        {
            features = Predict(stat, (Moves)move);
            float value = 0;
            //features = FeatureExtractor.Extract(predictedStat);
            value = Vectors.Dot(features, weights);
            return value;
        }

        private Moves ChooseAction(GameStat stat, out float value, out float[] features)
        {
            float maxVal = -int.MaxValue;
            Moves action = Moves.Nothing;

            // epsilon greedy
            float chance = Random.Range(0, 1);
            if (chance < epsilon)
            {
                action = (Moves)Mathf.Floor(Random.Range(0, countMoves + 0.99f));
                value = QValue(stat, action, out features);
                return action;
            }

            // max action
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

        private float[] Predict(GameStat stat, Moves move)
        {
            float[] f = FeatureExtractor.Extract(stat);
            float[] f2 = Vectors.ElementWiseProduct(f, predictionWeights[(int)move]);
            //f = Vectors.Sum(f, predictionBias);

            prePredictionFeatures = f;
            lastPredictedFeatures = f2;

            return f2;
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

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] += learningRate * difference * lastFeatures[i];
            }
        }

        private void LearnPrediction(float[] currentFeatures, Moves lastMove)
        {
            if (lastPredictedFeatures == null || prePredictionFeatures == null)
                return;

            for (int i = 0; i < lastPredictedFeatures.Length; i++)
            {
                float diff = currentFeatures[i] - lastPredictedFeatures[i];
                predictionWeights[(int)lastMove][i] += learningRate * diff;
            }
        }

        public void Lost()
        {
            lost = true;
            DataStore.Store(weights, "Weights");
            DataStore.Store(predictionWeights[0], "PredictionWeights_0");
            DataStore.Store(predictionWeights[1], "PredictionWeights_1");
            DataStore.Store(predictionWeights[2], "PredictionWeights_2");
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

