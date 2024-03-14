using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum typeItem
{
    def,
    rare,
    mythic
}
[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public string name;
    public int count;
    public typeItem Rarity;
    public Sprite icon;
    public Sprite iconTrans;
    public float[] buff;
    [TextArea]
    public string disc;

    public virtual void GetBuff()
    {
        count++;
        for (int i = 0; i < GameManager.Instance.newBuff.Length; i++)
        {
            GameManager.Instance.buff[i] += buff[i];
        }
        GetDescription();
    }
    public virtual void GetDescription()
    {
        GameManager.Instance.NameText.text = name;
        GameManager.Instance.DiscriptionText.text = disc;
        for(int i = 0; i < GameManager.Instance.DiscriptionStatText.Length; i++)
        {
            if( buff[i]!=0f)
            {
                GameManager.Instance.DiscriptionStatText[i].gameObject.transform.parent.gameObject.SetActive(true);
                GameManager.Instance.DiscriptionStatText[i].text =  (buff[i] * count).ToString("") + "%";
            }
            else
            {
                GameManager.Instance.DiscriptionStatText[i].gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}