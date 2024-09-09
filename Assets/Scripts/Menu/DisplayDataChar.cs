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
    public TMP_Text expText;
    public TMP_Text[] claimText;
    public Slider sliderExp;
    public Image[] starsImg;
    public Image curStar;
    public Sprite[] starsSprites;
    public Color lockColor;
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
            if (GameBack.Instance.charData is Character charSet )
            {
                float[] buffs = new float[charSet.GetStat().Length];
                Array.Copy(charSet.GetStat(), buffs, buffs.Length);
                if(charSet.lvlChar<=3)
                {
                     expText.text = $"{charSet.expPoint.ToString("F0")}/{charSet.expPointBarrier[charSet.lvlChar].ToString("F0")}";
                    curStar.sprite = starsSprites[charSet.lvlChar];
                    sliderExp.value = charSet.expPoint/charSet.expPointBarrier[charSet.lvlChar];
                }
                else
                {
                    expText.text = $"Max Level";
                    curStar.sprite = starsSprites[starsSprites.Length-1];
                    sliderExp.value = sliderExp.maxValue ;
                }
                for(int i = 0; i < claimText.Length; i++)
                {
                    if(i >= charSet.lvlChar)
                    {
                        claimText[i].color = lockColor;
                        starsImg[i].color = lockColor;
                    }
                    else
                    {
                        claimText[i].color = new Color(255,255,255,255);
                        starsImg[i].color = new Color(255,255,255,255);
                    }
                }
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
            else if(GameBack.Instance.charData is CharWithOutEvolve charSet1)
            {
                float[] buffs = new float[charSet1.GetStat().Length];
                Array.Copy(charSet1.GetStat(), buffs, buffs.Length);
                if(charSet1.lvlChar<3)
                {
                    expText.text = $"{charSet1.expPoint.ToString("F0")}/{charSet1.expPointBarrier[charSet1.lvlChar].ToString("F0")}";
                    curStar.sprite = starsSprites[charSet1.lvlChar];
                    sliderExp.value = charSet1.expPoint/charSet1.expPointBarrier[charSet1.lvlChar];
                }
                else
                {
                    expText.text = $"Max Level";
                    curStar.sprite = starsSprites[starsSprites.Length-1];
                    sliderExp.value = sliderExp.maxValue ;
                }
                for(int i = 0; i < claimText.Length; i++)
                {
                    if(i >= charSet1.lvlChar)
                    {
                        claimText[i].color = lockColor;
                        starsImg[i].color = lockColor;
                    }
                    else
                    {
                        claimText[i].color = new Color(255,255,255,255);
                        starsImg[i].color = new Color(255,255,255,255);
                    }
                }
                for (int i = 0; i < GameBack.Instance.buffGlobal.Length; i++)
                {
                    buffs[i] += GameBack.Instance.buffGlobal[i];
                }
                string nameChar = charSet1.GetStatName();
                string history = charSet1.GetHistory();
                Sprite iconSprite = charSet1.GetIcon();

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