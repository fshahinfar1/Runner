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
        Jump,
    }

    public class AIController : MonoBehaviour {

        private int countMoves = 4;

        public float responseDelay = 0.2f; // seconds
        private float timer = 0;

        public GameObject player;
        private Player.CharacterInput inputInterface;

        public StatExtractor consultor;

        public AIKeyboard keyboard;
        public StateMoveValueDisplay display;

        private GameStat lastStat;
        private Moves lastMove = Moves.Nothing;
        private float lastMoveValue;

        public float learningRate = 0.01f;
        public float discount = 0.80f;
        public float epsilon = 0.20f;

        private bool lost = false;

        //[pos, height, dist, move, obsType]
        private float[,,,,] QMat;

        // temp
        float[] qvalues;


        private void Awake()
        {
            inputInterface = player.GetComponent<Player.PlayerCharacter>();
            if (inputInterface == null)
            {
                Debug.LogError("Input interface not found");
            }


            QMat = DataStore.Load("QMat");

            if (QMat == null)
            {
                Debug.LogWarning("Coudln't load QMat!");
                QMat = new float[10, 2, 5, countMoves, 3];
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

            //temps
            qvalues = new float[countMoves];
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
            Moves move = ChooseAction(stat, out value);

            // learn from last action
            Feedback(reward, lastMoveValue, lastStat, lastMove, stat, value);

            // performe the action
            Act(move);

            // remember action so you can evaluate and learn
            lastStat = stat;
            lastMove = move;
            lastMoveValue = value;

            // display
            display.SetReward(reward);

            // if  Ai lost the game
            if (stat.lose)
                Lost();
        }

        private float QValue(GameStat stat, Moves move)
        {
            int pos = stat.pos;
            int height = stat.height;
            int dist = stat.dist[pos];
            int obsType = (int)stat.obstacleType[pos];
            return QMat[pos, height, dist, (int)move, obsType];
        }

        private Moves ChooseAction(GameStat stat, out float value)
        {
            float maxVal = -int.MaxValue;
            Moves action = Moves.Nothing;

            Debug.Log("type: " + stat.obstacleType[stat.pos].ToString());

            float chance = Random.Range(0, 1);
            if (chance < epsilon)
            {
                action = (Moves)Mathf.FloorToInt(Random.Range(0, countMoves));
                value = QValue(stat, action);

                qvalues[(int)action] = value;
                display.Set(qvalues);

                return action;
            }


            for (int move=0; move < countMoves; move++)
            {
                Moves m = (Moves)move;
                //if ((m == Moves.Left && !stat.canLeft) || 
                //    (m == Moves.Right && !stat.canRight))
                //{
                //    continue;
                //}

                float qValue = QValue(stat, m);   
                if (qValue > maxVal)
                {
                    maxVal = qValue;
                    action = m;
                }

                qvalues[move] = qValue;
            }
            Debug.Log("Action: " + action.ToString());
            Debug.Log("Value: " + maxVal);
            display.Set(qvalues);

            value = maxVal;
            return action;
        }

        private void Act(Moves move)
        {
            //keyboard.SetAction(move);
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

        private float GetReward(GameStat last, GameStat current)
        {
            if (current.lose)
            {
                return -5; // lose
            }
            return current.coins;
        }

        private void Feedback(float reward, float lastStatePredictedValue, GameStat last,
            Moves lastMove, GameStat current, float currentStatePredictedValue)
        {
            if (last.dist == null)
                return;

            float difference = (reward + discount * currentStatePredictedValue)
                - lastStatePredictedValue;

            int lastPos = last.pos;
            int lastDist = last.dist[lastPos];
            int lastHeight = last.height;
            int lastObsType = (int)last.obstacleType[lastPos];
            QMat[lastPos, lastHeight, lastDist, (int)lastMove, lastObsType] 
                += learningRate * difference;
        }

        public void Lost()
        {
            lost = true;
            DataStore.Store(QMat, "QMat");

            string path = Application.persistentDataPath + "/death.txt";
            System.IO.TextWriter tw = new System.IO.StreamWriter(path, true);
            tw.Write(string.Format("{0}\n", System.DateTime.Now.ToString()));
            tw.Flush();
            tw.Close();
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

