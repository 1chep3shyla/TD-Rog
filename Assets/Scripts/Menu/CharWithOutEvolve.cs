using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChar", menuName = "Character/CharWithOutEvolve")]
public class CharWithOutEvolve : ScriptableObject, ICharSet
{

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
    public void SetData()
    {
        for (int i = 0; i < buffs.Length; i++)
        {
            GameManager.Instance.buff[i] = buffs[i];
        }
        GameObject[] newArrayTowers = towerPull;
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
    public void SetDataChar(int index, float count)
    {
        buffs[index] = count;
    }
}
