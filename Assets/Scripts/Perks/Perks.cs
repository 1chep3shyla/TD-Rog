using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewDefaultPerkDefault", menuName = "Perks/PerkDefault")]
public class Perks : ScriptableObject, IPerk
{
    public float buffGlobalDamage;
    public string name;
    public void ApplyPerk()
    {
        GameManager.Instance.buff[0] += buffGlobalDamage;
    }
    public string SetData()
    {
        return name;
    }
}
