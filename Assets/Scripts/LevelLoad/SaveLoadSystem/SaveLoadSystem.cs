using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MergeAndFight.Merge;
using UnityEngine;

namespace SaveLoadSystem
{
    public class SaveLoadSystem<T> where T : struct
    {
        private string _fileName;

        public SaveLoadSystem(string fileName)
        {
            _fileName = "/" + fileName + ".dat";
        }

        public T GetLoadedData()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            T data;

            if (File.Exists(Application.persistentDataPath + _fileName))
            {
                using FileStream file = File.Open(Application.persistentDataPath + _fileName, FileMode.Open);
                data = (T)binaryFormatter.Deserialize(file);
                file.Close();
                Debug.Log("Game data loaded!");
            }
            else
            {
                data = new T();
                Debug.Log("Default game data loaded!");
            }

            return data;
        }

        public void SaveData(T data)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using FileStream fileStream = File.Create(Application.persistentDataPath + _fileName);

            binaryFormatter.Serialize(fileStream, data);
            fileStream.Close();
            Debug.Log("Save completed!");
        }
    }
}
