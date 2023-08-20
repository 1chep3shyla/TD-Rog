using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public Rolling rollBase;
    public GameObject monster;
    public int level;
    public GameObject curGM;


    public bool CanPlaceMonster()
    {
        return monster == null;
    }

    void OnMouseUp()
    {
        Debug.Log("Попал" + " " + gameObject.name);

            if (rollBase.choosing && rollBase.towerPrefab != curGM && rollBase.towerPrefab != null && curGM != null)
            {
                Up();
                Debug.Log("Up1");
            }
            else if (!CanPlaceMonster() && curGM.GetComponent<UpHave>().UpVersion != null && rollBase.towerPrefab != null && curGM != null && !rollBase.choosing)
            {
                Up();
                Debug.Log("Up");
            }
            else if(curGM !=null)
            {
                rollBase.towerPrefab = curGM;
                rollBase.choosing = true;
                rollBase.unPanel.SetActive(true);
                rollBase.OrderUp();
            }

        


    }


    public void Up()
    {
        rollBase.unPanel.SetActive(false);
        bool change = false;
        if (rollBase.towerPrefab.GetComponent<UpHave>().UpVersion != null)
        {
            if (rollBase.towerPrefab.GetComponent<UpHave>().id == curGM.GetComponent<UpHave>().id && !rollBase.choosing && rollBase.towerPrefab != curGM)
            {
                for (int o = 0; o < rollBase.slots.Length; o++)
                {
                    rollBase.butChoose[o].interactable = false;
                    rollBase.butChoose[o].gameObject.SetActive(false);
                    rollBase.pressSpace.SetActive(true);
                }
                Debug.Log("Апнул просто");
                GameObject newVer = Instantiate(curGM.GetComponent<UpHave>().UpVersion, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), Quaternion.identity);
                Destroy(curGM);
                rollBase.AddTower(newVer.GetComponent<SpriteRenderer>());
                curGM = newVer;
                curGM.GetComponent<UpHave>().baseOf = this;
                rollBase.towerPrefab = null;
                rollBase.choosing = false;
                rollBase.UpLevelAnim(transform);
                change = true;
            }
            else if (!CanPlaceMonster() && curGM.GetComponent<UpHave>().UpVersion != null && rollBase.towerPrefab != null && curGM != null) 
            {
                Debug.Log("Апнул не просто");
                GameObject newVer = Instantiate(curGM.GetComponent<UpHave>().UpVersion, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), Quaternion.identity);
                Destroy(curGM);
                rollBase.AddTower(newVer.GetComponent<SpriteRenderer>());
                curGM = newVer;
                curGM.GetComponent<UpHave>().baseOf = this;
                rollBase.towerPrefab.GetComponent<UpHave>().baseOf.monster = null;
                rollBase.towerPrefab.GetComponent<UpHave>().baseOf.curGM = null;
                rollBase.UpLevelAnim(transform);
                Destroy(rollBase.towerPrefab.GetComponent<UpHave>().baseOf.gameObject);
                Destroy(rollBase.towerPrefab);
                rollBase.choosing = false;
                rollBase.towerPrefab = null;
                change = true;
            }
        }
        else if (curGM != null && !change)
        {
            rollBase.towerPrefab = curGM;
            rollBase.choosing = true;
        }
        rollBase.StartCoroutine(rollBase.Un());
        rollBase.OrderDown();
    }


}
