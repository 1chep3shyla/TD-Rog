using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
public class LoseWinScript : MonoBehaviour
{
    public TMP_Text wave;
    public TMP_Text enemyDefeated;
    public TMP_Text nameChar;
    public TMP_Text titleWhich;
    public TMP_Text[] statChar;
    public TMP_Text expText;
    
    public Slider sliderExp;
    public Image iconExp;
    public Image icon;
    public Transform itemParent;
    public ItemOpenner itemOpenner;
    public GameObject completeGM;
    public GameObject prefabItem;
    public GameObject backStat;
    public GameObject pauseGM;
    [Space]
    public AudioClip winSFX;
    public AudioClip loseSFX;
    public Action EndGame;
    public Sprite[] starIcon;
    private float addExp;

    private void Start()
    {
        completeGM.SetActive(false);
        nameChar.text = "";
        enemyDefeated.text = "";
        wave.text = "";
        for(int i = 0; i < statChar.Length;i++)
        {
            statChar[i].text = "";
        }
    }

    public void Result(bool which)
    {
        GameBack.Instance.enemiesKilled += GameManager.Instance.whichEnemyKill;
        GameBack.Instance.souls += (int)((float)GameManager.Instance.whichEnemyKill * UnityEngine.Random.Range(0.8f, 1.2f)*((GameManager.Instance.secondsBuff[11]+100)/100));
        GameBack.Instance.waveInStage[GameBack.Instance.indexState] = GameManager.Instance.curWave;
        if (which)
        {
            Win();
            GameBack.Instance.winGame++;
            GameBack.Instance.gamePlayed++;
            string filePath = Application.persistentDataPath + "/levelData.dat";
            if (File.Exists(filePath))
            {
                File.Delete(Application.persistentDataPath + "/levelData.dat");
            }
        }
        else
        {
            Lose();
            string filePath = Application.persistentDataPath + "/levelData.dat";
            if (File.Exists(filePath))
            {
                File.Delete(Application.persistentDataPath + "/levelData.dat");
            }

            if (GameManager.Instance.spawn.currentWaveIndexMain == 0)
            {
                GameBack.Instance.loseFirstWave = true;
            }
            GameBack.Instance.gamePlayed++;
        }
        if (GameManager.Instance.healthBreak >= GameBack.Instance.healthBreak)
        {
            GameBack.Instance.healthBreak = GameManager.Instance.healthBreak;
        }
    }

    public void Win()
    {
        addExp+=250 + (float)GameManager.Instance.Health * 5 + (float)GameManager.Instance.whichEnemyKill * 0.5f + (float)GameManager.Instance.itemOpenner.items.Length * 2;
        if (GameManager.Instance.Health < GameBack.Instance.healthWin)
        {
            GameBack.Instance.healthWin = GameManager.Instance.Health;
        }
        GameManager.Instance.aS.PlayOneShot(winSFX);
        bool canWork = true;
        bool canWorkPerfecto = false;
        for (int i = 0; i < GameManager.Instance.buff.Length; i++)
        {
            if (GameManager.Instance.buff[i] >= 0)
            {
                canWork = false;
            }
            if (GameManager.Instance.buff[i] != 100 && !canWorkPerfecto)
            {
                canWorkPerfecto = false;
            }
            else
            {
                canWorkPerfecto = true;
            }
        }
        if (canWork)
        {
            GameBack.Instance.hardWinning = 1;
        }
        if (canWorkPerfecto)
        {
            GameBack.Instance.perfecto = 1;
        }

        completeGM.SetActive(true);
        pauseGM.SetActive(false);
        backStat.SetActive(false);
        titleWhich.text = "Stage Completed!";

        StartCoroutine(ShowWinLoseDetails());
    }

    public void Lose()
    {
        addExp+=(float)GameManager.Instance.whichEnemyKill * 0.5f + (float)GameManager.Instance.itemOpenner.items.Length * 2;
        completeGM.SetActive(true);
        GameManager.Instance.aS.PlayOneShot(loseSFX);
        pauseGM.SetActive(false);
        backStat.SetActive(false);
        titleWhich.text = "Stage Failed!";

        StartCoroutine(ShowWinLoseDetails());
    }

