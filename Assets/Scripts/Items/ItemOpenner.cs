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

    private Item curItem;

    private Animator chestAnimator;
    private Animator iconAnimator;

    private HoverDetector sellButtonHoverDetector;
    private HoverDetector claimButtonHoverDetector;

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
        if(countChest > 0 && !openning && !claimReward)
        {
            StartCoroutine(OpenChestCoroutine());
            countChest -=1;
            allOpen = false;
        }
        else if (countChest <= 0 && !openning && !claimReward)
        {
            allOpen = true;
            ps[4].gameObject.SetActive(false);
            Light.SetActive(false);
            panelOpen.SetActive(false);
            iconImage.gameObject.SetActive(false);
        }
        else if (openning && !claimReward)
        {
            StopCoroutine(OpenChestCoroutine());
            claimReward = true;
            iconImage.gameObject.SetActive(true);
            Item selectedItem = GetRandomItem();
            curItem = selectedItem;
            iconImage.sprite = selectedItem.iconTrans;
            nameText.text = selectedItem.name;
            ps[5].Stop();
            if(selectedItem.Rarity == typeItem.def)
            {
                sellCost = 150;
                sellText.text = "Sell for " + sellCost;
            }
            else if(selectedItem.Rarity == typeItem.rare)
            {
                sellCost = 450;
                sellText.text = "Sell for " + sellCost;
            }
            else if(selectedItem.Rarity == typeItem.mythic)
            {
                sellCost = 750;
                sellText.text = "Sell for " + sellCost;
            }
            nameText.gameObject.SetActive(true);
            Light.SetActive(false);

            sellButton.interactable = true;
            claimButton.interactable = true;
        }
    }

    IEnumerator OpenChestCoroutine()
    {
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
        if(!claimReward)
        {
        claimReward = true;
        iconImage.gameObject.SetActive(true);
        Item selectedItem = GetRandomItem();
        curItem = selectedItem;
        iconImage.sprite = selectedItem.iconTrans;
        nameText.text = selectedItem.name;
        yield return new WaitForSecondsRealtime(1.8f);
        yield return new WaitForSecondsRealtime(0.25f);
        ps[5].Stop();
        if(selectedItem.Rarity == typeItem.def)
        {
            sellCost = 150;
            sellText.text = "Sell for " + sellCost;
        }
        else if(selectedItem.Rarity == typeItem.rare)
        {
            sellCost = 450;
            sellText.text = "Sell for " + sellCost;
        }
        else if(selectedItem.Rarity == typeItem.mythic)
        {
            sellCost = 750;
            sellText.text = "Sell for " + sellCost;
        }
        nameText.gameObject.SetActive(true);
        Light.SetActive(false);

        sellButton.interactable = true;
        claimButton.interactable = true;
        }
    }

    public void Sell()
    {
        openning = false;
        claimReward = false;
        GameManager.Instance.AddMoney(sellCost);
        curItem = null;
        OpenChest();
    }

    public void Claim()
{
    openning = false;
    claimReward = false;
    // Add your logic here
    if (curItem != null)
    {

        bool itemExists = false;
        foreach (Item item in items)
        {
            if (item == curItem)
            {
                item.count++;
                item.GetBuff();
                itemExists = true;
                break;
            }
            itemExists = false;
        }

        if (!itemExists)
        {
            Debug.Log("Up Count");
            System.Array.Resize(ref items, items.Length + 1);
            items[items.Length - 1] = curItem;
            CreateItem();
            items[items.Length - 1].GetBuff();
        }
    }
    OpenChest();
}

    public void CreateItem()
    {
        if (prefabItem != null)
        {
            GameObject newItem = Instantiate(prefabItem, transform.position, Quaternion.identity);
            newItem.transform.parent = headPrefab; 
            newItem.transform.localScale = new Vector3(1, 1, 1);

            Image imageComponent = newItem.transform.GetChild(0).GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = curItem.icon;
            }

            curItem = null;
        }
    }

    private Item GetRandomItem()
    {
        float randomValue = Random.value;

        if (randomValue < 0.70f) // 70% chance for default item
            return defItem[Random.Range(0, defItem.Length)];
        else if (randomValue < 0.95f) // 25% chance for rare item
            return rareItem[Random.Range(0, rareItem.Length)];
        else // 5% chance for epic item
            return epicItem[Random.Range(0, epicItem.Length)];
    }

    private void Update()
    {
        iconAnimator.SetBool("Sell", sellButtonHoverDetector.IsHovered);
        if(!sellButtonHoverDetector.IsHovered && !claimButtonHoverDetector.IsHovered)
        {
            ps[4].gameObject.SetActive(true);
        }
        else
        {
            ps[4].gameObject.SetActive(false);
        }
        iconAnimator.SetBool("Claim", claimButtonHoverDetector.IsHovered);
    }
}