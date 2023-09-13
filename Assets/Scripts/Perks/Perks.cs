using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewDefaultPerkDefault", menuName = "Perks/PerkDefault")]
public class Perks : ScriptableObject, IPerk
{
    public float buffGlobal;
    public string name;
    public string disc;
    public int indexOfBuff;
    public Sprite sprite;
    public void ApplyPerk()
    {
        GameManager.Instance.buff[indexOfBuff] += buffGlobal;
    }
    public string SetData()
    {
        return name;
    }
    public string SetDataDis()
    {
        string retDis = disc + " " + buffGlobal + "%";
        return retDis;
    }
    public Sprite GetData()
    {
        return sprite;
    }
}
