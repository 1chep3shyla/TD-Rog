using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class SeeText :  MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject descriptionObject; // объект с описанием
    public TMP_Text descriptionText; // текст описания (если используется текстовое поле)
    public Color descriptionColor = Color.white; // начальный цвет описания
    public string description;

    private GameObject parentObject; // объект, на котором лежит этот скрипт

    private void Start()
    {
        parentObject = this.gameObject;

        if (descriptionObject != null)
        {
            descriptionObject.SetActive(false);
            descriptionObject.transform.SetParent(parentObject.transform);
            descriptionObject.transform.localPosition = new Vector3(0, 60, 0);
        }

        UpdateDescriptionColor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (descriptionObject != null)
        {
            descriptionObject.SetActive(true);
            descriptionObject.transform.SetParent(parentObject.transform);
            descriptionObject.transform.localPosition = new Vector3(0, 60, 0);
        }

        if (descriptionText != null)
        {
            descriptionText.text = description;
            UpdateDescriptionColor();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (descriptionObject != null)
        {
            descriptionObject.SetActive(false);
        }
    }

    // Метод для обновления цвета текста описания
    private void UpdateDescriptionColor()
    {
        if (descriptionText != null)
        {
            descriptionText.color = descriptionColor;
        }
    }
}