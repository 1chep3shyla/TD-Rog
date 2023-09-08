using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIcePerkDefault", menuName = "Perks/PerkIce")]
public class IcePerks : ScriptableObject, IPerk
{
    public float buffIcePower;
    public string name;
    public void ApplyPerk()
    {
        GameManager.Instance.buff[1] += buffIcePower;
    }
    public string SetData()
    {
        return name;
    }
}