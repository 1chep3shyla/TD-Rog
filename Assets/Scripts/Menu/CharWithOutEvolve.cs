using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "NewChar", menuName = "Character/CharWithOutEvolve")]
public class CharWithOutEvolve : ScriptableObject, ICharSet
{
    public int indexChar;
    public float[] buffs;
    public string name;
    public Sprite icon;
    public GameObject[] towerPull;
    [TextArea(15,20)]
    public string history;
    public bool has;
    public void ApplyBuff()
    {
        GameBack.Instance.charData = this;
        GameBack.Instance.iconChar = icon;
    }
    public GameObject[] SetGameObject()
    {
        return towerPull;
    }
    public int GetIndex()
    {
        return indexChar;
    }
    public void SetData()
    {
        for (int i = 0; i < buffs.Length; i++)
        {
            GameManager.Instance.buff[i] = buffs[i] + GameBack.Instance.buffGlobal[i];
        }
        GameObject[] newArrayTowers = new GameObject[towerPull.Length];
        Array.Copy(towerPull, newArrayTowers, towerPull.Length);
        GameManager.Instance.gameObject.GetComponent<Rolling>().towers = newArrayTowers;
        GameManager.Instance.gameObject.GetComponent<Rolling>().SetSprite();
        GameManager.Instance.gameObject.GetComponent<Rolling>().Roll();
    }
    public float[] GetStat()
    {
        return buffs;
    }
    public string GetStatName()
    {
        return name;
    }
    public string GetHistory()
    {
        return history;
    }
    public Sprite GetIcon()
    {
        return icon;
    }
    public int GetIndexEvolve()
    {
        return -10;
    }
    public void SetDataChar(int index, float count)
    {
        buffs[index] = count;
    }
}
