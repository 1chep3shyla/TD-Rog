using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.IO;

public class ReaderFile : MonoBehaviour
{

    public string tsvFilePath; // Specify the TSV file path in the Inspector
    public Enemy[] enemyStat;
    public EnemyMoving[] enemyMoveStat;

    private void Start()
    {
        LoadEnemyData();
    }

    private void LoadEnemyData()
    {
        if (!string.IsNullOrEmpty(tsvFilePath) && File.Exists(tsvFilePath))
        {
            string[] lines = File.ReadAllLines(tsvFilePath);
            Debug.Log(lines[2]);
            for (int i = 1; i < lines.Length; i++) // Skip the header line
            {
                string[] values = lines[i].Split('\t'); // Changed to '\t' for TSV

                enemyStat[i - 1].maxHealth = int.Parse(values[1]);
                enemyMoveStat[i - 1].typeEnemy = (EnemyType)Enum.Parse(typeof(EnemyType), values[3]);
                float speedValue;
                if (float.TryParse(values[2], NumberStyles.Float, CultureInfo.InvariantCulture, out speedValue))
                {
                    enemyMoveStat[i - 1].maxSpeed = speedValue;
                }

            }
        }
        else
        {
            Debug.LogError("Enemy data TSV file is not found or path is invalid!");
        }
    }
}