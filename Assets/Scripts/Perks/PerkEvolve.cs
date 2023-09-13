using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewDefaultPerkDefault", menuName = "Perks/PerkEvolve")]
public class PerkEvolve : ScriptableObject, IPerk
{
    public int indexCur;
    public string name;
    public string disc;
    public Sprite sprite;
    public void ApplyPerk()
    {
        GameManager.Instance.allEvolution[indexCur].WorkThis();
    }
    public string SetData()
    {
        return name;
    }
    public string SetDataDis()
    {
        string retDis = disc;
        return retDis;
    }
    public Sprite GetData()
    {
        return sprite;
    }
}
