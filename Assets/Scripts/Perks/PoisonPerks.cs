using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPoisonPerkDefault", menuName = "Perks/PerkPoison")]
public class PoisonPerks : ScriptableObject, IPerk
{
    public float buffPoisonDamage;
    public string name;
    public string disc;
    public Sprite sprite;
    public void ApplyPerk()
    {
        GameManager.Instance.buff[3] += buffPoisonDamage;
    }
    public string SetData()
    {
        return name;
    }
    public string SetDataDis()
    {
        string retDis = disc + " " + buffPoisonDamage + "%";
        return retDis;
    }
    public Sprite GetData()
    {
        return sprite;
    }
}