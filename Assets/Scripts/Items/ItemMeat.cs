using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/ItemMeat")]
public class ItemMeat : Item
{
    public int newHp;

    public override void GetBuff()
    {
        base.GetBuff();

        if (GameManager.Instance != null && GameManager.Instance.gameObject != null)
        {
            PerkRoll rollingComponent = GameManager.Instance.gameObject.GetComponent<PerkRoll>();
            
            if (rollingComponent != null)
            {
                Button[] buttons = rollingComponent.buttons;

                // Уменьшаем значение i на 1 и проверяем i >= 0
                for (int i = buttons.Length - 1; i >= 0; i--)
                {
                    if (buttons[i].interactable)
                    {
                        buttons[i].interactable = false;
                        GameManager.Instance.maxHealth += newHp;
                        GameManager.Instance.Health += newHp;
                        break;
                    }
                }
            }
        }
        
    }
    public override void GetBuffSave()
    {
         if (GameManager.Instance != null && GameManager.Instance.gameObject != null)
        {
            PerkRoll rollingComponent = GameManager.Instance.gameObject.GetComponent<PerkRoll>();
            
            if (rollingComponent != null)
            {
                Button[] buttons = rollingComponent.buttons;

                // Уменьшаем значение i на 1 и проверяем i >= 0
                for (int i = buttons.Length - 1; i >= 0; i--)
                {
                    if (buttons[i].interactable)
                    {
                        buttons[i].interactable = false;
                        GameManager.Instance.maxHealth += newHp;
                        GameManager.Instance.Health += newHp;
                        break;
                    }
                }
            }
        }
        count++;
    }
     public override void GetDescription()
    {
        base.GetDescription();
        GameManager.Instance.DiscriptionText.text = string.Format(disc , newHp * count, count);
    }
    public override string GetDescriptionItem()
    {
        return string.Format(disc , newHp * count, count);
    }
}