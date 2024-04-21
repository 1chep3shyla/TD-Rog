using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.IO;

public class ReaderFile : MonoBehaviour
{

    public string tsvFilePath; // Specify the TSV file path in the Inspector
    public int[] healthCount;
    public float[] speedCount;
    public float[] multiplaerHealth;
    public float[] multiplaerSpeed;
    public Enemy[] enemyStat;
    public EnemyMoving[] enemyMoveStat;

    private void Start()
    {
#if UNITY_EDITOR
    tsvFilePath = "Assets/StreamingAssets/EnemyStatAll.tsv";
#else
        tsvFilePath = System.IO.Path.Combine(Application.dataPath, "StreamingAssets/EnemyStatAll.tsv");
#endif
        LoadEnemyData();
    }

    private void LoadEnemyData()
    {
        if (!string.IsNullOrEmpty(tsvFilePath) && File.Exists(tsvFilePath))
        {
            string[] lines = File.ReadAllLines(tsvFilePath);
            Debug.Log(lines[2]);
            for (int i = 1; i < 31; i++) // Skip the header line
            {

                string[] values = lines[i].Split('\t'); // Changed to '\t' for TSV
                healthCount[i - 1] = int.Parse(values[2]);
                float speedValue;
                if (float.TryParse(values[3], NumberStyles.Float, CultureInfo.InvariantCulture, out speedValue))
                {
                    speedCount[i - 1] = speedValue;
                }
            }
            for(int a = 1; a < multiplaerHealth.Length+1;a++)
            {
                string[] values = lines[a].Split('\t'); // Changed to '\t' for TSV
                float multy;
                if (float.TryParse(values[10], NumberStyles.Float, CultureInfo.InvariantCulture, out multy))
                {
                    multiplaerHealth[a - 1] = multy;
                }
                float multySpeed;
                if (float.TryParse(values[11], NumberStyles.Float, CultureInfo.InvariantCulture, out multySpeed))
                {
                    multiplaerSpeed[a - 1] = multySpeed;
                }
            }
        }
        else
        {
            Debug.LogError("Enemy data TSV file is not found or path is invalid!");
        }
    }
}