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
    public bool animHas;
    public string anim1;
    public string anim2;
    public Vector3 offSet;

    private GameObject parentObject; // объект, на котором лежит этот скрипт
 private bool isPointerInside = false; // флаг, указывающий, находится ли указатель внутри объекта

    private void Start()
    {
        parentObject = this.gameObject;
        descriptionObject.SetActive(false);
        if (descriptionObject != null && !need)
        {
            descriptionObject.transform.position = new Vector3(transform.position.x - offSet.x,
                transform.position.y - offSet.y,
                transform.position.z + offSet.z);
        }

        UpdateDescriptionColor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!work)
        {
            if (descriptionObject != null && !need)
            {
                descriptionObject.transform.position = new Vector3(transform.position.x + offSet.x,
                    transform.position.y + offSet.y,
                    transform.position.z + offSet.z);
            }
            descriptionObject.SetActive(true);
            if (animHas)
            {
                descriptionObject.GetComponent<Animator>().SetTrigger("On");
            }

            if (descriptionText != null)
            {
                descriptionText.text = description;
                UpdateDescriptionColor();
            }
        }
        else
        {
            descriptionObject.GetComponent<Animator>().SetTrigger("On");
        }
        isPointerInside = true; // указатель внутри объекта
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false; // указатель покинул объект
        if (descriptionObject != null && !work)
        {
            if (!animHas)
            {
                descriptionObject.SetActive(false);
            }
            else
            {
                if (!work && !descriptionObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(anim1))
                {
                    descriptionObject.GetComponent<Animator>().SetTrigger("Off");
                }
                descriptionObject.GetComponent<Animator>().ResetTrigger("On");
            }
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
        work = !work;
        if (work)
        {
            descriptionObject.GetComponent<Animator>().ResetTrigger("Off");
        }
        else if (!isPointerInside)
        {
            // Запустить анимацию "Off" только если курсор не над объектом
            descriptionObject.GetComponent<Animator>().SetTrigger("Off");
        }
    }
}