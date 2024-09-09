using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IChar 
{
    public void ApplyBuff(); // ������ �������� ��� ����
}
public interface ICharSet : IChar
{
    public void SetData(); // ������ �������� ��� ����
    public float[] GetStat();
    public string GetStatName();
    public GameObject[] SetGameObject();
    public string GetHistory();
    public int GetIndex();
    public int GetIndexEvolve();
    public Sprite GetIcon();
    public void SetDataChar(int index, float count);
    public float[] GetExp();
}
[CreateAssetMenu(fileName = "NewChar", menuName = "Character/Char")]
public class Character : ScriptableObject, ICharSet
{
    public bool isHave;
    public int indexChar;
    public int indexEvolve; // 0 - split, 1 - elf, 2 - blizzard, 3 - Oil, 4 - Boom, 5 - divine, 6 - storm, 7 - poisonSmoke, 8 - rage, 9 - portal, 10 - gear, 11 - assasin, 12 - cannon, 13 - magnet, 14 - gladiator
    public float[] buffs;
    public int lvlChar;
    public float expPoint;
    public float[] expPointBarrier; 
    public string name;
    public Sprite icon;
    [TextArea(15,20)]
    public string history;
    public bool has;
    public Perks perkEvolveNo;
    public GameObject[] towerPull;
    public void ApplyBuff()
    {
        GameBack.Instance.charData = this;
        GameBack.Instance.iconChar = icon;
    }
      public float[] GetExp()
    {
        return new float[] { expPoint, expPointBarrier[lvlChar],(float)lvlChar };
    }
    public GameObject[] SetGameObject()
    {
        return towerPull;
    }
    public void SetData()
    {
        for (int i = 0; i < buffs.Length; i++)
        {
            GameManager.Instance.buff[i] = buffs[i] + GameBack.Instance.buffGlobal[i];
        }
        if (indexEvolve != -10)
        {
            GameManager.Instance.allEvolution[indexEvolve].work = true;
            if(perkEvolveNo != null)
            {
                GameManager.Instance.gameObject.GetComponent<PerkRoll>().allEvolutionPerks.Remove(perkEvolveNo);
            }
        }
        GameObject[] newArrayTowers = new GameObject[towerPull.Length];
        Array.Copy(towerPull, newArrayTowers, towerPull.Length);
        GameManager.Instance.gameObject.GetComponent<Rolling>().towers = newArrayTowers;
        GameManager.Instance.gameObject.GetComponent<Rolling>().SetSprite();
        GameManager.Instance.gameObject.GetComponent<Rolling>().Roll();
        if(!GameBack.Instance.saveThis)
        {
            if(lvlChar == 1)
            {
                GameManager.Instance.Gold+=150;
                GameManager.Instance.ChangeMoney();
            }
            if(lvlChar == 2)
            {
                GameManager.Instance.maxHealth+=5;
                GameManager.Instance.Health = GameManager.Instance.maxHealth;
            }
            if(lvlChar == 3)
            {
                GameManager.Instance.secondsBuff[10]+=15;
            }
            
        }
    }
    public float[] GetStat()
    {
        return buffs;
    }
    public int GetIndex()
    {
        return indexChar;
    }
    public string GetStatName()
    {
        return name;
    }
       public string GetHistory()
    {
        return history;
    }
    public int GetIndexEvolve()
    {
        return indexEvolve;
    }
    public Sprite GetIcon()
    {
        return icon;
    }
    public void SetDataChar(int index, float count)
    {
        buffs[index] = count;
    }
}

