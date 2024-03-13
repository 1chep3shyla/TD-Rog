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
    public TowerBase[,] allBases = new TowerBase[20, 7];
    public GameObject[] towers;
    public Button[] butChoose;
    public Sprite[] imageidTower;
    public GameObject towerPrefab;
    public GameObject unPanel;
    public GameManager GameM;
    public GameObject baseOfTower;
    public Tilemap tilemap;
    public TMP_Text[] nameOfTowerText;
    public TMP_Text costTowerText;
    public GameObject pressSpace;
    public GameObject cursor;
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
        //costTowerText.text = costTower.ToString("");
        costTowerText.text = "300";
        HandleMouseInput();
        HandleSpaceKey();
        UpdateCursorVisibility();
    }
    public void Sell()
    {
        if (towerPrefab != null && choosing)
        {
            UpHave uh = towerPrefab.GetComponent<UpHave>();
            GameManager.Instance.Gold += (int)Math.Pow(100, uh.LVL+1);
            Destroy(uh.baseOf.gameObject);
            Destroy(towerPrefab);
            GameManager.Instance.ChangeMoney();
            UnChoose();
        }
    }
    private void HandleMouseInput()
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
        cursor.SetActive(choosing && towerPrefab != null);
        if (choosing && towerPrefab != null)
        {
            cursor.transform.position = new Vector2(towerPrefab.transform.position.x, towerPrefab.transform.position.y +0.05f);
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
                GameManager.Instance.Gold -= 150;
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
        }
        else if (allBases[columnIndex + 10, rowIndex + 3] != null)
        {
            allBases[columnIndex + 10, rowIndex + 3].OnMouseUpping();
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
        if (!choosing && slots[i] != null)
        {
            curIndex = i;
            info.SetActive(true);
            towerPrefab = slots[i].tower;
            UpHave uh = towerPrefab.GetComponent<UpHave>();
            sell.text = "Sell:" + (int)Math.Pow(100, uh.LVL+1);
            towerInfo[0].text  = "" + uh.name;
            towerInfo[1].text = uh.description;
            towerInfo[2].text = "Damage:" + uh.towerDataCur.lvlData[uh.LVL, 1];
            towerInfo[3].text = "LVL:" + (uh.LVL + 1);
        }
    }

    public void UnChoose()
    {
        info.SetActive(false);
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