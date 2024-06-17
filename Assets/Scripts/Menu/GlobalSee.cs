using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GlobalSee : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text costText;
    public TMP_Text soulText;
    [Space]
    public Button buyBut;
    public Button[] buttonUps;
    [Space]
    public Image iconImage;
    public Image[] sliders;
    [Space]
    public GlobalUp[] allUps;
    public GlobalUp allUpsCur;
    public int curIndex;
    void Start()
    {
        UpdateFillAmounts();
        for(int i =0; i < allUps.Length;i++)
        {
            allUps[i].bought = GameBack.instance.boughtStat[i];
            if(allUps[i].bought)
            {
                allUps[i].ApplyBuffSave();
            }
        }
    }
    public void Update()
    {
        soulText.text = GameBack.Instance.souls.ToString("");
    }
    public void SetData(int index)
    {
        UpdateFillAmounts();
        allUpsCur = allUps[index];
        curIndex = index;
        nameText.text = allUps[index].name;
        iconImage.sprite = allUps[index].icon;
        descriptionText.text = string.Format(allUps[index].description, allUps[index].onWhichUp);
        if(allUps[index].bought)
        {
            buyBut.gameObject.SetActive(false);
            costText.text = "Bought";
        }
        else
        {
            buyBut.gameObject.SetActive(true);
            costText.text = allUps[index].cost.ToString("");
        }
    }
    public void Buy()
    {
        allUpsCur.ApplyBuff(curIndex);
        UpdateFillAmounts();
    }
    void UpdateFillAmounts()
    {
        int numGroups = sliders.Length; // количество групп (количество изображений)
        
        for (int i = 0; i < numGroups; i++)
        {
            float fill = 0f;
            int startIndex = i * 4; 

            for (int a = startIndex; a < startIndex + 4 && a < allUps.Length; a++)
            {
                if (allUps[a].bought)
                {
                    fill += 0.25f; 
                    if (a+1 < buttonUps.Length) // Проверка, чтобы избежать выхода за пределы массива кнопок
                    {
                        buttonUps[a+1].interactable = true; // делаем кнопку интерактивной
                    }
                }
            }

            sliders[i].fillAmount = fill;
        }
    }
}
