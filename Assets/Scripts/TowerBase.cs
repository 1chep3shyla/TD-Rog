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
        Debug.Log("œÓÔ‡Î" + " " + gameObject.name);
        if (rollBase.choosing && rollBase.towerPrefab != curGM && rollBase.towerPrefab != null && curGM != null)
        {
            Up();
        }
        else if (!CanPlaceMonster() && curGM.GetComponent<UpHave>().UpVersion != null && rollBase.towerPrefab != null && curGM != null && !rollBase.choosing)
        {
            Up();
        }
        else if (curGM != null)
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
        if (curGM.GetComponent<UpHave>().LVL <5)
        {
            if (rollBase.towerPrefab.GetComponent<UpHave>().id == -1) // joker
            {
                if (!rollBase.choosing && rollBase.towerPrefab != curGM)
                {
                    JustUp();
                    change = true;
                }
                else if (!CanPlaceMonster() && curGM.GetComponent<UpHave>().UpVersion != null && rollBase.towerPrefab != null && curGM != null)
                {
                    JustNotUp();
                    change = true;
                }
            }
            else if (rollBase.towerPrefab.GetComponent<UpHave>().id == -2 || curGM.GetComponent<UpHave>().id == -2) // moving
            {
                if (rollBase.towerPrefab.GetComponent<UpHave>().id == curGM.GetComponent<UpHave>().id && !rollBase.choosing && rollBase.towerPrefab != curGM)
                {
                    JustUp();
                    change = true;
                }
                else if (rollBase.towerPrefab.GetComponent<UpHave>().id == curGM.GetComponent<UpHave>().id && !CanPlaceMonster() && curGM.GetComponent<UpHave>().UpVersion != null && rollBase.towerPrefab != null && curGM != null)
                {
                    JustNotUp();
                    change = true;
                }
                else if (rollBase.towerPrefab.GetComponent<UpHave>().LVL == curGM.GetComponent<UpHave>().LVL)
                {
                    Debug.Log("Ã≈Õﬂ… Ã≈—“¿Ã»");
                    MovingThis();
                    change = true;
                }
            }
            else // just up tower
            {
                if (rollBase.towerPrefab.GetComponent<UpHave>().id == curGM.GetComponent<UpHave>().id && !rollBase.choosing && rollBase.towerPrefab != curGM)
                {
                    JustUp();
                    change = true;
                }
                else if (rollBase.towerPrefab.GetComponent<UpHave>().id == curGM.GetComponent<UpHave>().id && !CanPlaceMonster() && curGM.GetComponent<UpHave>().UpVersion != null && rollBase.towerPrefab != null && curGM != null)
                {
                    JustNotUp();
                    change = true;
                }
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
    public void MovingThis()
    {

        GameObject monstr = monster;
        Vector3 trans = curGM.transform.position;
        Debug.Log(trans);
        GameObject NewcurGM = curGM;
        curGM = rollBase.towerPrefab.GetComponent<UpHave>().baseOf.curGM;
        monster = rollBase.towerPrefab.GetComponent<UpHave>().baseOf.monster;
        curGM.transform.position = rollBase.towerPrefab.GetComponent<UpHave>().baseOf.curGM.transform.position;
        rollBase.towerPrefab.GetComponent<UpHave>().baseOf.curGM.transform.position = trans;
        rollBase.towerPrefab.GetComponent<UpHave>().baseOf.monster = monstr;
        rollBase.towerPrefab.GetComponent<UpHave>().baseOf.curGM = NewcurGM;


    }
    public void JustUp()
    {
        for (int o = 0; o < rollBase.slots.Length; o++)
        {
            rollBase.butChoose[o].interactable = false;
            rollBase.butChoose[o].gameObject.SetActive(false);
            rollBase.pressSpace.SetActive(true);
        }
        Debug.Log("¿ÔÌÛÎ ÔÓÒÚÓ");
        GameObject newVer = Instantiate(curGM.GetComponent<UpHave>().UpVersion, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), Quaternion.identity);
        Destroy(curGM);
        rollBase.AddTower(newVer.GetComponent<SpriteRenderer>());
        curGM = newVer;
        curGM.GetComponent<UpHave>().baseOf = this;
        monster = newVer;
        rollBase.towerPrefab = null;
        rollBase.choosing = false;
        rollBase.UpLevelAnim(transform);
    }

    public void JustNotUp()
    {
        Debug.Log("¿ÔÌÛÎ ÌÂ ÔÓÒÚÓ");
        if (rollBase.towerPrefab.GetComponent<UpHave>().id == 76 && curGM.GetComponent<UpHave>().id == 76)
        {
            GameManager.Instance.gameObject.GetComponent<SunMoonScript>().moonCount -= 1;
        }
        else if (rollBase.towerPrefab.GetComponent<UpHave>().id == 77 && curGM.GetComponent<UpHave>().id == 77)
        {
            GameManager.Instance.gameObject.GetComponent<SunMoonScript>().sunCount -= 1;
        }
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
        monster = newVer;
        rollBase.choosing = false;
        rollBase.towerPrefab = null;
    }


}
