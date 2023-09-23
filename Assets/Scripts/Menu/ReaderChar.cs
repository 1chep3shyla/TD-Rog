using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

public class ReaderChar : MonoBehaviour
{
    public string tsvFilePath; // Specify the TSV file path in the Inspector
    public ScriptableObject[] allData;
    public string[] DataCharName;
    public int[] countOfSee;
    private void Start()
    {
#if UNITY_EDITOR
    tsvFilePath = "Assets/StreamingAssets/Character.tsv";
#else
        tsvFilePath = System.IO.Path.Combine(Application.dataPath, "StreamingAssets/Character.tsv");
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

                    if (SearchForWord(lines[i], DataCharName[k]))
                    {

                        string[] values = lines[curLine].Split('\t'); // Changed to '\t' for TSV


                        for (int valueInd = 1; valueInd < values.Length; valueInd++)
                        {
                            if (float.TryParse(values[valueInd], out float parsedValue))
                            {
                                Debug.Log(curIndex);
                                Debug.Log("ƒл€" + DataCharName[k] + "задали значение" + values[0]);
                                ICharSet charCur = allData[k] as ICharSet;
                                charCur.SetDataChar(valueInd-1, parsedValue);
                            }
                            else
                            {
                                Debug.Log("не работает перевод в float" + values[valueInd]);
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
        // »спользуем регул€рное выражение дл€ поиска слова с возможными изменени€ми
        string pattern = @"\b" + Regex.Escape(word) + @"\w*"; // »щем слово и любые дополнительные символы после него
        MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);

        return matches.Count > 0;
    }
}
