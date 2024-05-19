using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TowerBase : MonoBehaviour
{
    public Rolling rollBase;
    public GameObject monster;
    [SerializeField]
    public int level;
    [SerializeField]
    public GameObject curGM;
    [SerializeField]
    private Color[] colors;


    void Update()
    {
        for (int i = 0; i < GameManager.Instance.allEvolution.Length; i++)
        {
            if (GameManager.Instance.allEvolution[i].index == curGM.GetComponent<UpHave>().id && GameManager.Instance.allEvolution[i].work == true)
            {
                GameObject newGM = Instantiate(GameManager.Instance.allEvolution[i].EvolveScript, curGM.transform.position, Quaternion.identity);
                newGM.GetComponent<UpHave>().LVL = curGM.GetComponent<UpHave>().LVL;
                Destroy(curGM);
                monster = newGM;
                curGM = newGM;
                curGM.GetComponent<UpHave>().baseOf = this;
                rollBase.AddTower(curGM.GetComponent<SpriteRenderer>());
            }
        }
        GetComponent<SpriteRenderer>().color = colors[monster.GetComponent<UpHave>().LVL];
    }
    public bool CanPlaceMonster()
    {
        return monster == null;
    }

    public void OnMouseUpping()
    {
        Debug.Log("�����" + " " + gameObject.name);
        if (rollBase.choosing && rollBase.towerPrefab != curGM && rollBase.towerPrefab != null && curGM != null
        && rollBase.towerPrefab.GetComponent<UpHave>().LVL == curGM.GetComponent<UpHave>().LVL)
        {
            Up();
        }
        else if (!CanPlaceMonster() && rollBase.towerPrefab != null && curGM != null && !rollBase.choosing 
        && rollBase.towerPrefab.GetComponent<UpHave>().LVL == curGM.GetComponent<UpHave>().LVL)
        {
            Up();
        }
        else if(rollBase.choosing && rollBase.towerPrefab != curGM && rollBase.towerPrefab.GetComponent<UpHave>().id != -2 && rollBase.towerPrefab != null
        && rollBase.towerPrefab.GetComponent<UpHave>().LVL == curGM.GetComponent<UpHave>().LVL)
        {
            Up();
        }
        else if (curGM != null)
        {
            UpHave uh = curGM.GetComponent<UpHave>();
            rollBase.info.SetActive(true);
            rollBase.towerInfo[0].text = "" + uh.name;
            if(uh.description != null)
            {
                var replacementValueDEF = uh.towerDataCur.lvlData[uh.LVL, 1]; //0 - fire, 1 - poison, 2 -ice, 3 - stan, 4 - targets
                var replacementValueIce = uh.towerDataCur.lvlData[uh.LVL, 5] + (uh.towerDataCur.lvlData[uh.LVL, 5]* (GameManager.Instance.buff[1]/100));
                var replacementValueFire = uh.towerDataCur.lvlData[uh.LVL, 6] + (uh.towerDataCur.lvlData[uh.LVL, 6]* (GameManager.Instance.buff[2]/100));
                var replacementValuePoison = uh.towerDataCur.lvlData[uh.LVL, 7] + (uh.towerDataCur.lvlData[uh.LVL, 7]* (GameManager.Instance.buff[3]/100));
                var replacementValueStan = uh.towerDataCur.lvlData[uh.LVL, 8];
                var replacementValueTarget = uh.towerDataCur.lvlData[uh.LVL, 10];
                string coloredFire = $"<color=#FF0000>{replacementValueFire}</color>"; // Красный
                string coloredPoison = $"<color=#00FF00>{replacementValuePoison}</color>"; // Зеленый
                string coloredIce = $"<color=#00FFFF>{replacementValueIce}</color>"; // Голубой
                string coloredStan = $"<color=#808080>{replacementValueStan}</color>"; // Серый

                rollBase.towerInfo[1].text = string.Format(uh.description, coloredFire, 
                coloredPoison,coloredIce,coloredStan, replacementValueTarget);
            }
            if(uh.discInfo != null)
            {
                var replacementValueDEF = uh.towerDataCur.lvlData[uh.LVL, 1];
                var replacementValueGB = uh.towerDataCur.lvlData[uh.LVL, 1] +uh.towerDataCur.lvlData[uh.LVL, 1]* (GameManager.Instance.buff[0]/100); // global
                var replacementValueMB = uh.towerDataCur.lvlData[uh.LVL, 1] + uh.towerDataCur.lvlData[uh.LVL, 1] * (GameManager.Instance.buff[4]/100); //money
                rollBase.towerInfo[2].text = string.Format(uh.discInfo, replacementValueGB, replacementValueMB.ToString("0"),replacementValueDEF );
            }
            rollBase.towerInfo[3].text = "LVL:" + (uh.LVL + 1);
            rollBase.towerPrefab = curGM;
            rollBase.choosing = true;
            rollBase.OrderUp();
        }




    }


    public void Up()
    {
        bool change = false;
        if (curGM.GetComponent<UpHave>().LVL <4)
        {
            if (rollBase.towerPrefab.GetComponent<UpHave>().id == -1) // joker
            {
                if (!rollBase.choosing && rollBase.towerPrefab != curGM && rollBase.towerPrefab.GetComponent<UpHave>().LVL == curGM.GetComponent<UpHave>().LVL)
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
                    Debug.Log("����� �������");
                    MovingThis();
                    change = true;
                }
            }
            else if (rollBase.towerPrefab.GetComponent<UpHave>().id == 34) // clone
            {
                Debug.Log("cloning");
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
                    Debug.Log("����� �������");
                    GameObject newClone = Instantiate(curGM, rollBase.towerPrefab.GetComponent<UpHave>().baseOf.curGM.transform.position, Quaternion.identity);
                    rollBase.towerPrefab.GetComponent<UpHave>().baseOf.monster = newClone;
                    rollBase.AddTower(newClone.GetComponent<SpriteRenderer>());
                    newClone.GetComponent<UpHave>().baseOf = rollBase.towerPrefab.GetComponent<UpHave>().baseOf;
                    Destroy(rollBase.towerPrefab.GetComponent<UpHave>().baseOf.curGM);
                    rollBase.towerPrefab.GetComponent<UpHave>().baseOf.curGM = newClone;
                    rollBase.UnChoose();


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
                else if (curGM != null && !change)
                {
                    rollBase.towerPrefab = curGM;
                    rollBase.choosing = true;
                    UpHave uh = curGM.GetComponent<UpHave>();
                    rollBase.info.SetActive(true);
                    rollBase.towerInfo[0].text = "" + uh.name;
                    rollBase.towerInfo[2].text = "Damage:" + uh.towerDataCur.lvlData[uh.LVL, 1];
                    rollBase.towerInfo[3].text = "LVL:" + (uh.LVL + 1);

                }
            }
        }
        else if (curGM != null && !change)
        {
            rollBase.towerPrefab = curGM;
            rollBase.choosing = true;
            UpHave uh = curGM.GetComponent<UpHave>();
            rollBase.info.SetActive(true);
            rollBase.towerInfo[0].text = "" + uh.name;
            rollBase.towerInfo[2].text = "Damage:" + uh.towerDataCur.lvlData[uh.LVL, 1];
            rollBase.towerInfo[3].text = "LVL:" + (uh.LVL + 1);

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
        if (GameManager.Instance.Gold >= GameManager.Instance.gameObject.GetComponent<Rolling>().costTower)// Здесь
        {
            UpHave uh = curGM.GetComponent<UpHave>();

            Debug.Log("����� ������");
            curGM.GetComponent<UpHave>().LVL++;
            rollBase.AddTower(curGM.GetComponent<SpriteRenderer>());
            curGM.GetComponent<UpHave>().baseOf = this;
            rollBase.towerPrefab = null;
            rollBase.choosing = false;
            rollBase.butChoose[rollBase.curIndex].interactable = false;
            rollBase.UpLevelAnim(transform);
            if (uh.id == 36)
            {
                GameManager.Instance.UpSome(uh.LVL - 1, this.gameObject);
            }
            for (int i = 0; i < rollBase.slots.Length; i++)
            {
                if (rollBase.slots[i].id == uh.id )
                {
                    rollBase.OffCard(i);
                }
            }
            GameManager.Instance.Gold -= rollBase.costTower;
            GameManager.Instance.ChangeMoney();
        }
    }
    public void JustUpBoost()
    {
        Debug.Log("����� ������");
        curGM.GetComponent<UpHave>().LVL++;
        rollBase.AddTower(curGM.GetComponent<SpriteRenderer>());
        curGM.GetComponent<UpHave>().baseOf = this;
        rollBase.towerPrefab = null;
        rollBase.choosing = false;
        rollBase.UpLevelAnim(transform);
    }

    public void JustNotUp()
    {
        Debug.Log("����� �� ������");
        UpHave uh = curGM.GetComponent<UpHave>();
        if (rollBase.towerPrefab.GetComponent<UpHave>().id == 25 && uh.id == 25)
        {
            GameManager.Instance.gameObject.GetComponent<SunMoonScript>().moonCount -= 1;
        }
        else if (rollBase.towerPrefab.GetComponent<UpHave>().id == 26 && uh.id == 26)
        {
            GameManager.Instance.gameObject.GetComponent<SunMoonScript>().sunCount -= 1;
        }
        uh.LVL++;
        rollBase.AddTower(curGM.GetComponent<SpriteRenderer>());
        rollBase.towerPrefab.GetComponent<UpHave>().baseOf.monster = null;
        rollBase.towerPrefab.GetComponent<UpHave>().baseOf.curGM = null;
        rollBase.UpLevelAnim(transform);
        Destroy(rollBase.towerPrefab.GetComponent<UpHave>().baseOf.gameObject);
        Destroy(rollBase.towerPrefab);
        rollBase.choosing = false;
        rollBase.towerPrefab = null;
        if (uh.id == 36)
        {
            GameManager.Instance.UpSome(uh.LVL - 1, this.gameObject);
        }
    }


}
