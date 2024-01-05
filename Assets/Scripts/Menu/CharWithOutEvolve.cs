using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChar", menuName = "Character/CharWithOutEvolve")]
public class CharWithOutEvolve : ScriptableObject, ICharSet
{

    public float[] buffs;
    public string name;
    public GameObject[] towerPull;
    public void ApplyBuff()
    {
        GameBack.Instance.charData = this;
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
        GameManager.Instance.gameObject.GetComponent<Rolling>().towers = towerPull;
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
    public void SetDataChar(int index, float count)
    {
        buffs[index] = count;
    }
}
