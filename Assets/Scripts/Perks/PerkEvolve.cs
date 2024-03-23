using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewDefaultPerkDefault", menuName = "Perks/PerkEvolve")]
public class PerkEvolve : Perks
{
      public int indexCur;
    public GameObject changeGM;
    public GameObject evolveGM;
    public string name;
    [TextArea]
    public string disc;
    public Sprite sprite;

    public void ApplyPerk()
    {
        GameManager gameManager = GameManager.Instance;
        GameObject[] towers = gameManager.gameObject.GetComponent<Rolling>().towers;

        for (int i = 0; i < towers.Length; i++)
        {
            if (towers[i] == changeGM)
            {
                towers[i] = evolveGM;
                gameManager.gameObject.GetComponent<Rolling>().imageidTower[i] = evolveGM.GetComponent<SpriteRenderer>().sprite;
                gameManager.allEvolution[indexCur].WorkThis();
                break;
            }
        }
    }

    public string SetData()
    {
        return name;
    }

    public string SetDataDis()
    {
        return disc;
    }

    public Sprite GetData()
    {
        return sprite;
    }
}