using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BestiarSee : MonoBehaviour
{
    public Image[] icons;
    public Button[] buttons;
    public string[] descrip;
    public Image icon;
    public TMP_Text descripText;
    public TMP_Text typeText;

    void Start()
    {
        SetColor();
    }
    public void SetColor()
    {
         for (int i = 0; i < icons.Length; i++)
        {
            if (Beastiar.Instance.seeThis[i] == true)
            {
                icons[i].color = Color.white;
            }
            else
            {
                icons[i].color = Color.black;
            }

            // Получаем доступ к кнопке на i-м элементе

                int index = i; // сохраняем индекс для использования внутри обратного вызова
                buttons[i].onClick.AddListener(() => Set(index));           
        }
        Set(0);
    }

    public void Set(int which)
    {
        Debug.Log("Работает");
        if (Beastiar.Instance.seeThis[which] == true)
        {
            typeText.text = "???";
            descripText.text = "???";
            descripText.text = descrip[which];
            icon.color = Color.white;
            icon.sprite = icons[which].sprite;
        }
        else
        {
            typeText.text = "???";
            descripText.text = "???";
            icon.color = Color.black;
            icon.sprite = icons[which].sprite;
        }
    }
}