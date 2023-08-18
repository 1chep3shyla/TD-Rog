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
        if (!rollBase.cant)
        {

            if (rollBase.choosing && rollBase.towerPrefab != curGM && rollBase.towerPrefab != null && curGM != null)
            {
                Up();
                Debug.Log("Up1");
            }
            else if (!CanPlaceMonster() && curGM.GetComponent<UpHave>().UpVersion != null && rollBase.towerPrefab != null && curGM != null)
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


    }


    public void Up()
    {
        if (rollBase.towerPrefab.GetComponent<UpHave>().UpVersion != null)
        {
            if (rollBase.towerPrefab.GetComponent<UpHave>().id == curGM.GetComponent<UpHave>().id && !rollBase.choosing && rollBase.towerPrefab != curGM)
            {
                Debug.Log("Апнул просто");
                GameObject newVer = Instantiate(curGM.GetComponent<UpHave>().UpVersion, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), Quaternion.identity);
                Destroy(curGM);
                rollBase.AddTower(newVer.GetComponent<SpriteRenderer>());
                curGM = newVer;
                curGM.GetComponent<UpHave>().baseOf = this;
                rollBase.towerPrefab = null;
                rollBase.UpLevelAnim(transform);
                rollBase.choosing = false;
            }
            else if (rollBase.choosing && rollBase.towerPrefab.GetComponent<UpHave>().id == curGM.GetComponent<UpHave>().id && rollBase.towerPrefab != curGM)
            {
                Debug.Log("Апнул не просто");
                GameObject newVer = Instantiate(curGM.GetComponent<UpHave>().UpVersion, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), Quaternion.identity);
                Destroy(curGM);
                rollBase.AddTower(newVer.GetComponent<SpriteRenderer>());
                curGM = newVer;
                curGM.GetComponent<UpHave>().baseOf = this;
                rollBase.towerPrefab.GetComponent<UpHave>().baseOf.monster = null;
                rollBase.towerPrefab.GetComponent<UpHave>().baseOf.curGM = null;
                rollBase.choosing = false;
                rollBase.UpLevelAnim(transform);
                Destroy(rollBase.towerPrefab.GetComponent<UpHave>().baseOf.gameObject);
                Destroy(rollBase.towerPrefab);
                rollBase.towerPrefab = null;
            }
        }
        rollBase.OrderDown();
    }


}
