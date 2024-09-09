using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TowerSpawner : MonoBehaviour
{
     public GameObject parentObject; // The parent object where the new GameObjects will be created
    public GameObject objectToInstantiate; // The GameObject prefab to instantiate
    public GameObject[] instantiatedObjects;
    public GameObject[] sprites;    // Array of GameObjects to assign to the Image component of the children
    public TMP_Text nameText;
    public TMP_Text discText;
    public TMP_Text typeText;
    public Image icon;
    public CursorWork cursorWork;

    void Start()
    {
        CreateNewObjects();
    }

    public void CreateNewObjects()
    {
        instantiatedObjects = new GameObject[sprites.Length];
        cursorWork.buttons = new Button[sprites.Length];

        // Loop through the sprites array and create a new object for each sprite
        for (int i = 0; i < sprites.Length; i++)
        {
            var sprite = sprites[i];
            
            // Instantiate the specified GameObject as a child of the specified parent
            GameObject newObject = Instantiate(objectToInstantiate, parentObject.transform);

            // Add an Image component to the new object (if it doesn't already have one)
            Image imageComponent = newObject.transform.GetChild(1).GetComponent<Image>();
            if (imageComponent == null)
            {
                imageComponent = newObject.AddComponent<Image>();
            }

            // Ensure the sprite GameObject has an Image component
            SpriteRenderer spriteImage = sprite.GetComponent<SpriteRenderer>();
            if (spriteImage != null)
            {
                // Assign the sprite's Image component to the new object's Image component
                imageComponent.sprite = spriteImage.sprite;
            }

            // Add the instantiated object to the array
            instantiatedObjects[i] = newObject;

            // Add a listener to the button component of the instantiated object
            Button buttonComponent = newObject.GetComponent<Button>();
            if (buttonComponent != null)
            {
                int index = i; // Capture the index for the closure
                buttonComponent.onClick.AddListener(() => SetInfoAboutTower(index));
            }
            cursorWork.buttons[i] = newObject.GetComponent<Button>();
        }
        cursorWork.SetMore();

    }

    public void SetInfoAboutTower(int index)
    {
        icon.sprite = sprites[index].GetComponent<SpriteRenderer>().sprite;
    discText.text = sprites[index].GetComponent<UpHave>().description;
    nameText.text = sprites[index].GetComponent<UpHave>().name;
    typeText.text = "Type: Base";

    UpHave uh = sprites[index].GetComponent<UpHave>();

    // Здесь создаем массивы для каждого типа значений, чтобы отобразить их по уровням
    string[] fireValues = new string[5];
    string[] poisonValues = new string[5];
    string[] iceValues = new string[5];
    string[] stanValues = new string[5];
    string[] thiefValues = new string[5];
    string[] targetValues = new string[5];
    string[] dmgValues = new string[5];
    string[] asValues = new string[5];

    for (int i = 0; i < 5; i++)
    {
        fireValues[i] = uh.towerDataCur.lvlData[i, 6].ToString("0.00");
        poisonValues[i] = uh.towerDataCur.lvlData[i, 7].ToString("0.00");
        iceValues[i] = uh.towerDataCur.lvlData[i, 5].ToString("0");
        stanValues[i] = uh.towerDataCur.lvlData[i, 8].ToString("0");
        thiefValues[i] = uh.towerDataCur.lvlData[i, 11].ToString("0");
        targetValues[i] = uh.towerDataCur.lvlData[i, 10].ToString("0");
        dmgValues[i] = uh.towerDataCur.lvlData[i, 1].ToString("0");
        asValues[i] = uh.towerDataCur.lvlData[i, 3].ToString("0.00");
    }

    // Форматируем строки с использованием "/" для отображения значений всех уровней
    string fireFormatted = string.Join("/", fireValues);
    string poisonFormatted = string.Join("/", poisonValues);
    string iceFormatted = string.Join("/", iceValues);
    string stanFormatted = string.Join("/", stanValues);
    string thiefFormatted = string.Join("/", thiefValues);
    string targetFormatted = string.Join("/", targetValues);
    string dmgFormatted = string.Join("/", dmgValues);
    string asFormatted = string.Join("/", asValues);


    // Используем цветное форматирование, как и раньше
    string coloredFire = $"<color=#FF0000>{fireFormatted}</color>"; // Красный
    string coloredPoison = $"<color=#00FF00>{poisonFormatted}</color>"; // Зеленый
    string coloredIce = $"<color=#00FFFF>{iceFormatted}</color>"; // Голубой
    string coloredStan = $"<color=#333333>{stanFormatted}</color>"; // Серый
    string coloredThief = $"<color=#FFD700>{thiefFormatted}</color>"; // Золотой
    string coloredTarget = targetFormatted; // Без цвета

    // Обновляем текст описания, используя цветные строки
    discText.text = string.Format(uh.description, coloredFire, coloredPoison, coloredIce, coloredStan, coloredTarget, coloredThief);
    typeText.text = string.Format(uh.discInfo, dmgFormatted, asFormatted,dmgFormatted,asFormatted);
    // typeText.text = $"<sprite name=\"0_icon\">:{dmgFormatted}\n<sprite name=\"5_icon\">:{asFormatted}";

    
    }
}