using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[CreateAssetMenu(fileName = "NewItemHealthRemove", menuName = "Items/ItemHealthRemove")]
public class ItemHealthRemove : Item
{
   public int hpAdd;
   public int hpPerRound;
   public int maxHpAdd;

    public override void GetBuff()
    {
        base.GetBuff();

        if (GameManager.Instance != null && GameManager.Instance.gameObject != null)
        {
            if(GameManager.Instance.Health + hpAdd>0)
            {
                GameManager.Instance.Health += hpAdd;
            }
            else
            {
                GameManager.Instance.Health = 1;
            }
            GameManager.Instance.maxHealth += maxHpAdd;
            GameManager.Instance.restoreHeal += hpPerRound;
        }
        
    }
    public override void GetBuffSave()
    {
        if (GameManager.Instance != null && GameManager.Instance.gameObject != null)
            {
                if(GameManager.Instance.Health + hpAdd>0)
                {
                    GameManager.Instance.Health += hpAdd;
                }
                else
                {
                    GameManager.Instance.Health = 1;
                }
                GameManager.Instance.maxHealth += maxHpAdd;
                GameManager.Instance.restoreHeal += hpPerRound;
            }
        count++;
    }
     public override void GetDescription()
    {
        base.GetDescription();
        UpdateHealthStatText(hpAdd, GameManager.Instance.DiscriptionStatText[9], "");
        UpdateHealthStatText(hpPerRound, GameManager.Instance.DiscriptionStatText[10], "");
        UpdateHealthStatText(maxHpAdd, GameManager.Instance.DiscriptionStatText[11], "");
        GameManager.Instance.DiscriptionText.text = string.Format(disc , hpAdd * count, hpPerRound * count, maxHpAdd * count);
    }
    public override string GetDescriptionItem()
    {
        return string.Format(disc , hpAdd * (count + 1), hpPerRound * (count + 1), maxHpAdd * (count + 1));
    }
    private void UpdateHealthStatText(float value, TMP_Text textComponent, string prefix)
    {
        if (value != 0)
        {
            textComponent.gameObject.transform.parent.gameObject.SetActive(true);
            textComponent.text = prefix + (value * count).ToString();
        }
        else
        {
            textComponent.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}