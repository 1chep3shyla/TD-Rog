using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewDefaultPerkDefault", menuName = "Perks/PerkEvolve")]
public class PerkEvolve : Perks
{
      public int indexCur;
    public GameObject changeGM;
    public GameObject evolveGM;
    public Perks perkNo;
    public string name;
    [TextArea]
    public string disc;
    public Sprite sprite;
    public Color changeColor; 
    public Color changeColorEvolve;

    public override void ApplyPerk()
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
    public Sprite[] GetDataSprites()
    {
        Sprite[] sprites = new Sprite[2];
        sprites[0] = changeGM.GetComponent<SpriteRenderer>().sprite;
        sprites[1] = evolveGM.GetComponent<SpriteRenderer>().sprite;
        return sprites;
    }
        public Color[] GetDataColors()
    {
        Color[] colors = new Color[2];
        colors[0] = changeColor;
        colors[1] = changeColorEvolve;
        return colors;
    }
}