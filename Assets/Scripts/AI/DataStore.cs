using System.Collections;
using UnityEngine;
using System;


namespace AI
{
    public class DataStore
    {

        public static void Store(float[] mat, string name)
        {
            string path = Application.persistentDataPath + "/" + name + ".txt";
            System.IO.TextWriter tw = new System.IO.StreamWriter(path);

            int size = mat.Length * 2 - 1;
            System.Text.StringBuilder builder = new System.Text.StringBuilder(size);
            for (int i = 0; i < mat.Length; i++)
            {
                if (i == mat.Length - 1)
                {
                    builder.Append(mat[i].ToString());
                }
                else
                {
                    builder.AppendFormat("{0},", mat[i]);
                }
            }

            tw.Write(String.Format("{0}\n", mat.Length));
            tw.Write(builder.ToString());
            tw.Flush();
            tw.Close();
        }

        public static float[] Load(string name)
        {

            string path = Application.persistentDataPath + "/" + name + ".txt";

            if (!System.IO.File.Exists(path))
            {
                return null;
            }

            System.IO.TextReader tr = new System.IO.StreamReader(path);

            string line = tr.ReadLine();
            int size = int.Parse(line);
            float[] result = new float[size];

            string[] tmp = line.Split(',');
            for (int i = 0; i < tmp.Length; i++)
            {
                result[i] = float.Parse(tmp[i]);
            }

            return result;
        }
    }
}