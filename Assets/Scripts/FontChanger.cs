using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FontChanger : MonoBehaviour
{
     public Font newFont; // Новый шрифт Unity Text

    private void Start()
    {
        ChangeAllFonts(newFont); // Применение нового шрифта при запуске
    }

    private void ChangeAllFonts(Font font)
    {
        // Получение всех объектов Text в сцене
        Text[] textObjects = FindObjectsOfType<Text>();

        // Применение нового шрифта ко всем объектам
        foreach (var text in textObjects)
        {
            if(text.font== null)
            {
                text.font = font; // Изменение шрифта
            }
        }
    }
}