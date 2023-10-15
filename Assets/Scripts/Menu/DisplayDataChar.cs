using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

public class DisplayDataChar : MonoBehaviour
{
    public TextMeshProUGUI buffText;
    public TextMeshProUGUI nameText;
    public ScriptableObject charCur;
    private ICharSet charSet;
    void Start()
    {
        charSet = charCur as ICharSet;
        // Access the character's stats using GetStat()
        float[] buffs = charSet.GetStat();
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
                // Access the character's stats using GetStat()
                float[] buffs = charSet.GetStat();
                string nameChar = charSet.GetStatName();

                // Format the buffs with tabulation
                string formattedBuffs = string.Join("\t", buffs);

                // Display the formatted buffs in the TextMeshPro component
                buffText.text = formattedBuffs;
                nameText.text = nameChar;
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