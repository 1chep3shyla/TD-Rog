using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealPermanentPerkDefault", menuName = "Perks/PerkHealPermanent")]
public class HealthPerk : Perks
{
    public int hpRestore;
    public string name;
    public string disc;
    public Sprite sprite;
    public override void ApplyPerk()
    {
        if (GameManager.Instance.Health + hpRestore > GameManager.Instance.maxHealth)
        {
            GameManager.Instance.Health = GameManager.Instance.maxHealth;
        }
        else
        {
            GameManager.Instance.Health += hpRestore;
            Debug.Log("���0");

        }
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
