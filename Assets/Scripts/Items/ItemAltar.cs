using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/ItemAltar")]
public class ItemAltar : Item
{
    public int goldGive;
    public int removeHealth;
    public int indexThing;
    public GameObject blodyGM;
    public BloodyAltar altar;

    public override void GetBuff()
    {
        if (count <= 0)
        {
            GameObject bloodyAltar = Instantiate(blodyGM, new Vector3(0, 0, 0), Quaternion.identity);
            BloodyAltar path =  bloodyAltar.GetComponent<BloodyAltar>();
            InventoryController.Instance.things[indexThing] =  bloodyAltar;
            path.goldGive += goldGive;
            path.healthRemove += removeHealth;
            path.spawner = GameManager.Instance.spawn;
            
        }
        else
        {
            altar = InventoryController.Instance.things[indexThing].GetComponent<BloodyAltar>();
            altar.goldGive += goldGive;
            altar.healthRemove += removeHealth;
        }
        base.GetBuff();
        GetDescription();
    }
    public override void GetBuffSave()
    {
         if (count <= 0)
        {
            GameObject bloodyAltar = Instantiate(blodyGM, new Vector3(0, 0, 0), Quaternion.identity);
            BloodyAltar path =  bloodyAltar.GetComponent<BloodyAltar>();
            InventoryController.Instance.things[indexThing] =  bloodyAltar;
            path.goldGive += goldGive;
            path.healthRemove += removeHealth;
            path.spawner = GameManager.Instance.spawn;
            
        }
        else
        {
            altar = InventoryController.Instance.things[indexThing].GetComponent<BloodyAltar>();
            altar.goldGive += goldGive;
            altar.healthRemove += removeHealth;
        }
        count++;
        GetDescription();
    }
    public override void GetDescription()
    {
        base.GetDescription();
        GameManager.Instance.DiscriptionText.text = string.Format(disc , goldGive * count, removeHealth * count);
    }
    public override string GetDescriptionItem()
    {
        return string.Format(disc , goldGive * count, removeHealth * count);
    }
}