    private IEnumerator ShowWinLoseDetails()
    {
        SaveManager.instance.SaveGameData();
        icon.sprite = GameBack.Instance.charData.GetIcon();
        GameManager.Instance.StartTypingText(new TMP_Text[] { nameChar }, new string[] { GameBack.Instance.charData.GetStatName() }, 0.4f);
        yield return new WaitForSecondsRealtime(0.4f);
        float[] expCount = GameBack.Instance.charData.GetExp();
        if((int)expCount[2] <3)
        {
            iconExp.sprite = starIcon[(int)expCount[2]];
            yield return new WaitForSecondsRealtime(0.7f);
            for(int i = 0; i < 10; i++)
            {
                sliderExp.value = (expCount[0]/(10-i))/expCount[1];
                yield return new WaitForSecondsRealtime(0.05f);
            }
            GameManager.Instance.StartTypingText(new TMP_Text[] { expText }, new string[] {$"{expCount[0].ToString("F0")}/{expCount[1].ToString("F0")}"}, 0.4f);
            yield return new WaitForSecondsRealtime(0.5f);
            float addNeed = addExp / 35;
            for(int i = 0; i < 35; i++)
            {
                if(GameBack.Instance.charData is CharWithOutEvolve charSet && charSet.lvlChar <3 )
                {
                    if(charSet.expPoint < charSet.expPointBarrier[charSet.lvlChar])
                    {
                        charSet.expPoint += addNeed;
                        expText.text = $"{charSet.expPoint.ToString("F0")}/{charSet.expPointBarrier[charSet.lvlChar].ToString("F0")}";
                        sliderExp.value = charSet.expPoint/charSet.expPointBarrier[charSet.lvlChar];
                        iconExp.sprite = starIcon[charSet.lvlChar];
                    }
                    else
                    {
                        charSet.expPoint = 0;
                        charSet.lvlChar++;
                        charSet.expPoint += addNeed;
                        expText.text = $"{charSet.expPoint.ToString("F0")}/{charSet.expPointBarrier[charSet.lvlChar].ToString("F0")}";
                        sliderExp.value = charSet.expPoint/charSet.expPointBarrier[charSet.lvlChar];
                        iconExp.sprite = starIcon[charSet.lvlChar];
                    }
                }
                else if(GameBack.Instance.charData is Character charSet1  && charSet1.lvlChar <3 )
                {
                    if(charSet1.expPoint < charSet1.expPointBarrier[charSet1.lvlChar])
                    {
                    charSet1.expPoint += addNeed;
                    expText.text = $"{charSet1.expPoint.ToString("F0")}/{charSet1.expPointBarrier[charSet1.lvlChar].ToString("F0")}";
                    sliderExp.value = charSet1.expPoint/charSet1.expPointBarrier[charSet1.lvlChar];
                    iconExp.sprite = starIcon[charSet1.lvlChar];
                    }
                    else
                    {
                        charSet1.expPoint = 0;
                        charSet1.lvlChar++;
                        charSet1.expPoint += addNeed;
                        expText.text = $"{charSet1.expPoint.ToString("F0")}/{charSet1.expPointBarrier[charSet1.lvlChar].ToString("F0")}";
                        sliderExp.value = charSet1.expPoint/charSet1.expPointBarrier[charSet1.lvlChar];
                        iconExp.sprite = starIcon[charSet1.lvlChar];
                    }
                }
                if(addNeed > 0 )
                {
                    yield return new WaitForSecondsRealtime(0.05f);
                }
            }
        }
        else
        {
            GameManager.Instance.StartTypingText(new TMP_Text[] { expText }, new string[] {$"Max"}, 0.3f);
            sliderExp.value = sliderExp.maxValue;
            iconExp.sprite = starIcon[3];
            yield return new WaitForSecondsRealtime(0.7f);
            
        }
        
        GameManager.Instance.StartTypingText(new TMP_Text[] { wave }, new string[] { "Wave Complete: " + GameManager.Instance.curWave.ToString("") }, 0.4f);
        yield return new WaitForSecondsRealtime(0.7f);

        GameManager.Instance.StartTypingText(new TMP_Text[] { enemyDefeated }, new string[] { "Enemy Defeated: " + GameManager.Instance.whichEnemyKill.ToString("") }, 0.4f);
        yield return new WaitForSecondsRealtime(0.7f);


        float[] baseStat = GameBack.Instance.charData.GetStat();
        string[] statTexts = new string[statChar.Length];
        for (int i = 0; i < statChar.Length; i++)
        {
            statTexts[i] = baseStat[i] + "%" + "->" + GameManager.Instance.buff[i] + "%";
        }
        GameManager.Instance.StartTypingText(statChar, statTexts, 0.4f);
        yield return new WaitForSecondsRealtime(0.7f);

        StartCoroutine(ShowItems());
        
    }

    private IEnumerator ShowItems()
    {
        for (int i = 0; i < itemOpenner.items.Length; i++)
        {
            GameObject newItem = Instantiate(prefabItem, transform.position, Quaternion.identity);
            newItem.transform.parent = itemParent;
            newItem.transform.localScale = new Vector3(1, 1, 1);
            newItem.transform.localPosition += new Vector3(0, 0, 110);

            Image imageComponent = newItem.transform.GetChild(0).GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = itemOpenner.items[i].icon;
            }
            newItem.transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text = itemOpenner.items[i].count + "x";
            ItemSee itemSee = newItem.GetComponent<ItemSee>();
            if (itemSee != null)
            {
                itemSee.itemIs = itemOpenner.items[i];
            }
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }
}