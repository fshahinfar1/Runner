using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Stat
{
    public class StatDisplay : MonoBehaviour
    {
        private GameStat stat;
        private bool ready = false;

        public void Display (GameStat gameStat)
        {
            ready = true;
            stat = gameStat;
        }

        private void OnGUI()
        {
            if (stat.dist == null || !ready)
            {
                return;
            }

            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 5 / 100);
            rect.yMin = h * 10 / 100;
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 5 / 100;
            style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Debug.LogWarning(stat.pos);
            try
            {
                string text = string.Format("Pos: {0}, Dist: {1} ",
                    stat.pos, stat.dist[stat.pos]);
                GUI.Label(rect, text, style);

            }
            catch
            {
                Debug.Break();
            }

            Rect rect2 = new Rect(0, 0, w, h * 5 / 100);
            rect2.yMin = h * 20 / 100;
            string text2 = string.Format("ObsType: {0}", stat.obstacleType[stat.pos]);
            GUI.Label(rect2, text2, style);
        }
    }
}