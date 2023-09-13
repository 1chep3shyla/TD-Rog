using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class PerkRoll : MonoBehaviour
{
    public int[] chancePerk;
    public List<ScriptableObject> allBronzePerks; // Use a List instead of an array for flexibility
    public List<ScriptableObject> allSilverPerks; // Use a List instead of an array for flexibility
    public List<ScriptableObject> allGoldenPerks; // Use a List instead of an array for flexibility
    public List<ScriptableObject> allEvolutionPerks; // Use a List instead of an array for flexibility
    public ScriptableObject[] curPerks;
    public Text[] perkText;
    public Image[] cardBack;
    public Image[] cardIcon;
    public GameObject PerkGM;
    public Sprite[] spritesCard;
    public Text[] discription;
    public bool rollingEvolve;

    void Start()
    {
        RollPerk();
    }
    public void ChoosePerk(int index)
    {
        if (curPerks[index] is IPerk perk)
        {
            // Call the ApplyPerk method on the concrete type
            perk.ApplyPerk();
        }
        rollingEvolve = false;
        PerkGM.SetActive(false);
    }
    public void RollPerk()
    {
        List<ScriptableObject> availableBronzePerks = allBronzePerks;
        List<ScriptableObject> availableSilverPerks = allSilverPerks;
        List<ScriptableObject> availableGoldenPerks = allGoldenPerks;
        for (int i = 0; i < curPerks.Length; i++)
        {
            int random = Random.Range(0, 100);
            for (int o = 0; o < chancePerk.Length; o++) // Fixed the loop condition here
            {
                if (random < chancePerk[o])
                {
                    if (o == 0)
                    {
                        cardBack[i].sprite = spritesCard[0];
                        int randomPerk = Random.Range(0, availableBronzePerks.Count);
                        curPerks[i] = availableBronzePerks[randomPerk];
                        availableBronzePerks.Remove(availableBronzePerks[randomPerk]);
                    }
                    else if (o == 1)
                    {
                        cardBack[i].sprite = spritesCard[1];
                        int randomPerk = Random.Range(0, availableSilverPerks.Count);
                        curPerks[i] = availableSilverPerks[randomPerk];
                        availableSilverPerks.Remove(availableSilverPerks[randomPerk]);
                    }
                    else if (o == 2)
                    {
                        cardBack[i].sprite = spritesCard[2];
                        int randomPerk = Random.Range(0, availableGoldenPerks.Count);
                        curPerks[i] = availableGoldenPerks[randomPerk];
                        availableGoldenPerks.Remove(availableGoldenPerks[randomPerk]);
                    }
                    o = 1000;
                }
            }

            if (curPerks[i] is IPerk perk)
            {
                // Call the ApplyPerk method on the concrete type
                perkText[i].text = perk.SetData();
                cardIcon[i].sprite = perk.GetData();
                discription[i].text = perk.SetDataDis();
            }
        }
        PerkGM.SetActive(true);
    }
    public void RollPerkEvolve()
    {
        rollingEvolve = true;
        List<ScriptableObject> availablePerks = allEvolutionPerks;

        for (int i = 0; i < curPerks.Length; i++)
        {
            cardBack[i].sprite = spritesCard[2];
            int randomPerk = Random.Range(0, availablePerks.Count);
            curPerks[i] = availablePerks[randomPerk];
            availablePerks.Remove(availablePerks[randomPerk]);

            if (curPerks[i] is IPerk perk)
            {
                // Call the ApplyPerk method on the concrete type
                perkText[i].text = perk.SetData();
                cardIcon[i].sprite = perk.GetData();
                discription[i].text = perk.SetDataDis();
            }
            PerkGM.SetActive(true);
        }

    }

}

