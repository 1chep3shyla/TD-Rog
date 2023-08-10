using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Rolling : MonoBehaviour
{

    public Slot[] slots;
    public GameObject panelChoose;

    public GameObject[] towers;
    public GameObject[] helpers;
    public GameObject[] building;

    public Sprite[] imageidTower;
    public Sprite[] imageidHelper;
    public Sprite[] imageidBuilding;

    public GameObject towerPrefab;
    public bool choosing;
    public TowerBase TB;
    public bool cant;
    public GameObject unPanel;
    public GameManager GameM;

    public void Roll()
    {
        cant = true;
        panelChoose.SetActive(true);
        bool[] lockerTower = new bool[towers.Length];
        bool[] lockerHelpers = new bool[helpers.Length];
        bool[] lockerBuilding = new bool[building.Length];
        for (int i = 0; i < slots.Length; i++)
        {
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
                }



            }

        }


    }
    public void Choose(int i)
    {
        if (!choosing)
        {

            towerPrefab = slots[i].tower;
            panelChoose.SetActive(false);
            cant = false;
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