using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealPerkDefault", menuName = "Perks/PerkHeal")]
public class PerkHeal :  Perks
{
    public int hpRestore;
    public string name;
    public string disc;
    public Sprite sprite;
    public override void ApplyPerk()
    {
        GameManager.Instance.restoreHeal += hpRestore;
    }
    public string SetData()
    {
        return name;
    }
    public override string SetDataDis()
    {
        string retDis = disc + " " + hpRestore + "";
        return retDis;
    }
    public Sprite GetData()
    {
        return sprite;
    }
    public override int ReturnUpBuff()
    {
        return hpRestore;
    }
}
