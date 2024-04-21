using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class PerkRoll : MonoBehaviour
{
    public int[] chancePerk;
    public int costReroll;
    public List<ScriptableObject> allBronzePerks; // Use a List instead of an array for flexibility
    public List<ScriptableObject> allSilverPerks; // Use a List instead of an array for flexibility
    public List<ScriptableObject> allGoldenPerks; // Use a List instead of an array for flexibility
    public List<ScriptableObject> allEvolutionPerks; // Use a List instead of an array for flexibility
    public ScriptableObject[] curPerks;
    public Text[] perkText;
    public Image[] cardBack;
    public Image[] cardIcon;
    public Button[] buttons;
    public GameObject PerkGM;
    public Text[] discription;
    public bool rollingEvolve;
    private bool evolutionBool;
    public ParticleSystem[] perkPS;
    public Color[] colors;
    public Sprite[] iconSprites;
    public GameObject iconPrefab;
    public Button rerollBut;
    public TMP_Text rerollText;
    [SerializeField]
    private GameObject[] icons = new GameObject[5];

    void Start()
    {
        StartCoroutine("StartGame");
    }
    void Update()
    {
        if(evolutionBool)
        {
            rerollBut.gameObject.SetActive(false);
        }
        else
        {
            rerollBut.gameObject.SetActive(true);
        }
        if(GameManager.Instance.Gold >= costReroll)
        {
            rerollBut.interactable = true;
        }
        else
        {
             rerollBut.interactable = false;
        }
    }
    public void ChoosePerk(int index)
    {
        if (curPerks[index] is Perks perk)
        {
            Debug.Log("Perked");

            perk.ApplyPerk();
        }
        if (curPerks[index] is PerkEvolve perkEv)
        {
            if(evolutionBool)
            {
                allEvolutionPerks.Remove(perkEv);
                if(perkEv.perkNo !=null)
                {
                    allEvolutionPerks.Remove(perkEv.perkNo);
                }
            }
        }
        evolutionBool = false;
        rollingEvolve = false;
        PerkGM.SetActive(false);
    }
    public void RollPerk()
    {
        PerkGM.SetActive(false);
        chancePerk[0] = 100 - (int)System.Math.Log(GameManager.Instance.curWave * GameManager.Instance.curWave, 1.1f);
        chancePerk[1] = 100 - (int)System.Math.Log(GameManager.Instance.curWave * GameManager.Instance.curWave, 1.4f);
        List<ScriptableObject> availableBronzePerks = new List<ScriptableObject>(allBronzePerks);
        List<ScriptableObject> availableSilverPerks = new List<ScriptableObject>(allSilverPerks);
        List<ScriptableObject> availableGoldenPerks = new List<ScriptableObject>(allGoldenPerks);
        for (int i = 0; i < curPerks.Length; i++)
        {
            int random = Random.Range(0, 100);
            for (int o = 0; o < chancePerk.Length; o++) // Fixed the loop condition here
            {
                if (random < chancePerk[o])
                {
                    if (o == 0)
                    {
                        cardBack[i].color = colors[0];
                        int randomPerk = Random.Range(0, availableBronzePerks.Count);
                        perkPS[i].startColor = colors[0];
                        curPerks[i] = availableBronzePerks[randomPerk];
                        availableBronzePerks.Remove(availableBronzePerks[randomPerk]);
                    }
                    else if (o == 1)
                    {
                        cardBack[i].color = colors[1];
                        int randomPerk = Random.Range(0, availableSilverPerks.Count);
                        perkPS[i].startColor = colors[1];
                        curPerks[i] = availableSilverPerks[randomPerk];
                        availableSilverPerks.Remove(availableSilverPerks[randomPerk]);
                    }
                    else if (o == 2)
                    {
                        cardBack[i].color = colors[2];
                        int randomPerk = Random.Range(0, availableGoldenPerks.Count);
                        perkPS[i].startColor = colors[2];
                        curPerks[i] = availableGoldenPerks[randomPerk];
                        availableGoldenPerks.Remove(availableGoldenPerks[randomPerk]);
                    }
                    
                    o = 1000;
                }
            }

            if (curPerks[i] is Perks perk)
            {
                // Call the ApplyPerk method on the concrete type
                if(icons[i] != null)
                {
                    Destroy(icons[i]);
                }
                GameObject cardBackInstance = Instantiate(iconPrefab, cardBack[i].gameObject.transform.position, Quaternion.identity);
                icons[i] = cardBackInstance;
                cardBackInstance.transform.localScale = new Vector3(
                    cardBackInstance.transform.localScale.x / 108f,
                    cardBackInstance.transform.localScale.y / 108f,
                    cardBackInstance.transform.localScale.z / 108f
                    );
                cardBackInstance.transform.position -= new Vector3(0.1f,1.3f,0);
                cardBackInstance.transform.parent = cardBack[i].transform;
                Image cardImage = cardBackInstance.GetComponent<Image>();
                TMPro.TMP_Text cardText = cardBackInstance.GetComponentInChildren<TMPro.TMP_Text>();
                cardText.text = "+ " +perk.ReturnUpBuff().ToString("");
                cardImage.sprite = iconSprites[perk.indexOfBuff];
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
        evolutionBool = true;
        List<ScriptableObject> availablePerks = new List<ScriptableObject>(allEvolutionPerks);  // Создаем копию списка

        for(int i = 0; i < availablePerks.Count; i++)
        {
            if (availablePerks[i] is PerkEvolve perk)
            {
                for(int a = 0; a < GameManager.Instance.gameObject.GetComponent<Rolling>().towers.Length; a++)
                {
                    if(perk.evolveGM == GameManager.Instance.gameObject.GetComponent<Rolling>().towers[a])
                    {
                        availablePerks.Remove(perk);
                        allEvolutionPerks.Remove(perk);
                        break; // Выходим из цикла, так как элемент уже удален
                    }
                }
            }
            
        }

        for (int i = 0; i < curPerks.Length; i++)
        {
            if(icons[i] != null)
            {
                Destroy(icons[i]);
            }
            int randomPerk = Random.Range(0, availablePerks.Count);
            curPerks[i] = availablePerks[randomPerk];
            availablePerks.Remove(availablePerks[randomPerk]);
            perkPS[i].startColor = colors[3];
            cardBack[i].color = colors[3];
            if (curPerks[i] is Perks perk)
            {
                // Call the ApplyPerk method on the concrete type
                perkText[i].text = perk.SetData();
                cardIcon[i].sprite = perk.GetData();
                discription[i].text = perk.SetDataDis();
            }
            PerkGM.SetActive(true);
        }
    }
    public void RerollPerk()
    {
        if(GameManager.Instance.Gold >= costReroll)
        {
            RollPerk();
            GameManager.Instance.Gold -=costReroll;
            costReroll *= 2;
            rerollText.text = costReroll.ToString("");
            GameManager.Instance.goldCount.text = GameManager.Instance.Gold.ToString("");
        }
    }
    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3f);
        RollPerk();
        Time.timeScale = 0;
        yield return new WaitUntil(() => rollingEvolve == false);
        Time.timeScale = 1f;
    }
}

