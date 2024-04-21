using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoseWinScript : MonoBehaviour
{
    public TMP_Text wave;
    public TMP_Text enemyDefeated;
    public TMP_Text nameChar;
    public TMP_Text titleWhich;
    public TMP_Text[] statChar;

    public Image icon;
    public Transform itemParent;
    public ItemOpenner itemOpenner;
    public GameObject completeGM;
    public GameObject prefabItem;
    public GameObject backStat;
    public GameObject pauseGM;

    public void Result(bool which)
    {
        GameBack.Instance.enemiesKilled += GameManager.Instance.whichEnemyKill;
        if(which)
        {
            Win();
            GameBack.Instance.winGame++;
            GameBack.Instance.gamePlayed++;
        }
        else
        {
            Lose();
            if(GameManager.Instance.spawn.currentWaveIndexMain == 0)
            {
                GameBack.Instance.loseFirstWave = true;
            }
            GameBack.Instance.gamePlayed++;
        }
    }

    public void Win()
    {
        completeGM.SetActive(true);
        titleWhich.text = "Stage Completed!";
        pauseGM.SetActive(false);
        backStat.SetActive(false);
        wave.text = "Wave Complete: " +GameManager.Instance.curWave.ToString("");
        enemyDefeated.text = "Enemy Defeated: " +  GameManager.Instance.whichEnemyKill.ToString("");
        nameChar.text = GameBack.Instance.charData.GetStatName();
        icon.sprite = GameBack.Instance.charData.GetIcon();
        float[] baseStat = GameBack.Instance.charData.GetStat();
        for(int i = 0; i < statChar.Length; i++)
        {
            statChar[i].text = baseStat[i] + "%" + "->" + GameManager.Instance.buff[i]+ "%";
        }
        for(int i = 0; i <itemOpenner.items.Length; i++)
        {
            GameObject newItem = Instantiate(prefabItem, transform.position, Quaternion.identity);
            newItem.transform.parent = itemParent; 
            newItem.transform.localScale = new Vector3(1, 1, 1);
            newItem.transform.localPosition += new Vector3(0,0,110);

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
        }
    }
    public void Lose()
    {
        completeGM.SetActive(true);
        titleWhich.text = "Stage Failed!";
        pauseGM.SetActive(false);
        backStat.SetActive(false);
        wave.text = "Wave Complete: " +GameManager.Instance.curWave.ToString("");
        enemyDefeated.text = "Enemy Defeated: " +  GameManager.Instance.whichEnemyKill.ToString("");
        nameChar.text = GameBack.Instance.charData.GetStatName();
        icon.sprite = GameBack.Instance.charData.GetIcon();
        float[] baseStat = GameBack.Instance.charData.GetStat();
        for(int i = 0; i < statChar.Length; i++)
        {
            statChar[i].text = baseStat[i] + "%" + "->" + GameManager.Instance.buff[i]+ "%";
        }
        for(int i = 0; i <itemOpenner.items.Length; i++)
        {
            GameObject newItem = Instantiate(prefabItem, transform.position, Quaternion.identity);
            newItem.transform.parent = itemParent; 
            newItem.transform.localScale = new Vector3(1, 1, 1);
            newItem.transform.localPosition += new Vector3(0,0,110);

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
        }
    }

}
