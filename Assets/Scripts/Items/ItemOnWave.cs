using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/ItemOnWave")]
public class ItemOnWave : Item
{
    public GameObject gameObjectCreate;
   public override void GetBuff()
    {
         GameObject meteorCon = Instantiate(gameObjectCreate, new Vector3(Random.Range(-4f,4f), Random.Range(-4f,4f), 0), Quaternion.identity);
        base.GetBuff();
    }
    public override void GetBuffSave()
    {
         GameObject meteorCon = Instantiate(gameObjectCreate, new Vector3(Random.Range(-4f,4f), Random.Range(-4f,4f), 0), Quaternion.identity);
         count++;
    }
    public override void GetDescription()
    {
        base.GetDescription();
        GameManager.Instance.DiscriptionText.text = string.Format(disc , count.ToString(""));
    }
    public override string GetDescriptionItem()
    {
        return string.Format(disc, (count+1).ToString(""));
    }
}