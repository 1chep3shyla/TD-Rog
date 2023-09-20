using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

public class ReaderState : MonoBehaviour
{
    public string tsvFilePath; // Specify the TSV file path in the Inspector
    public string[] formules;
    void Start()
    {
#if UNITY_EDITOR
    tsvFilePath = "Assets/StreamingAssets/StateSetting.tsv";
#else
        tsvFilePath = System.IO.Path.Combine(Application.dataPath, "StreamingAssets/StateSetting.tsv");
#endif
        LoadTowerData();
    }

    private void LoadTowerData()
    {
        if (!string.IsNullOrEmpty(tsvFilePath) && File.Exists(tsvFilePath))
        {
            string[] lines = File.ReadAllLines(tsvFilePath);
            formules = new string[lines.Length-1];
            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split('\t'); // Changed to '\t' for TSV
                formules[i-1] = values[1];
            }
        }
        else
        {
            Debug.LogError("Enemy data TSV file is not found or path is invalid!");
        }
    }
    public void SetData(int i)
    {
        GameBack.Instance.curFormula = formules[i];
        GameBack.Instance.indexState = i;
        Debug.Log(formules[i]);
    }
}
