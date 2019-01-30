using System.Collections;
using UnityEngine;
using System;
using System.IO;


namespace AI
{
    public class DataStore
    {
        public static void Store(float [,,,,] mat, string name)
        {
            string path = Application.persistentDataPath + "/" + name + ".bin";
            Stream stream = File.Open(path, FileMode.Create);

            var bformatter = new System.Runtime.Serialization
                .Formatters.Binary.BinaryFormatter();

            bformatter.Serialize(stream, mat);
            stream.Flush();
            stream.Close();
        }

        public static float[,,,,] Load(string name)
        {
            string path = Application.persistentDataPath + "/" + name + ".bin";

            if (!File.Exists(path))
                return null;

            Stream stream = File.Open(path, FileMode.Open);

            var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            float[,,,,] result = (float[,,,,])bformatter.Deserialize(stream);

            stream.Close();

            return result;
        }
    }
}