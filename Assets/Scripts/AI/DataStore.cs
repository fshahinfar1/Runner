using System.Collections;
using UnityEngine;
using System;


namespace AI
{
    public class DataStore
    {
        static int l1 = 10; // pos
        static int l2 = 2;  // height
        static int l3 = 5;  // dist
        static int l4 = 4;  // moves

        public static void Store(float [,,,] mat, string name)
        {
            

            string path = Application.persistentDataPath + "/" + name + ".txt";
            System.IO.TextWriter tw = new System.IO.StreamWriter(path);

            /*
             * 1,2,3;\n
             * 4,5,6;
             * .
             * .
             * .
             * 13,14,15;$
             * .
             * .
             * .
             * . . .;$*
             * .
             * .
             * .
             */
            int size = l1 * (l2 * ((l3 * ((l4 * 2) + 1)) + 1) + 1); 
            System.Text.StringBuilder builder = new System.Text.StringBuilder(size);
            for (int i=0; i<l1; i++)
            {
                for (int j = 0; j < l2; j++)
                {
                    for (int k=0; k<l3; k++)
                    {
                        for (int x = 0; x < l4; x++)
                        {
                            if (x == l4 - 1)
                            {
                                builder.AppendFormat("{0};", mat[i, j, k, x]);
                            }
                            else
                            {
                                builder.AppendFormat("{0},", mat[i, j, k, x]);
                            }
                        }
                        if (k == l3 - 1)
                        {
                            builder.Append("$");
                        }
                        else
                        {
                            builder.Append("\n");
                        }
                    }
                    if (j == l2 - 1)
                    {
                        builder.Append("*\n");
                    }
                    else
                    {
                        builder.Append("\n");
                    }
                }
            }


            tw.Write(builder.ToString());
            tw.Flush();
            tw.Close();
        }

        public static float[,,,] Load(string name)
        {
            float[,,,] result = new float[l1, l2, l3, l4];

            string path = Application.persistentDataPath + "/" + name + ".txt";

            if (!System.IO.File.Exists(path))
            {
                return null;
            }

            System.IO.TextReader tr = new System.IO.StreamReader(path);

            string line;
            int pos = 0;
            int height = 0;
            int dist = 0;
            while ((line = tr.ReadLine()) != null)
            {
                string[] tmp = line.Split(',');
                for (int i=0; i<l4-1; i++)
                {
                    result[pos, height, dist, i] = float.Parse(tmp[i]);
                }
                int lastIndex = l4 - 1;
                int lastCharIdx = tmp[lastIndex].Length - 1;
                if (tmp[lastIndex][lastCharIdx] == '*')
                {
                    lastCharIdx-=2;
                    result[pos, height, dist, lastIndex] =
                        float.Parse(tmp[lastIndex].Substring(0, lastCharIdx));
                    dist = 0;
                    height = 0;
                    pos++;
                }
                else if (tmp[lastIndex][lastCharIdx] == '$')
                {
                    lastCharIdx--;
                    result[pos, height, dist, lastIndex] =
                        float.Parse(tmp[lastIndex].Substring(0, lastCharIdx));
                    dist = 0;
                    height++;
                }
                else if (tmp[lastIndex][lastCharIdx] == ';')
                {
                    result[pos, height, dist, lastIndex] =
                        float.Parse(tmp[lastIndex].Substring(0, lastCharIdx));
                    dist++;
                }
                else
                {
                    throw new Exception("Unexpected character");
                }
            }

            return result;
        }
    }
}