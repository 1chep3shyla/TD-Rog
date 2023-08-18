using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
public class Rolling : MonoBehaviour
{

    public Slot[] slots;
    public TowerBase[,] allBases = new TowerBase[15, 7];

    public GameObject[] towers;
    public GameObject[] helpers;
    public GameObject[] building;
    public Button[] butChoose;

    public Sprite[] imageidTower;
    public Sprite[] imageidHelper;
    public Sprite[] imageidBuilding;

    public GameObject towerPrefab;
    public bool choosing;
    public TowerBase TB;
    public bool cant;
    public GameObject unPanel;
    public GameManager GameM;
    public GameObject baseOfTower;

    public Tilemap tilemap;
    public GameObject objectToInstantiate;
    public TMP_Text[] nameOfTowerText;
    public int costTower;
    public TMP_Text costTowerText;
    void Start()
    {
        Roll();
    }
    void Update()
    {
        costTowerText.text = costTower.ToString("");
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(mousePosition);

            if (tilemap.HasTile(cellPosition) && !choosing)
            {
                Clicking(mousePosition, cellPosition);
            }
        }
    }
    public void Clicking(Vector3 vec3, Vector3Int vec3Int)
    {
        Vector3 cellCenterPosition = tilemap.GetCellCenterWorld(vec3Int);
        Vector3 spawnPosition = new Vector3(cellCenterPosition.x, cellCenterPosition.y + 0.2f, cellCenterPosition.z);
        int columnIndex = vec3Int.x;
        int rowIndex = vec3Int.y;   
        if (towerPrefab != null && allBases[columnIndex + 10, rowIndex + 3] ==null)
        {
            GameObject newGM = Instantiate(towerPrefab, spawnPosition, Quaternion.identity);
            GameObject towerBase = Instantiate(baseOfTower, spawnPosition = new Vector3(spawnPosition.x , spawnPosition.y -0.2f, spawnPosition.z), Quaternion.identity);
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
        }
        else if (allBases[columnIndex + 10, rowIndex + 3] != null && choosing)
        {

        }
    }
    public void RollingThis()
    {
        if (GameManager.Instance.Gold >= costTower)
        {
            GameManager.Instance.Gold -= costTower;
            costTower += 10;
            Roll();
        }
        else
        {
            GameManager.Instance.notEnought();
        }
    }
    public void Roll()
    {

        choosing = false;
        cant = true;
        bool[] lockerTower = new bool[towers.Length];
        bool[] lockerHelpers = new bool[helpers.Length];
        bool[] lockerBuilding = new bool[building.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            butChoose[i].interactable = true;
            if (i < 3)
            {
                int randomId = Random.Range(0, towers.Length);

                while (lockerTower[randomId])
                {
                    randomId = Random.Range(0, towers.Length);
                }

                lockerTower[randomId] = true;
                slots[i].id = randomId;
                slots[i].tower = towers[randomId];
                slots[i].icon.sprite = imageidTower[slots[i].id];
                slots[i].tower = towers[randomId];
                nameOfTowerText[i].text = slots[i].tower.GetComponent<UpHave>().name;
            }
            else if (i >= 3)
            {
                int randomType = Random.Range(0, 3);

                if (randomType == 0)
                {
                    int randomId = Random.Range(0, towers.Length);
                    while (lockerTower[randomId])
                    {
                        randomId = Random.Range(0, towers.Length);
                    }

                    lockerTower[randomId] = true;
                    slots[i].id = randomId;
                    slots[i].icon.sprite = imageidTower[slots[i].id];
                    slots[i].tower = towers[randomId];
                    nameOfTowerText[i].text = slots[i].tower.GetComponent<UpHave>().name;
                }

                else if(randomType == 1) 
                {
                    int randomId = Random.Range(0, helpers.Length);
                    while (lockerHelpers[randomId])
                    {
                        randomId = Random.Range(0, helpers.Length);
                    }

                    lockerHelpers[randomId] = true;
                    slots[i].id = randomId;
                    slots[i].icon.sprite = imageidHelper[slots[i].id];
                    slots[i].tower = helpers[randomId];
                    nameOfTowerText[i].text = slots[i].tower.GetComponent<UpHave>().name;
                }

                else if (randomType == 2)
                {
                    int randomId = Random.Range(0, building.Length);
                    while (lockerHelpers[randomId])
                    {
                        randomId = Random.Range(0, building.Length);
                    }

                    lockerHelpers[randomId] = true;
                    slots[i].id = randomId;
                    slots[i].icon.sprite = imageidBuilding[slots[i].id];
                    slots[i].tower = building[randomId];
                    nameOfTowerText[i].text = slots[i].tower.GetComponent<UpHave>().name;
                }


            }

        }


    }
    public void Choose(int i)
    {
        if (!choosing)
        {

            towerPrefab = slots[i].tower;
            cant = false;
            for (int o = 0; o < slots.Length; o++)
            {
                butChoose[o].interactable = false;
            }

        }
    }

    public void UnChoose()
    {
        choosing = false;
        towerPrefab = null; 
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
    }
    public void OrderDown()
    {
        GameM.DownLay();
    }
}
[System.Serializable]
public class Slot 
{
    public int id;
    public GameObject tower;
    public Image icon;
}