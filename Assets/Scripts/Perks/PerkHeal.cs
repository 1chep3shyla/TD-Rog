using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealPerkDefault", menuName = "Perks/PerkHeal")]
public class PerkHeal : ScriptableObject, IPerk
{
    public int hpRestore;
    public string name;
    public string disc;
    public Sprite sprite;
    public void ApplyPerk()
    {
        GameManager.Instance.restoreHeal += hpRestore;
    }
    public string SetData()
    {
        return name;
    }
    public string SetDataDis()
    {
        string retDis = disc + " " + hpRestore + "";
        return retDis;
    }
    public Sprite GetData()
    {
        return sprite;
    }
}
