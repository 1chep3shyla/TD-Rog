using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIcePerkDefault", menuName = "Perks/PerkIce")]
public class IcePerks : ScriptableObject, IPerk
{
    public float buffIcePower;
    public string name;
    public string disc;
    public Sprite sprite;
    public void ApplyPerk()
    {
        GameManager.Instance.buff[1] += buffIcePower;
    }
    public string SetData()
    {
        return name;
    }
    public string SetDataDis()
    {
        string retDis = disc + " " + buffIcePower + "%";
        return retDis;
    }
    public Sprite GetData()
    {
        return sprite;
    }
}