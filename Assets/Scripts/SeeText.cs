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
    public bool need;
    public bool work;
    public Vector3 offSet;

    private GameObject parentObject; // объект, на котором лежит этот скрипт

    private void Start()
    {
        parentObject = this.gameObject;
        descriptionObject.SetActive(false);
        if (descriptionObject != null && !need)
        {
            //descriptionObject.transform.SetParent(parentObject.transform);
            descriptionObject.transform.position = new Vector3(transform.position.x - offSet.x, 
            transform.position.y - offSet.y, 
            transform.position.z + offSet.z);
        }

        UpdateDescriptionColor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!work)
        {
        if (descriptionObject != null && !need)
        {
            //descriptionObject.transform.SetParent(parentObject.transform);
            descriptionObject.transform.position = new Vector3(transform.position.x + offSet.x, 
            transform.position.y + offSet.y, 
            transform.position.z + offSet.z);
        }
        descriptionObject.SetActive(true);
        if (descriptionText != null)
        {
            descriptionText.text = description;
            UpdateDescriptionColor();
        }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (descriptionObject != null && !work)
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
    public void Work()
    {
        if(!work)
        {
            work = true;
        }
        else
        {
            work = false;
        }
    }
}