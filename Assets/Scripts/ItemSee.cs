using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSee : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item itemIs;
    public Vector3 offSet;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemIs != null)
        {
                itemIs.GetDescription();
                GameManager.Instance.discItemGM.SetActive(true);
                GameManager.Instance.discItemGM.transform.localPosition = new Vector3(transform.localPosition.x - offSet.x, 
                transform.localPosition.y - offSet.y, 
                transform.localPosition.z + offSet.z);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemIs != null)
        {
                GameManager.Instance.discItemGM.SetActive(false);
        }
    }
}