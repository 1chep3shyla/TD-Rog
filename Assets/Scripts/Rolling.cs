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
    public float[] chanceTower;
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
    public int rollCoster;
    public bool choosing = false;
    public bool draging;
    public SpriteRenderer cursorTower;
    public SpriteRenderer cursorTowerPanel;
    public BoxCollider2D colliderSell;
    public Color[] colors;
    public Vector3 offsetingTileSet;




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
        for(int i = 0; i < butChoose.Length;i++)
        {
            if(!butChoose[i].interactable)
            {
                Color color = new Color(53 / 255f, 53 / 255f, 53 / 255f);
                butChoose[i].GetComponent<Image>().color = color;
            }
        }
        costTowerText.text = rollCoster.ToString("");
        rollCoster = (int)(300f - (300f * GameManager.Instance.secondsBuff[10]/100));
        //costTowerText.text = "300";
        HandleMouseInput();
        HandleSpaceKey();
        UpdateCursorVisibility();
       if (cursorTower.gameObject.activeSelf)
    {
        UpHave uh = towerPrefab.GetComponent<UpHave>();
        int[] towerCosts = { 150, 900, 1800, 3600, 7200 };
        costTower = towerCosts[uh.LVL];

        if (GameManager.Instance.Gold < costTower)
        {
            cursorTower.color = new Color(75/255f, 0, 0); // Значения цвета должны быть в диапазоне от 0 до 1
            cursorTowerPanel.color = new Color(75/255f, 0, 0);
        }
        else
        {
            cursorTower.color = new Color(1, 1, 1);
            cursorTowerPanel.color = colors[uh.LVL];
        }

        Vector3 mouseScreenPosition = Input.mousePosition; // Получаем текущую позицию мыши на экране
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition); // Преобразуем её в мировые координаты
        mouseWorldPosition.z = 0; // Убедимся, что Z координата установлена в 0 (для 2D игры)

        // Добавляем оффсет к позиции мыши
        Vector3 offset = new Vector3(0.5f, 0.5f, 0); // Пример оффсета, который сдвигает позицию вправо и вверх на 0.5 единицы
        mouseWorldPosition += offsetingTileSet;

        Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPosition); // Преобразуем мировые координаты в координаты ячейки
        int columnIndex = cellPosition.x + 10;
        int rowIndex = cellPosition.y + 3;

        // Проверяем, если клик происходит по области с тайлами
        if (tilemap.HasTile(cellPosition) && (allBases[columnIndex, rowIndex] == null ||
            (allBases[columnIndex, rowIndex].curGM.GetComponent<UpHave>().id == towerPrefab.GetComponent<UpHave>().id &&
            allBases[columnIndex, rowIndex].curGM.GetComponent<UpHave>().LVL == towerPrefab.GetComponent<UpHave>().LVL)))
        {
            Vector3 cellCenterWorldPosition = tilemap.GetCellCenterWorld(cellPosition); // Получаем мировые координаты центра клетки
            cursorTower.transform.position = cellCenterWorldPosition; // Обновляем позицию курсора
        }
    }
        if(towerPrefab == null)
        {
            cursorTower.gameObject.SetActive(false);
        }
    }
    public void UpdateInfoBut()
    {
        for(int i = 0; i < butChoose.Length;i++)
        {
            if(!butChoose[i].interactable)
            {
                int[] towerCosts = { 150, 900, 1800, 3600, 7200 };
                Color color = new Color(53 / 255f, 53 / 255f, 53 / 255f);
                butChoose[i].GetComponent<Image>().color = color;
                costOfTowerText[i].text = towerCosts[slots[i].tower.GetComponent<UpHave>().LVL].ToString();
            }
            else
            {
                int[] towerCosts = { 150, 900, 1800, 3600, 7200 };
                butChoose[i].GetComponent<Image>().color = colors[slots[i].tower.GetComponent<UpHave>().LVL]; 
                costOfTowerText[i].text = towerCosts[slots[i].tower.GetComponent<UpHave>().LVL].ToString();
            }
        }
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
    if (!perk.activeSelf)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(mousePosition);
            int columnIndex = cellPosition.x + 10;
            int rowIndex = cellPosition.y + 3;

            // Проверяем, если клик происходит по области с тайлами
            if (tilemap.HasTile(cellPosition))
            {
                if (choosing)
                {
                    HandleChoosingState(mousePosition, cellPosition, columnIndex, rowIndex);
                }
                else
                {
                    HandleDefaultState(mousePosition, cellPosition, columnIndex, rowIndex);
                }
            }
            else if(!colliderSell.OverlapPoint(mousePosition))
            {
                UnChoose();
            }
            cursorTower.gameObject.SetActive(false);
        }
        if (Input.GetMouseButtonDown(1))
        {
            UnChoose();
        }
        if (Input.GetMouseButtonUp(0) && choosing)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(mousePosition);
            int columnIndex = cellPosition.x + 10;
            int rowIndex = cellPosition.y + 3;

            if (tilemap.HasTile(cellPosition))
            {
                TowerBase baseTower = allBases[columnIndex, rowIndex];
                if (baseTower != null)
                {
                    baseTower.OnMouseUpping();
                }
            }
        }
    }
}

