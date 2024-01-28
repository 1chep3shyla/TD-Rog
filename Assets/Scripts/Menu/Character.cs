using System.Collections;
using System.Collections.Generic;
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
    public void SetDataChar(int index, float count);
}
[CreateAssetMenu(fileName = "NewChar", menuName = "Character/Char")]
public class Character : ScriptableObject, ICharSet
{
    public int indexEvolve; // 0 - split, 1 - elf, 2 - blizzard, 3 - Oil, 4 - Boom, 5 - divine, 6 - storm, 7 - poisonSmoke, 8 - rage, 9 - portal, 10 - gear, 11 - assasin, 12 - cannon, 13 - magnet, 14 - gladiator
    public float[] buffs;
    public string name;
    public Sprite icon;
    [TextArea(15,20)]
    public string history;
    public GameObject[] towerPull;
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
        if (indexEvolve != -10)
        {
            GameManager.Instance.allEvolution[indexEvolve].work = true;
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
       public string GetHistory()
    {
        return history;
    }
    public void SetDataChar(int index, float count)
    {
        buffs[index] = count;
    }
}

