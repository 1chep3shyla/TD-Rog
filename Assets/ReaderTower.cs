using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.IO;

public class ReaderTower : MonoBehaviour
{
    public string tsvFilePath; // Specify the TSV file path in the Inspector
    public DataTower[] allData;

    private void Start()
    {
        LoadTowerData();
    }

    private void LoadTowerData()
    {
        if (!string.IsNullOrEmpty(tsvFilePath) && File.Exists(tsvFilePath))
        {
            string[] lines = File.ReadAllLines(tsvFilePath);
            int lineIndex = 0;
            for (int k = 0; k < allData.Length; k++)
            {
                int curLineIndex = lineIndex + 5;
                for (int i = 1; i < lines.Length && lineIndex < curLineIndex; i++) // Skip the header line
                {
                    lineIndex++;
                    string[] values = lines[lineIndex].Split('\t'); // Changed to '\t' for TSV
                    for (int o = 1; o < values.Length; o++)
                    {
                        allData[k].lvlData[i - 1, o] = float.Parse(values[o]);
                        if (allData[k].lvlData[i - 1, o - 1] != 0)
                        {
                            Debug.Log(allData[k].lvlData[i - 1, o - 1]);
                        }
                    }

                }

            }
        }
        else
        {
            Debug.LogError("Enemy data TSV file is not found or path is invalid!");
        }
    }
}