private void HandleChoosingState(Vector3 mousePosition, Vector3Int cellPosition, int columnIndex, int rowIndex)
{
    if (allBases[columnIndex, rowIndex] == null)
    {
        UnChoose();
    }
    else
    {
        allBases[columnIndex, rowIndex].OnMouseUpping();
        allBases[columnIndex, rowIndex].SetInfo();
        cursorTower.gameObject.SetActive(false);
    }
}

private void HandleDefaultState(Vector3 mousePosition, Vector3Int cellPosition, int columnIndex, int rowIndex)
{
    if (allBases[columnIndex, rowIndex] == null)
    {
        Clicking(mousePosition, cellPosition, curIndex);
        if(allBases[columnIndex, rowIndex]!=null)
        {
            allBases[columnIndex, rowIndex].SetInfo();
        }
    }
    else
    {
        allBases[columnIndex, rowIndex].OnMouseUpping();
        allBases[columnIndex, rowIndex].SetInfo();
        sell.text = (100 * Math.Pow(2, allBases[columnIndex, rowIndex].curGM.GetComponent<UpHave>().LVL)).ToString("");
        sell.transform.parent.gameObject.SetActive(true);
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
        if(!cursorTower.gameObject.activeSelf)
        {
            cursor.SetActive(choosing && towerPrefab != null) ;
            radiusAttack.SetActive((choosing && towerPrefab != null)&& (towerPrefab.GetComponent<Default>() != null || 
            towerPrefab.GetComponent<AttackBuff>() != null || 
            towerPrefab.GetComponent<SpeedBuff>() != null));
            if (choosing && towerPrefab != null)
            {
                cursor.transform.position = new Vector2(towerPrefab.transform.position.x, towerPrefab.transform.position.y +0.05f);
                radiusAttack.transform.position = new Vector2(towerPrefab.transform.position.x, towerPrefab.transform.position.y );
                if(towerPrefab.GetComponent<Default>() != null)
                {
                    UpHave def = towerPrefab.GetComponent<UpHave>();
                    radiusAttack.transform.localScale = new Vector2(def.towerDataCur.lvlData[def.LVL, 2] + 
                    (def.towerDataCur.lvlData[def.LVL, 2] *GameManager.Instance.secondsBuff[12]/100),
                    def.towerDataCur.lvlData[def.LVL, 2] +
                    (def.towerDataCur.lvlData[def.LVL, 2] *GameManager.Instance.secondsBuff[12]/100)); 
                }
                else if(towerPrefab.GetComponent<AttackBuff>() != null)
                {
                    radiusAttack.transform.localScale = new Vector2(towerPrefab.GetComponent<AttackBuff>().boostRadius,towerPrefab.GetComponent<AttackBuff>().boostRadius);
                }
                else if(towerPrefab.GetComponent<SpeedBuff>() != null)
                {
                    radiusAttack.transform.localScale = new Vector2(towerPrefab.GetComponent<SpeedBuff>().boostRadius,towerPrefab.GetComponent<SpeedBuff>().boostRadius);
                }
            }
        }
        else
        {
            if(towerPrefab == null)
            {
                cursorTower.gameObject.SetActive(false);
            }
            else
            {
                radiusAttack.SetActive(towerPrefab.GetComponent<Default>() != null || 
                towerPrefab.GetComponent<AttackBuff>() != null || 
                towerPrefab.GetComponent<SpeedBuff>() != null);
                if (towerPrefab != null)
                {
                    radiusAttack.transform.position = new Vector2(cursorTower.transform.position.x, cursorTower.transform.position.y );
                    if(towerPrefab.GetComponent<Default>() != null)
                    {
                        UpHave def = towerPrefab.GetComponent<UpHave>();
                        radiusAttack.transform.localScale = new Vector2(def.towerDataCur.lvlData[def.LVL, 2] + 
                    (def.towerDataCur.lvlData[def.LVL, 2] *GameManager.Instance.secondsBuff[12]/100),
                    def.towerDataCur.lvlData[def.LVL, 2] +
                    (def.towerDataCur.lvlData[def.LVL, 2] *GameManager.Instance.secondsBuff[12]/100));                
                    }
                    else if(towerPrefab.GetComponent<AttackBuff>() != null)
                    {
                        radiusAttack.transform.localScale = new Vector2(towerPrefab.GetComponent<AttackBuff>().boostRadius,towerPrefab.GetComponent<AttackBuff>().boostRadius);
                    }
                    else if(towerPrefab.GetComponent<SpeedBuff>() != null)
                    {
                        radiusAttack.transform.localScale = new Vector2(towerPrefab.GetComponent<SpeedBuff>().boostRadius,towerPrefab.GetComponent<SpeedBuff>().boostRadius);
                    }
                }
            }
        }
    }
    public void UpLevelAnim(Transform transform)
    {
        Instantiate(newLvl, transform.position, Quaternion.identity);
    }
    private void Clicking(Vector3 vec3, Vector3Int vec3Int, int indexOfSlot)
    {
        Vector3 cellCenterPosition = tilemap.GetCellCenterWorld(vec3Int);
        Vector3 spawnPosition = new Vector3(cellCenterPosition.x, cellCenterPosition.y + 0.2f, cellCenterPosition.z);
        int columnIndex = vec3Int.x + 10;
        int rowIndex = vec3Int.y + 3;

        if (towerPrefab != null && allBases[columnIndex, rowIndex] == null)
        {
            int[] towerCosts = { 150, 900, 1800, 3600, 7200 };
            costTower = towerCosts[towerPrefab.GetComponent<UpHave>().LVL];

            if (GameManager.Instance.Gold >= costTower)
            {
                for (int i = 0; i < 5; i++)
                {
                    GameManager.Instance.gameObject.GetComponent<GameController>().OffBut(i);
                }

                GameBack.Instance.towerSet++;
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

                GameObject newGM = Instantiate(towerPrefab, spawnPosition = new Vector3(spawnPosition.x, spawnPosition.y - 0.2f, spawnPosition.z - (rowIndex) * (-0.01f)), Quaternion.identity);
                GameObject towerBase = Instantiate(baseOfTower, spawnPosition, Quaternion.identity);

                towerBase.GetComponent<TowerBase>().curGM = newGM;
                towerBase.GetComponent<TowerBase>().rollBase = this;

                allBases[columnIndex, rowIndex] = towerBase.GetComponent<TowerBase>();
                AddTower(towerBase.GetComponent<TowerBase>().curGM.GetComponent<SpriteRenderer>());
                towerBase.GetComponent<TowerBase>().curGM.GetComponent<UpHave>().baseOf = towerBase.GetComponent<TowerBase>();
                towerBase.GetComponent<TowerBase>().monster = newGM;

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
        else if (allBases[columnIndex, rowIndex] != null)
        {
            for (int i = 0; i < 5; i++)
            {
                GameManager.Instance.gameObject.GetComponent<GameController>().OffBut(i);
            }
            allBases[columnIndex, rowIndex].OnMouseUpping();
            sell.text = (100 * Math.Pow(2, allBases[columnIndex, rowIndex].curGM.GetComponent<UpHave>().LVL)).ToString("");
            sell.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            UnChoose();
        }
    }
    public void RollingThis()
    {
        if (GameManager.Instance.Gold >= rollCoster)
        {
            //GameManager.Instance.Gold -= costTower;
            GameManager.Instance.Gold -= rollCoster;
            GameManager.Instance.ChangeMoney();
            //costTower += 10;
        //GameManager.Instance.cardAnim.Play("card_animation", -1, 0f);
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
    
    int[] towerCosts = { 150, 900, 1800, 3600, 7200 };
    float[] weights = new float[towerCosts.Length];

    // Determine maximum available level based on gold amount
    int maxLevel;

    if (GameManager.Instance.Gold >= 6000)
    {
        maxLevel = 5;
    }
    else if (GameManager.Instance.Gold >= 3500)
    {
        maxLevel = 4;
    }
    else if (GameManager.Instance.Gold >= 1700)
    {
        maxLevel = 3;
    }
    else if (GameManager.Instance.Gold >= 700)
    {
        maxLevel = 2;
    }
    else 
    {
        maxLevel = 1; // Minimum level 1 if gold is less than 700
    }

    // Initialize available levels array
    int[] availableLevels = new int[maxLevel];
    for (int i = 0; i < maxLevel; i++)
    {
        availableLevels[i] = i;
    }

    // Calculate weights for available levels and determine total weight
    float totalWeight = 0;
    for (int i = 0; i < maxLevel; i++)
    {
        weights[i] = Mathf.Log(GameManager.Instance.Gold / towerCosts[i] + 1);
        totalWeight += weights[i];
    }

    // Display chances for each tower
    for (int i = 0; i < weights.Length; i++)
    {
        float chance = weights[i] / totalWeight * 100f; // Calculate percentage chance
        Debug.Log($"Chance for Tower {i + 1}: {chance:F2}%");
        // You can display this chance in your UI or store it as needed
    }

    // Normalize weights to probabilities
    float[] probabilities = new float[maxLevel];
    for (int i = 0; i < maxLevel; i++)
    {
        probabilities[i] = weights[i] / totalWeight;
    }

    bool[] usedTowers = new bool[towers.Length];

    for (int i = 0; i < slots.Length; i++)
    {
        butChoose[i].interactable = true;
        butChoose[i].gameObject.SetActive(true);

        // Select a tower randomly
        int randomId = UnityEngine.Random.Range(0, towers.Length);
        while (usedTowers[randomId])
        {
            randomId = UnityEngine.Random.Range(0, towers.Length);
        }
        usedTowers[randomId] = true;

        // Select level based on probabilities
        int randomLevel = GetRandomLevel(probabilities);

        // Assign tower and level to slot
        slots[i].id = randomId;
        slots[i].icon.sprite = imageidTower[randomId];
        slots[i].tower = towers[randomId];
        slots[i].tower.GetComponent<UpHave>().LVL = availableLevels[randomLevel]; // Assign the actual level
        butChoose[i].GetComponent<Image>().color = colors[randomLevel]; 
        costOfTowerText[i].text = towerCosts[availableLevels[randomLevel]].ToString();

        if (slots[i].tower != null)
        {
            nameOfTowerText[i].text = slots[i].tower.GetComponent<UpHave>().name;
        }
    }
}

    private int GetRandomLevel(float[] probabilities)
    {
        float randomPoint = UnityEngine.Random.value;
        float cumulative = 0.0f;

        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulative += probabilities[i];
            if (randomPoint < cumulative)
            {
                return i;
            }
        }
        return probabilities.Length - 1; // This should ideally not happen if probabilities sum to 1.
    }
    public void Choose(int i)
    {
        GameManager.Instance.DownLay();
        if (!choosing && slots[i] != null)
        {
            curIndex = i;
            GameManager.Instance.PlaySFX(GameManager.Instance.clickTowerSFX);
            info.SetActive(true);
            towerPrefab = slots[i].tower;
            UpHave uh = towerPrefab.GetComponent<UpHave>();
            cursorTower.gameObject.SetActive(true);
            cursorTower.sprite = towerPrefab.GetComponent<SpriteRenderer>().sprite;
            cursorTowerPanel.color = colors[uh.LVL];
            sell.text = "Sell: " + (int)Math.Pow(100, uh.LVL+1);

                var replacementValueDEF = uh.towerDataCur.lvlData[uh.LVL, 1]; //0 - fire, 1 - poison, 2 -ice, 3 - stan, 4 - targets, 5 - thiefChance
                var replacementValueIce = uh.towerDataCur.lvlData[uh.LVL, 5] + (uh.towerDataCur.lvlData[uh.LVL, 5]* (GameManager.Instance.buff[1]/100));
                var replacementValueFire = uh.towerDataCur.lvlData[uh.LVL, 6] + (uh.towerDataCur.lvlData[uh.LVL, 6]* (GameManager.Instance.buff[2]/100));
                var replacementValuePoison = uh.towerDataCur.lvlData[uh.LVL, 7] + (uh.towerDataCur.lvlData[uh.LVL, 7]* (GameManager.Instance.buff[3]/100));
                var replacementValueThief = uh.towerDataCur.lvlData[uh.LVL, 11] + uh.towerDataCur.lvlData[uh.LVL, 11] * (GameManager.Instance.buff[6]/100); 
                var replacementValueStan = uh.towerDataCur.lvlData[uh.LVL, 8];
                var replacementValueTarget = uh.towerDataCur.lvlData[uh.LVL, 10];
                string coloredFire = $"<color=#FF0000>{replacementValueFire.ToString("0.00")}</color>"; // Красный
                string coloredPoison = $"<color=#00FF00>{replacementValuePoison.ToString("0.00")}</color>"; // Зеленый
                string coloredIce = $"<color=#00FFFF>{replacementValueIce.ToString("0.00")}</color>"; // Голубой
                string coloredStan = $"<color=#333333>{replacementValueStan.ToString("0.00")}</color>"; // Серый
                string coloredThief =  $"<color=#FFD700>{replacementValueThief.ToString("0.00")}</color>";

                //towerInfo[1].text = string.Format(uh.description, coloredFire, 
                //coloredPoison,coloredIce,coloredStan, replacementValueTarget);
            

                //var replacementValueDEF = uh.towerDataCur.lvlData[uh.LVL, 1];
                var replacementValueGB = uh.towerDataCur.lvlData[uh.LVL, 1] +uh.towerDataCur.lvlData[uh.LVL, 1]* (GameManager.Instance.buff[0]/100); // global
                var replacementValueMB = uh.towerDataCur.lvlData[uh.LVL, 1] + uh.towerDataCur.lvlData[uh.LVL, 1] * (GameManager.Instance.buff[4]/100); //money
                //var replacementValueAS = 1 / (uh.towerDataCur.lvlData[uh.LVL, 3] + (uh.towerDataCur.lvlData[uh.LVL, 3]* (GameManager.Instance.buff[5]/100))); //as
                 var replacementValueAS =  uh.towerDataCur.lvlData[uh.LVL, 3] /(1 + GameManager.Instance.buff[5] / 100);
            TMP_Text[] texts = new TMP_Text[4];
            texts[0] = towerInfo[0];
            texts[1] = towerInfo[1];
            texts[2] = towerInfo[2];
            texts[3] = towerInfo[3];
            string[] textsStrings = new string[4];
            textsStrings[0] = "" + uh.name;
            textsStrings[1] = string.Format(uh.description, coloredFire, 
            coloredPoison,coloredIce,coloredStan, replacementValueTarget, coloredThief);
            textsStrings[2] = string.Format(uh.discInfo, replacementValueGB.ToString("0.0"), replacementValueMB.ToString("0"),replacementValueDEF,replacementValueAS.ToString("0.00"));
            textsStrings[3] ="LVL:" + (uh.LVL + 1);
            GameManager.Instance.StartTypingText(texts, textsStrings, 0f);
            OrderUp();
        }
    }

    public void UnChoose()
    {
        info.SetActive(false);
        cursorTower.gameObject.SetActive(false);
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