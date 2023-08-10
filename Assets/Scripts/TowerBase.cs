using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public Rolling rollBase;
    public GameObject monster;
    public int level;
    public GameObject curGM;

    void Update()
    {
        if (curGM == null)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    private bool CanPlaceMonster()
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
            else if (CanPlaceMonster() && rollBase.towerPrefab != null && !rollBase.choosing)
            {
                Set();
                Debug.Log("Set");
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
        if (rollBase.towerPrefab.GetComponent<UpHave>().id == curGM.GetComponent<UpHave>().id && !rollBase.choosing && rollBase.towerPrefab != curGM)
        {
            Debug.Log("Апнул просто");
            GameObject newVer = Instantiate(curGM.GetComponent<UpHave>().UpVersion, transform.position, Quaternion.identity);
            Destroy(curGM);
            rollBase.AddTower(newVer.GetComponent<SpriteRenderer>());
            curGM = newVer;
            curGM.GetComponent<UpHave>().baseOf = this;
            rollBase.towerPrefab = null;

            rollBase.choosing = false;
        }
        else if (rollBase.choosing && rollBase.towerPrefab.GetComponent<UpHave>().id == curGM.GetComponent<UpHave>().id && rollBase.towerPrefab !=curGM)
        {
            Debug.Log("Апнул не просто");
            GameObject newVer = Instantiate(curGM.GetComponent<UpHave>().UpVersion, transform.position, Quaternion.identity);
            Destroy(curGM);
            rollBase.AddTower(newVer.GetComponent<SpriteRenderer>());
            curGM = newVer;
            curGM.GetComponent<UpHave>().baseOf = this;
            rollBase.towerPrefab.GetComponent<UpHave>().baseOf.monster = null;
            rollBase.towerPrefab.GetComponent<UpHave>().baseOf.curGM = null;
            rollBase.choosing = false;
            Destroy(rollBase.towerPrefab);
            rollBase.towerPrefab = null;
        }
        rollBase.OrderDown();
    }

    public void Set()
    {

        monster = (gameObject);
        curGM = Instantiate(rollBase.towerPrefab, transform.position, Quaternion.identity);
        curGM.GetComponent<UpHave>().baseOf = this;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        rollBase.towerPrefab = null;
        rollBase.AddTower(curGM.GetComponent<SpriteRenderer>());
    }
}
