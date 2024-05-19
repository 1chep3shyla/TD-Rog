using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;
using System;
public class DisplayDataChar : MonoBehaviour
{
    public TextMeshProUGUI buffText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI disText;
    public ScriptableObject charCur;
    public Image icon;
    private ICharSet charSet;
    void Start()
    {
        charSet = charCur as ICharSet;
        // Access the character's stats using GetStat()
        float[] buffs = new float[charSet.GetStat().Length];
        Array.Copy(charSet.GetStat(), buffs, buffs.Length);

        for (int i = 0; i < GameBack.Instance.buffGlobal.Length; i++)
        {
            buffs[i] += GameBack.Instance.buffGlobal[i];
        }
        string nameChar = charSet.GetStatName();

        // Format the buffs with tabulation
        string formattedBuffs = string.Join("\t", buffs);

        charSet.ApplyBuff();
        // Display the formatted buffs in the TextMeshPro component
        buffText.text = formattedBuffs;
        nameText.text = nameChar;
    }

    public void SetDataText()
    {
        // Check if GameBack.Instance and charData are not null
        if (GameBack.Instance != null && GameBack.Instance.charData != null)
        {
            // Attempt to cast charData to ICharSet
            if (GameBack.Instance.charData is ICharSet charSet)
            {
                float[] buffs = new float[charSet.GetStat().Length];
                Array.Copy(charSet.GetStat(), buffs, buffs.Length);

                for (int i = 0; i < GameBack.Instance.buffGlobal.Length; i++)
                {
                    buffs[i] += GameBack.Instance.buffGlobal[i];
                }
                string nameChar = charSet.GetStatName();
                string history = charSet.GetHistory();
                Sprite iconSprite = charSet.GetIcon();

                // Format the buffs with tabulation
                string formattedBuffs = string.Join("\t", buffs);

                buffText.text = formattedBuffs;
                nameText.text = nameChar;
                disText.text = history;
                icon.sprite = iconSprite;
            }
            else
            {
                // Handle the case where charData is not of type ICharSet
                buffText.text = "Character data not available.";
            }
        }
        else
        {
            // Handle the case where GameBack.Instance or charData is null
            buffText.text = "No character data available.";
        }
    }
}