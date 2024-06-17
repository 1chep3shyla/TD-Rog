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
                //GameManager.Instance.discItemGM.transform.SetParent(this.gameObject.transform);
                GameManager.Instance.discItemGM.SetActive(true);
                GameManager.Instance.discItemGM.transform.position = new Vector3(transform.position.x - offSet.x, 
                transform.position.y - offSet.y, 
                transform.position.z + offSet.z);
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