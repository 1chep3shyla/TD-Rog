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

      public virtual void GetBuff()
    {
        for (int i = 0; i < GameManager.Instance.newBuff.Length; i++)
        {
            GameManager.Instance.buff[i] += buff[i];
        }
    }
}