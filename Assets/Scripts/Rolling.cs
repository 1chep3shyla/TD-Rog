using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
using System;
public class Rolling : MonoBehaviour
{
    public Slot[] slots;
    [SerializeField]
    public TowerBase[,] allBases = new TowerBase[20, 7];
    public GameObject[] towers;
    public Button[] butChoose;
    public Sprite[] imageidTower;
    public GameObject towerPrefab;
    public GameObject unPanel;
    public GameManager GameM;
    public GameObject perk;
    public GameObject baseOfTower;
    public Tilemap tilemap;
    public TMP_Text[] nameOfTowerText;
    public TMP_Text[] costOfTowerText;
    public TMP_Text costTowerText;
    public GameObject pressSpace;
    public GameObject cursor;
    public GameObject radiusAttack;
    public GameObject newLvl;
    public GameObject info;
    public TMP_Text[] towerInfo; // 0 - name, 1 - disc, 2 - damage, 3 - lvl
    public Text sell;
    public int curIndex;
    public int costTower = 100;
    public bool choosing = false;
    public bool draging;



    public void SetSprite()
    {
        imageidTower = new Sprite[towers.Length];
        for (int i = 0; i < towers.Length; i++)
        {
            imageidTower[i] = towers[i].GetComponent<SpriteRenderer>().sprite;
        }
    }
    void Update()
    {
        if (GameManager.Instance.curWave < 10)
        {   
            costTower = 150;
            if(towerPrefab!=null)
            {
                towerPrefab.GetComponent<UpHave>().LVL = 0;
            }
        }
        else if(GameManager.Instance.curWave >= 10 && GameManager.Instance.curWave < 15 )
        {
            costTower = 300;
            if(towerPrefab!=null)
            {
                towerPrefab.GetComponent<UpHave>().LVL = 1;
            }
        }
        else if(GameManager.Instance.curWave >= 15 && GameManager.Instance.curWave < 20)
        {
            costTower = 600;
            if(towerPrefab!=null)
            {
                towerPrefab.GetComponent<UpHave>().LVL = 2;
            }
        }
        else if(GameManager.Instance.curWave >= 20 && GameManager.Instance.curWave < 25)
        {
            costTower = 1200;
            if(towerPrefab!=null)
            {
                towerPrefab.GetComponent<UpHave>().LVL = 3;
            }
        }
        else if(GameManager.Instance.curWave >= 25)
        {
            costTower = 2400;
            if(towerPrefab!=null)
            {
                towerPrefab.GetComponent<UpHave>().LVL = 4;
            }
        }
        //costTowerText.text = costTower.ToString("");
        //costTowerText.text = "300";
        HandleMouseInput();
        HandleSpaceKey();
        UpdateCursorVisibility();
    }
    public void Sell()
    {
        if (towerPrefab != null && choosing)
        {
            UpHave uh = towerPrefab.GetComponent<UpHave>();
            GameManager.Instance.Gold += 100 * (int)Math.Pow(2,  uh.LVL);
            Destroy(uh.baseOf.gameObject);
            Destroy(towerPrefab);
            GameManager.Instance.ChangeMoney();
            UnChoose();
        }
    }
    private void HandleMouseInput()
    {
        if(!perk.activeSelf)
        {
            if (Input.GetMouseButtonDown(0) && !choosing)
            {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPosition = tilemap.WorldToCell(mousePosition);
                int columnIndex = cellPosition.x;
                int rowIndex = cellPosition.y;

                //if (hit.collider != null && hit.collider.gameObject == tilemap.gameObject)
                //{
                    if (tilemap.HasTile(cellPosition))
                    {
                        Clicking(mousePosition, cellPosition, curIndex);
                        Debug.Log("����� �� ����������");
                    }
                //}
            }
            else if (Input.GetMouseButtonDown(0) && choosing)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPosition = tilemap.WorldToCell(mousePosition);
                int columnIndex = cellPosition.x;
                int rowIndex = cellPosition.y;

                Vector3 cellCenter = tilemap.GetCellCenterWorld(cellPosition);

                Ray ray = new Ray(mousePosition, Vector3.forward);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null && hit.collider.gameObject == tilemap.gameObject)
                {
                    if (tilemap.HasTile(cellPosition) && allBases[columnIndex + 10, rowIndex + 3] == null)
                    {
                        UnChoose();
                    }
                    else if (!tilemap.HasTile(cellPosition))
                    {
                        UnChoose();
                    }
                    else if (allBases[columnIndex + 10, rowIndex + 3] != null)
                    {
                        allBases[columnIndex + 10, rowIndex + 3].OnMouseUpping();
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0) && choosing)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPosition = tilemap.WorldToCell(mousePosition);
                int columnIndex = cellPosition.x;
                int rowIndex = cellPosition.y;
                if (tilemap.HasTile(cellPosition))
                {
                    if (allBases[columnIndex + 10, rowIndex + 3] != null)
                    {
                        allBases[columnIndex + 10, rowIndex + 3].OnMouseUpping();
                    }
                }
            }
        }
        
    }

    public void OffCard(int id)
    {
        butChoose[id].interactable = false;
        towerPrefab = null;
        choosing = false;
    }

    private void HandleSpaceKey()
    {
        if (Input.GetKeyDown("space"))
        {
            RollingThis();
            UnChoose();
        }
    }

    private void UpdateCursorVisibility()
    {
        cursor.SetActive(choosing && towerPrefab != null) ;
        radiusAttack.SetActive(choosing && towerPrefab != null && towerPrefab.GetComponent<Default>() != null);
        if (choosing && towerPrefab != null)
        {
            cursor.transform.position = new Vector2(towerPrefab.transform.position.x, towerPrefab.transform.position.y +0.05f);
            radiusAttack.transform.position = new Vector2(towerPrefab.transform.position.x, towerPrefab.transform.position.y );
            if(towerPrefab.GetComponent<Default>() != null)
            {
                radiusAttack.transform.localScale = new Vector2(towerPrefab.GetComponent<Default>().attackRadius,towerPrefab.GetComponent<Default>().attackRadius);
            }
        }
    }
    public void UpLevelAnim(Transform transform)
    {
        Instantiate(newLvl, transform.position, Quaternion.identity);
    }
    public void Clicking(Vector3 vec3, Vector3Int vec3Int, int indexOfSlot)
    {   

        Vector3 cellCenterPosition = tilemap.GetCellCenterWorld(vec3Int);
        Vector3 spawnPosition = new Vector3(cellCenterPosition.x, cellCenterPosition.y + 0.2f, cellCenterPosition.z);
        int columnIndex = vec3Int.x;
        int rowIndex = vec3Int.y;
        if (towerPrefab != null && allBases[columnIndex + 10, rowIndex + 3] == null)
        {
            if (GameManager.Instance.Gold >= costTower)
            {
                //GameManager.Instance.Gold -= costTower;
                for(int i =0; i < 5;i ++)
                {
                    GameManager.Instance.gameObject.GetComponent<GameController>().OffBut(i);
                }
                GameManager.Instance.Gold -= costTower;
                GameManager.Instance.ChangeMoney();
                if (towerPrefab.GetComponent<UpHave>().id == 26)
                {
                    GameManager.Instance.gameObject.GetComponent<SunMoonScript>().moonCount++;
                }
                else if (towerPrefab.GetComponent<UpHave>().id == 27)
                {
                    GameManager.Instance.gameObject.GetComponent<SunMoonScript>().sunCount++;
                }
                GameObject newGM = Instantiate(towerPrefab, spawnPosition = new Vector3(spawnPosition.x, spawnPosition.y - 0.2f, spawnPosition.z - (rowIndex + 3) * (-0.01f)), Quaternion.identity);
                GameObject towerBase = Instantiate(baseOfTower, spawnPosition = new Vector3(spawnPosition.x, spawnPosition.y, spawnPosition.z), Quaternion.identity);
                towerPrefab = null;

                // Create a new instance of TowerBase and set its position

                towerBase.GetComponent<TowerBase>().curGM = newGM;
                towerBase.GetComponent<TowerBase>().rollBase = this;
                Debug.Log("Column Index: " + columnIndex);
                Debug.Log("Row Index: " + rowIndex);
                allBases[columnIndex + 10, rowIndex + 3] = towerBase.GetComponent<TowerBase>();
                AddTower(towerBase.GetComponent<TowerBase>().curGM.GetComponent<SpriteRenderer>());
                towerBase.GetComponent<TowerBase>().curGM.GetComponent<UpHave>().baseOf = towerBase.GetComponent<TowerBase>();
                towerBase.GetComponent<TowerBase>().monster = newGM;
                GameManager.Instance.ChangeMoney();
                choosing = false;
                towerPrefab = null;

                butChoose[curIndex].interactable = false;


                UnChoose();
            }
            else
            {
                GameManager.Instance.notEnought();
            }
        }
        else if (allBases[columnIndex + 10, rowIndex + 3] != null)
        {
             for(int i =0; i < 5;i ++)
                {
                    GameManager.Instance.gameObject.GetComponent<GameController>().OffBut(i);
                }
            allBases[columnIndex + 10, rowIndex + 3].OnMouseUpping();
            sell.text = (100 * Math.Pow(2, allBases[columnIndex + 10, rowIndex + 3].curGM.GetComponent<UpHave>().LVL)).ToString("");
            sell.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            UnChoose();
        }
    }
    public void RollingThis()
    {
        if (GameManager.Instance.Gold >= 300)
        {
            //GameManager.Instance.Gold -= costTower;
            GameManager.Instance.Gold -= 300;
            GameManager.Instance.ChangeMoney();
            //costTower += 10;
            Roll();
        }
        else
        {
            GameManager.Instance.notEnought();
        }
    }
    public void Roll()
    {
        GameManager.Instance.ChangeMoney();
        pressSpace.SetActive(false);
        choosing = false;
        bool[] lockerTower = new bool[towers.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            butChoose[i].interactable = true;
            butChoose[i].gameObject.SetActive(true);
            int randomId = UnityEngine.Random.Range(0, towers.Length);
            while (lockerTower[randomId])
            {
                randomId = UnityEngine.Random.Range(0, towers.Length);
            }

            lockerTower[randomId] = true;
            costOfTowerText[i].text = costTower.ToString("");
            slots[i].id = randomId;
            slots[i].icon.sprite = imageidTower[slots[i].id];
            slots[i].tower = towers[randomId];
            if (slots[i].tower != null)
            {
                nameOfTowerText[i].text = slots[i].tower.GetComponent<UpHave>().name;
            }

        }
    }

    public void Choose(int i)
    {
        GameManager.Instance.DownLay();
        if (!choosing && slots[i] != null)
        {
            curIndex = i;
            info.SetActive(true);
            towerPrefab = slots[i].tower;
            UpHave uh = towerPrefab.GetComponent<UpHave>();
            sell.text = "Sell: " + (int)Math.Pow(100, uh.LVL+1);
            towerInfo[0].text  = "" + uh.name;
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

                towerInfo[1].text = string.Format(uh.description, coloredFire, 
                coloredPoison,coloredIce,coloredStan, replacementValueTarget);
            }
            if(uh.discInfo != null)
            {
                var replacementValueDEF = uh.towerDataCur.lvlData[uh.LVL, 1];
                var replacementValueGB = uh.towerDataCur.lvlData[uh.LVL, 1] +uh.towerDataCur.lvlData[uh.LVL, 1]* (GameManager.Instance.buff[0]/100); // global
                var replacementValueMB = uh.towerDataCur.lvlData[uh.LVL, 1] + uh.towerDataCur.lvlData[uh.LVL, 1] * (GameManager.Instance.buff[4]/100); //money
                towerInfo[2].text = string.Format(uh.discInfo, replacementValueGB, replacementValueMB.ToString("0"),replacementValueDEF );
            }
            towerInfo[3].text = "LVL:" + (uh.LVL + 1);
            OrderUp();
        }
    }

    public void UnChoose()
    {
        info.SetActive(false);
        sell.transform.parent.gameObject.SetActive(false);
        choosing = false;
        towerPrefab = null;
        OrderDown();
    }
    public void AddTower(SpriteRenderer SR)
    {

        SpriteRenderer[] newArray = new SpriteRenderer[GameM.allTower.Length + 1];

        // Copy elements from the old array to the new one
        for (int i = 0; i < GameM.allTower.Length; i++)
        {
            newArray[i] = GameM.allTower[i];
        }

        // Assign the new array to the original array variable
        GameM.allTower = newArray;
        GameM.allTower[GameM.allTower.Length - 1] = SR;
    }
    public void OrderUp()
    {
        GameM.UpLay();
        Debug.Log("UP");
    }
    public void OrderDown()
    {
        GameM.DownLay();
        Debug.Log("down");
    }
    public IEnumerator Un()
    {
        yield return new WaitForSeconds(0.25f);

    }
}
[System.Serializable]
public class Slot 
{
    public int id;
    public GameObject tower;
    public Image icon;
}