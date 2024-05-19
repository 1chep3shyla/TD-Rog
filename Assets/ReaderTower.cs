using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

public class ReaderTower : MonoBehaviour
{
    public string tsvFilePath; // Specify the TSV file path in the Inspector
    public DataTower[] allData;
    public string[] DataTowerName;
    public int[] countOfSee;
    private void Start()
    {
#if UNITY_EDITOR
    tsvFilePath = "Assets/StreamingAssets/TowerData.tsv";
#else
        tsvFilePath = System.IO.Path.Combine(Application.dataPath, "StreamingAssets/TowerData.tsv");
#endif
        LoadTowerData();

    }

    private void LoadTowerData()
    {
        if (!string.IsNullOrEmpty(tsvFilePath) && File.Exists(tsvFilePath))
        {
            string[] lines = File.ReadAllLines(tsvFilePath);
            for (int k = 0; k < allData.Length; k++)
            {
                int curLine = 1;
                int curIndex = 0;
                for (int i = 1; i < lines.Length; i++) // Skip the header line
                {

                    if (SearchForWord(lines[i], DataTowerName[k]))
                    {

                        string[] values = lines[curLine].Split('\t'); // Changed to '\t' for TSV


                        for (int valueInd = 1; valueInd < values.Length; valueInd++)
                        {
                            string normalizedValue = values[valueInd].Replace('.', ',');
                            if (float.TryParse(values[valueInd], out float parsedValue))
                            {
                                Debug.Log(curIndex);
                                Debug.Log("���" + DataTowerName[k] + "������ ��������" + values[0]);
                                allData[k].lvlData[curIndex, valueInd] = parsedValue;
                            }
                            else
                            {
                                Debug.Log("�� �������� ������� � float" + values[valueInd]);
                            }
                        }
                        curIndex++;

                    }
                    curLine++;
                }
            }
        }
        else
        {
            Debug.LogError("Enemy data TSV file is not found or path is invalid!");
        }
    }
    bool SearchForWord(string input, string word)
    {
        // ���������� ���������� ��������� ��� ������ ����� � ���������� �����������
        string pattern = @"\b" + Regex.Escape(word) + @"\w*"; // ���� ����� � ����� �������������� ������� ����� ����
        MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);

        return matches.Count > 0;
    }
}