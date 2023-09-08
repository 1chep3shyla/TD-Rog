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
        for (int i = 0; i < GameManager.Instance.allEvolution.Length; i++)
        {
            if (GameManager.Instance.allEvolution[i].index == curGM.GetComponent<UpHave>().id && GameManager.Instance.allEvolution[i].work == true)
            {
                Destroy(curGM);
                GameObject newGM = Instantiate(GameManager.Instance.allEvolution[i].EvolveScript, transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), Quaternion.identity);
                monster = newGM;
                curGM = newGM;
                curGM.GetComponent<UpHave>().baseOf = this;
                rollBase.AddTower(curGM.GetComponent<SpriteRenderer>());
    }
        }
    }
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
        else if (!CanPlaceMonster() && rollBase.towerPrefab != null && curGM != null && !rollBase.choosing)
        {
            Up();
        }
        else if(rollBase.choosing && rollBase.towerPrefab != curGM && rollBase.towerPrefab.GetComponent<UpHave>().id != -2 && rollBase.towerPrefab != null)
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
                else if (!CanPlaceMonster() && rollBase.towerPrefab != null && curGM != null)
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
                else if (rollBase.towerPrefab.GetComponent<UpHave>().id == curGM.GetComponent<UpHave>().id && !CanPlaceMonster() && rollBase.towerPrefab != null && curGM != null)
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
                if (rollBase.towerPrefab.GetComponent<UpHave>().id == curGM.GetComponent<UpHave>().id && !rollBase.choosing && rollBase.towerPrefab != curGM && curGM.GetComponent<UpHave>().LVL == rollBase.towerPrefab.GetComponent<UpHave>().LVL)
                {
                    JustUp();
                    change = true;
                }
                else if (rollBase.towerPrefab.GetComponent<UpHave>().id == curGM.GetComponent<UpHave>().id && !CanPlaceMonster() && rollBase.towerPrefab != null && curGM.GetComponent<UpHave>().LVL == rollBase.towerPrefab.GetComponent<UpHave>().LVL)
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
        Debug.Log(curGM.name);
        Debug.Log(rollBase.towerPrefab.name);
        GameObject NewcurGM = curGM;
        TowerBase newTB = rollBase.towerPrefab.GetComponent<UpHave>().baseOf;
        monster = rollBase.towerPrefab.GetComponent<UpHave>().baseOf.monster;
        curGM.transform.position = rollBase.towerPrefab.GetComponent<UpHave>().baseOf.curGM.transform.position;
        curGM = rollBase.towerPrefab.GetComponent<UpHave>().baseOf.curGM;
        curGM.GetComponent<UpHave>().baseOf = newTB;
        rollBase.towerPrefab.GetComponent<UpHave>().baseOf.curGM.transform.position = trans;
        rollBase.towerPrefab.GetComponent<UpHave>().baseOf.monster = monstr;
        rollBase.towerPrefab.GetComponent<UpHave>().baseOf.curGM = NewcurGM;
        rollBase.towerPrefab.GetComponent<UpHave>().baseOf = this;


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
        curGM.GetComponent<UpHave>().LVL++;
        rollBase.AddTower(curGM.GetComponent<SpriteRenderer>());
        curGM.GetComponent<UpHave>().baseOf = this;
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
        curGM.GetComponent<UpHave>().LVL++;
        rollBase.AddTower(curGM.GetComponent<SpriteRenderer>());
        rollBase.towerPrefab.GetComponent<UpHave>().baseOf.monster = null;
        rollBase.towerPrefab.GetComponent<UpHave>().baseOf.curGM = null;
        rollBase.UpLevelAnim(transform);
        Destroy(rollBase.towerPrefab.GetComponent<UpHave>().baseOf.gameObject);
        Destroy(rollBase.towerPrefab);
        rollBase.choosing = false;
        rollBase.towerPrefab = null;
    }


}
