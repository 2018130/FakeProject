using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsvReader
{
    // CSV 파일이 Resources 폴더 안에 있을 경우 파일 이름만 지정합니다.
    public static string csvFileName = "dialogue_data"; // 파일명: dialogue_data.csv

    /// <summary>
    /// Resources 폴더에서 CSV 파일을 읽어 DialogueData 리스트에 저장합니다.
    /// </summary>
    public static List<DialogueData> LoadCsvData()
    {
        List<DialogueData> dialogueDatas = new List<DialogueData>();

        // Resources 폴더에서 TextAsset으로 CSV 파일 로드
        TextAsset csvFile = Resources.Load<TextAsset>(csvFileName);

        if (csvFile == null)
        {
            Debug.LogError($"CSV 파일 '{csvFileName}.csv'을(를) Resources 폴더에서 찾을 수 없습니다.");
            return null;
        }

        // 전체 텍스트를 줄 단위로 분할
        string[] lines = csvFile.text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length <= 1)
        {
            Debug.LogWarning("CSV 파일에 헤더 또는 데이터가 없습니다.");
            return null;
        }

        // 첫 번째 줄은 헤더(열 이름)입니다.
        string[] headers = lines[0].Split(',');

        // 두 번째 줄부터 데이터입니다.
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            if (values.Length != headers.Length)
            {
                Debug.LogWarning($"줄 {i + 1}의 데이터 형식이 올바르지 않아 건너뜁니다.");
                continue;
            }

            DialogueData data = new DialogueData();

            // 헤더와 데이터를 매핑하여 할당
            for (int j = 0; j < headers.Length; j++)
            {
                string header = headers[j].Trim();
                string value = values[j].Trim();

                switch (header)
                {
                    case "ID":
                        int.TryParse(value, out data.ID);
                        break;
                    case "Name":
                        data.Name = value;
                        break;
                    case "Dialogue":
                        data.Dialogue = value;
                        break;
                    case "HasChoice":
                        // 문자열을 bool로 변환
                        // "TRUE", "True", "true", "1" 등을 모두 true로 처리할 수 있습니다.
                        data.HasChoice = bool.TryParse(value, out bool hasChoice) && hasChoice;
                        break;
                    case "Favorability":
                        // 문자열을 int로 변환
                        if (int.TryParse(value, out int favorability))
                        {
                            data.Favorability = favorability;
                        }
                        else
                        {
                            Debug.LogWarning($"줄 {i + 1}의 Favorability 값 '{value}'를 정수로 변환할 수 없습니다. 0으로 설정합니다.");
                            data.Favorability = 0;
                        }
                        break;
                    default:
                        // 정의되지 않은 헤더는 무시
                        break;
                }
            }
            dialogueDatas.Add(data);
        }

        return dialogueDatas;
    }
}
