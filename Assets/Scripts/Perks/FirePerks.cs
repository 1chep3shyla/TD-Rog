using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFirePerkDefault", menuName = "Perks/PerkFire")]
public class FirePerks : ScriptableObject, IPerk
{
    public float buffFireDamage;
    public string name;
    public void ApplyPerk()
    {
        GameManager.Instance.buff[2] += buffFireDamage;
    }
    public string SetData()
    {
        return name;
    }
}
