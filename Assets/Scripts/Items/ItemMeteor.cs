using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/ItemMeteor")]
public class ItemMeteor : Item
{
    public int indexThing;
    public GameObject meteorController;
    public GeneratingOnPath generatingObject;

    public override void GetBuff()
    {
        if (count <= 0)
        {
            GameObject meteorCon = Instantiate(meteorController, new Vector3(0, 0, 0), Quaternion.identity);
            GeneratingOnPath path =  meteorCon.GetComponent<GeneratingOnPath>();
            path.points = GameManager.Instance.spawn.waypoints[0].waypoints;
            InventoryController.Instance.things[indexThing] =  meteorCon;
        }
        else
        {
            generatingObject = InventoryController.Instance.things[indexThing].GetComponent<GeneratingOnPath>();
            generatingObject.numberOfObjects++;
            generatingObject.delayBetweenRepeats -= generatingObject.delayBetweenRepeats / 6f;
        }

        base.GetBuff();
    }
    public override void GetBuffSave()
    {
        if (count <= 0)
        {
            GameObject meteorCon = Instantiate(meteorController, new Vector3(0, 0, 0), Quaternion.identity);
            GeneratingOnPath path =  meteorCon.GetComponent<GeneratingOnPath>();
            path.points = GameManager.Instance.spawn.waypoints[0].waypoints;
            InventoryController.Instance.things[indexThing] =  meteorCon;
        }
        else
        {
            generatingObject = InventoryController.Instance.things[indexThing].GetComponent<GeneratingOnPath>();
            generatingObject.numberOfObjects++;
            generatingObject.delayBetweenRepeats -= generatingObject.delayBetweenRepeats / 6f;
        }
        count++;
    }
    public override void GetDescription()
    {
        base.GetDescription();
        GameManager.Instance.DiscriptionText.text = string.Format( count.ToString(""));
    }
      public override string GetDescriptionItem()
    {
        return string.Format( count.ToString(""));
    }
}