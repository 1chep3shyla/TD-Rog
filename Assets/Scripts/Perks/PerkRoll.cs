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
    public ScriptableObject[] curPerks;
    public Text[] perkText;
    public GameObject PerkGM;

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
        PerkGM.SetActive(false);
    }
    public void RollPerk()
    {
        for (int i = 0; i < curPerks.Length; i++)
        {
            int random = Random.Range(0, 100);
            for (int o = 0; o < chancePerk.Length; o++) // Fixed the loop condition here
            {
                if (random < chancePerk[o])
                {
                    if (o == 0)
                    {
                        int randomPerk = Random.Range(0, allBronzePerks.Count); 
                        curPerks[i] = allBronzePerks[randomPerk];
                    }
                    else if (o == 1)
                    {
                        int randomPerk = Random.Range(0, allSilverPerks.Count); 
                        curPerks[i] = allSilverPerks[randomPerk];
                    }
                    else if (o == 2)
                    {
                        int randomPerk = Random.Range(0, allGoldenPerks.Count); 
                        curPerks[i] = allGoldenPerks[randomPerk];
                    }
                    o = 1000;   
                }
            }

            if (curPerks[i] is IPerk perk)
            {
                // Call the ApplyPerk method on the concrete type
                perkText[i].text = perk.SetData();
            }
        }
        PerkGM.SetActive(true);
    }
}
