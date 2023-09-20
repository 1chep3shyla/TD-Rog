using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChar 
{
    public void ApplyBuff(); // Задать значение для бека
}
public interface ICharSet : IChar
{
    public void SetData(); // Задать значение для игры
    public float[] GetStat();
    public string GetStatName();
}
[CreateAssetMenu(fileName = "NewChar", menuName = "Character/Char")]
public class Character : ScriptableObject, ICharSet
{
    public int indexEvolve; // 0 - split, 1 - elf, 2 - blizzard, 3 - Oil, 4 - Boom, 5 - divine, 6 - storm, 7 - poisonSmoke, 8 - rage, 9 - portal, 10 - gear, 11 - assasin, 12 - cannon, 13 - magnet, 14 - gladiator
    public float[] buffs;
    public string name;
    public void ApplyBuff()
    {
        GameBack.Instance.charData = this;
    }
    public void SetData()
    {
        for (int i = 0; i < buffs.Length; i++)
        {
            GameManager.Instance.buff[i] = buffs[i];
        }
        GameManager.Instance.allEvolution[indexEvolve].work = true;
    }
    public float[] GetStat()
    {
        return buffs;
    }
    public string GetStatName()
    {
        return name;
    }
}

