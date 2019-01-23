using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Stat
{
    public class StatDisplay : MonoBehaviour
    {
        private GameStat stat;

        public void Display (GameStat gameStat)
        {
            stat = gameStat;
        }

        private void OnGUI()
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 5 / 100);
            rect.yMin = h * 10 / 100;
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 5 / 100;
            style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            string text = string.Format("{0:0.0}, {1:0.0}, {2:0.0} ",
                stat.leftDanger, stat.frontDanger, stat.rightDanger);
            GUI.Label(rect, text, style);
        }
    }
}