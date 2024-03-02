using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public string name;
    public int count;
    public Sprite icon;
    public Sprite iconTrans;
    public float[] buff;

      public virtual void GetBuff()
    {
        for (int i = 0; i < GameManager.Instance.newBuff.Length; i++)
        {
            GameManager.Instance.newBuff[i] += buff[i];
        }
    }
}