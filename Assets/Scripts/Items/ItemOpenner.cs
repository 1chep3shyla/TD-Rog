using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemOpenner : MonoBehaviour
{
 public int countChest;
    public Item[] items;
    [Space]
    public Item[] defItem;
    public Item[] rareItem;
    public Item[] epicItem;

    public GameObject chestAnimatorObject;
    public GameObject panelOpen;
    public GameObject Light;
    public GameObject prefabItem;
    public TMP_Text nameText;
    public TMP_Text countText;
    public Button sellButton;
    public Button claimButton;
    public Image iconImage;
    public ParticleSystem[] ps;
    public Transform headPrefab;
    public Text sellText;
    public int sellCost;
    public bool allOpen;
    public bool openning;
    public bool claimReward;
    public GameObject discriptionGM;
    public TMP_Text discriptionText;
    public TMP_Text[] statText;
    public TMP_Text[] countTextItems;

    public Item curItem;

    private Animator chestAnimator;
    private Animator iconAnimator;

    private HoverDetector sellButtonHoverDetector;
    private HoverDetector claimButtonHoverDetector;

    void Update()
    {
        countText.text = countChest.ToString();
        for (int i = 0; i < countTextItems.Length; i++)
        {
            if (countTextItems[i] != null)
            {
                countTextItems[i].text = "x" + items[i].count;
            }
        }
    }

    private void Awake()
    {
        foreach (Item item in defItem)
        {
            item.count = 0;
        }
        foreach (Item item in rareItem)
        {
            item.count = 0;
        }
        foreach (Item item in epicItem)
        {
            item.count = 0;
        }
    }

    private void Start()
    {
        chestAnimator = chestAnimatorObject.GetComponent<Animator>();
        iconAnimator = iconImage.gameObject.GetComponent<Animator>();

        sellButtonHoverDetector = sellButton.gameObject.AddComponent<HoverDetector>();
        claimButtonHoverDetector = claimButton.gameObject.AddComponent<HoverDetector>();

        // Disable buttons initially
        sellButton.interactable = false;
        claimButton.interactable = false;

        // Disable animations initially
        chestAnimator.SetBool("SellHover", false);
        chestAnimator.SetBool("ClaimHover", false);
    }

    public void OpenChest()
    {
        chestAnimatorObject.SetActive(false);
        claimButton.gameObject.SetActive(false);
        sellButton.gameObject.SetActive(false);

        if (countChest > 0)
        {
            panelOpen.SetActive(true);
            claimButton.gameObject.SetActive(true);
            sellButton.gameObject.SetActive(true);
        }

        if (countChest > 0 && !openning && !claimReward)
        {
            StartCoroutine(OpenChestCoroutine());
            countChest -= 1;
            allOpen = false;
        }
        else if (openning && !claimReward)
        {
            StopCoroutine(OpenChestCoroutine());
            claimReward = true;
            iconImage.gameObject.SetActive(true);
            Item selectedItem = GetRandomItem();
            curItem = selectedItem;
            discriptionGM.SetActive(true);
            discriptionGM.GetComponent<Animator>().Play("dis_anim");
            discriptionText.text = selectedItem.GetDescriptionItem();
            for (int i = 0; i < statText.Length; i++)
            {
                if (selectedItem.buff[i] == 0)
                {
                    statText[i].gameObject.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    statText[i].gameObject.transform.parent.gameObject.SetActive(true);
                    statText[i].text = selectedItem.buff[i] + "%";
                }
            }
            iconImage.sprite = selectedItem.iconTrans;
            nameText.text = selectedItem.name;
            ps[5].Stop();
            if (selectedItem.Rarity == typeItem.def)
            {
                sellCost = 150;
                sellText.text = "Sell for " + sellCost;
            }
            else if (selectedItem.Rarity == typeItem.rare)
            {
                sellCost = 450;
                sellText.text = "Sell for " + sellCost;
            }
            else if (selectedItem.Rarity == typeItem.mythic)
            {
                sellCost = 750;
                sellText.text = "Sell for " + sellCost;
            }
            nameText.gameObject.SetActive(true);
            Light.SetActive(false);
            claimButton.gameObject.SetActive(true);
            sellButton.gameObject.SetActive(true);
            sellButton.interactable = true;
            claimButton.interactable = true;
        }
    }

    IEnumerator OpenChestCoroutine()
    {
        discriptionGM.SetActive(false);
        claimButton.gameObject.SetActive(false);
        sellButton.gameObject.SetActive(false);
        chestAnimatorObject.SetActive(true);
        openning = true;
        nameText.text = "";
        sellButton.interactable = false;
        claimButton.interactable = false;
        chestAnimator.Play("chest_start");
        ps[4].gameObject.SetActive(false);
        Light.SetActive(false);
        panelOpen.SetActive(true);
        iconImage.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.5f);
        chestAnimator.SetTrigger("Open");
        ps[0].Play();
        yield return new WaitForSecondsRealtime(0.25f);
        ps[1].Play();
        yield return new WaitForSecondsRealtime(0.25f);
        ps[5].Play();
        ps[2].Play();
        Light.SetActive(true);
        yield return new WaitForSecondsRealtime(1.7f);
        ps[3].Play();
        yield return new WaitForSecondsRealtime(0.1f);
        ps[4].gameObject.SetActive(true);

        if (!claimReward)
        {
            claimReward = true;
            iconImage.gameObject.SetActive(true);
            Item selectedItem = GetRandomItem();
            curItem = selectedItem;
            discriptionGM.SetActive(true);
            discriptionGM.GetComponent<Animator>().Play("dis_anim");
            discriptionText.text = selectedItem.GetDescriptionItem();
            for (int i = 0; i < statText.Length; i++)
            {
                if (selectedItem.buff[i] == 0)
                {
                    statText[i].gameObject.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    statText[i].gameObject.transform.parent.gameObject.SetActive(true);
                    statText[i].text = selectedItem.buff[i] + "%";
                }
            }
            iconImage.sprite = selectedItem.iconTrans;
            nameText.text = selectedItem.name;
            yield return new WaitForSecondsRealtime(1.8f);
            yield return new WaitForSecondsRealtime(0.25f);
            ps[5].Stop();
            if (selectedItem.Rarity == typeItem.def)
            {
                sellCost = 150;
                sellText.text = "Sell for " + sellCost;
            }
            else if (selectedItem.Rarity == typeItem.rare)
            {
                sellCost = 450;
                sellText.text = "Sell for " + sellCost;
            }
            else if (selectedItem.Rarity == typeItem.mythic)
            {
                sellCost = 750;
                sellText.text = "Sell for " + sellCost;
            }
            nameText.gameObject.SetActive(true);
            Light.SetActive(false);
            claimButton.gameObject.SetActive(true);
            sellButton.gameObject.SetActive(true);
            sellButton.interactable = true;
            claimButton.interactable = true;
        }
    }

    public void Sell()
    {
        if (!claimReward) return;

        openning = false;
        claimReward = false;
        GameManager.Instance.AddMoney(sellCost);
        curItem = null;

        if (countChest == 0)
        {
            panelOpen.SetActive(false);
        }
        else
        {
            panelOpen.SetActive(true);
            OpenChest();
        }
    }

    public void Claim()
    {
        if (!claimReward) return;

        openning = false;
        claimReward = false;
        if (curItem != null)
        {
            bool itemExists = false;
            foreach (Item item in items)
            {
                if (item == curItem)
                {
                    item.GetBuff();
                    itemExists = true;
                    break;
                }
            }

            if (!itemExists)
            {
                System.Array.Resize(ref items, items.Length + 1);
                items[items.Length - 1] = curItem;
                CreateItem();
                items[items.Length - 1].GetBuff();
            }
        }

        if (countChest == 0)
        {
            panelOpen.SetActive(false);
        }
        else
        {
            panelOpen.SetActive(true);
            OpenChest();
        }
    }

    public void ClaimSave()
    {
        if (curItem != null)
        {
            bool itemExists = false;
            foreach (Item item in items)
            {
                if (item == curItem)
                {
                    item.GetBuffSave();
                    itemExists = true;
                    break;
                }
            }

            if (!itemExists)
            {
                System.Array.Resize(ref items, items.Length + 1);
                items[items.Length - 1] = curItem;
                CreateItem();
                items[items.Length - 1].GetBuffSave();
            }
        }
    }

    public void CreateItem()
    {
        if (prefabItem != null)
        {
            GameObject newItem = Instantiate(prefabItem, transform.position, Quaternion.identity);
            newItem.transform.SetParent(headPrefab);
            newItem.transform.localScale = new Vector3(1, 1, 1);

            Image imageComponent = newItem.transform.GetChild(0).GetComponent<Image>();
            System.Array.Resize(ref countTextItems, countTextItems.Length + 1);
            countTextItems[countTextItems.Length - 1] = newItem.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            if (imageComponent != null)
            {
                imageComponent.sprite = curItem.icon;
            }

            // Получаем скрипт ItemSee и устанавливаем itemIs
            ItemSee itemSee = newItem.GetComponent<ItemSee>();
            if (itemSee != null)
            {
                itemSee.itemIs = curItem;
            }

            curItem = null;
        }
    }

    private Item GetRandomItem()
    {
        float randomValue = Random.value;

        if (randomValue < 0.65f) // 65% chance for default item
            return defItem[Random.Range(0, defItem.Length)];
        else if (randomValue < 0.85f) // 20% chance for rare item
            return rareItem[Random.Range(0, rareItem.Length)];
        else // 15% chance for epic item
            return epicItem[Random.Range(0, epicItem.Length)];
    }
}
