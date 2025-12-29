using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersistentDataManager : MonoBehaviour
    {
        public static PersistentDataManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            LoadDataFromFile();
        }

        private List<StringPair> persistantData = new List<StringPair>();

        private void OnApplicationQuit()
        {
            //SaveDataToFile();
        }

    public void SaveData(string key, object data, bool overwrite = true)
    {
        StringPair currentData = GetData(key);
        bool isNewData = false; // 새로운 데이터인지 체크하는 플래그

        if (!overwrite && currentData != null)
        {
            Debug.LogWarning($"이미 저장되어 있는 데이터를 다시 저장합니다.{key} (덮어쓰기 불가)");
            return;
        }

        if (currentData == null)
        {
            currentData = CreateData(key);
            isNewData = true; // 리스트에 없는 데이터일 때만 true
        }

        string serializedData = "";
        // ... (데이터 타입 판별 로직은 그대로 유지) ...
        if (data.GetType() == typeof(int)) { serializedData = "i_" + data.ToString(); }
        else if (data.GetType() == typeof(float)) { serializedData = "f_" + data.ToString(); }
        else if (data.GetType() == typeof(bool)) { serializedData = "b_" + data.ToString(); }
        else if (data.GetType() == typeof(Vector3)) { /* ... */ }
        else if (data.GetType() == typeof(string)) { serializedData = "s_" + data.ToString(); }
        else { return; }

        currentData.Second = serializedData;

        // 핵심 수정: 새 데이터일 때만 리스트에 추가합니다.
        // 기존 데이터라면 위에서 currentData.Second 값만 바뀌었으므로 리스트를 건드릴 필요가 없습니다.
        if (isNewData)
        {
            persistantData.Add(currentData);
        }

        Debug.Log($"{key} 데이터가 버퍼에 저장되었습니다.");
    }

    public void RemoveData(string key)
        {
            StringPair removeableData = GetData(key);
            if (removeableData != null)
            {
                persistantData.Remove(removeableData);
            }
        }

        public T GetDataWithParsing<T>(string key, T defaultValue)
        {
            StringPair stringPair = GetData(key);
            if (stringPair == null)
            {
                Debug.LogWarning($"해당 키에 대한 데이터가 존재하지 않습니다. {key}");
                return defaultValue;
            }

            string rawData = stringPair.Second;
            string[] splitData = rawData.Split('_');

            if (splitData.Length != 2)
            {
                Debug.LogWarning($"데이터 파싱에 실패했습니다. {key} : {rawData}");
                return defaultValue;
            }
            string dataType = splitData[0];
            string dataValue = splitData[1];

            if (dataType[0] == 'i')
            {
                int parsedValue = int.Parse(dataValue);
                return (T)(object)parsedValue;
            }
            else if (dataType[0] == 'f')
            {
                float parsedValue = float.Parse(dataValue);
                return (T)(object)parsedValue;
            }
        else if (dataType[0] == 'b')
        {
            bool parsedValue = bool.Parse(dataValue);
            return (T)(object)parsedValue;
        }
        else if (dataType[0] == 'v')
            {
                string[] vectorComponents = dataValue.Split('.');
                if (vectorComponents.Length != 3)
                {
                    Debug.LogWarning($"Vector3 데이터 파싱에 실패했습니다. {key} : {rawData}");
                    return defaultValue;
                }
                float x = float.Parse(vectorComponents[0]);
                float y = float.Parse(vectorComponents[1]);
                float z = float.Parse(vectorComponents[2]);
                Vector3 parsedValue = new Vector3(x, y, z);
                return (T)(object)parsedValue;
            }
            else if (dataType[0] == 's')
            {
                return (T)(object)dataValue;
            }
            else
            {
                Debug.LogWarning($"지원하지 않는 데이터 타입입니다. {key} : {rawData}");
                return defaultValue;
            }
        }

        private void LoadDataFromFile()
        {
            if (File.Exists(Application.persistentDataPath + "/persistantData.txt"))
            {
                string[] loadedLines = File.ReadAllLines(Application.persistentDataPath + "/persistantData.txt");
                for (int i = 0; i < loadedLines.Length; i++)
                {
                    string line = loadedLines[i];
                    if (line.StartsWith("{") && line.EndsWith("}"))
                    {
                        string content = line.Substring(1, line.Length - 2);
                        string[] keyValue = content.Split(',');
                        if (keyValue.Length == 2)
                        {
                        StringPair dataPair = new StringPair() { First = keyValue[0], Second = keyValue[1] };
                        
                        ///kjh1229
                        //StringDouble dataPair = new StringDouble() { First = keyValue[0], Second = keyValue[1] };

                        persistantData.Add(dataPair);
                        }
                    }
                }
            }
        }
        public void SaveDataToFile()
        {
            string saveData = "";
            for(int i = 0; i < persistantData.Count; i++)
            {
                saveData += "{" + persistantData[i].First + "," + persistantData[i].Second + "}\n";
        }
        Debug.Log($"데이터가 파일에 저장되었습니다");
        File.WriteAllText(Application.persistentDataPath + "/persistantData.txt", saveData);
        }

        private StringPair CreateData(string key, string data = "")
        {
            return new StringPair() { First = key };
        }

        private StringPair GetData(string key)
        {
            for(int i = 0; i < persistantData.Count; i++)
            {
                if (persistantData[i].First == key)
                    return persistantData[i];
            }

            return null;
        }
    }
