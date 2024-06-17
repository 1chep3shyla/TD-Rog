using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTower : MonoBehaviour
{
    public GameObject objectPrefab; // Префаб объекта с компонентом BatController
    public Transform orbitTarget; // Цель, вокруг которой объекты будут вращаться
    public float orbitRadius = 5f; // Радиус орбиты
    public BatController[] allBats; // Массив всех контроллеров летучих мышей
    private UpHave uh; // Ссылка на компонент UpHave

    private int previousLevel = -1; // Переменная для хранения предыдущего уровня

    void Start()
    {
        uh = GetComponent<UpHave>(); // Получаем компонент UpHave из текущего объекта
        CreateObjects((int)GetComponent<UpHave>().towerDataCur.lvlData[GetComponent<UpHave>().LVL, 4]); // Создаем объекты при запуске
    }

    void Update()
    {
        int currentLevel = uh.LVL; // Получаем текущий уровень из UpHave

        // Проверяем, изменился ли уровень
        if (currentLevel != previousLevel)
        {
            // Удаляем все текущие объекты
            DestroyAllObjects();

            // Обновляем количество объектов на основе нового уровня
            int numberOfObjects = (int)uh.towerDataCur.lvlData[currentLevel, 4];
            CreateObjects(numberOfObjects); // Создаем новые объекты с обновленным количеством

            // Обновляем предыдущий уровень
            previousLevel = currentLevel;
        }

        // Обновляем параметры каждой летучей мыши
        for (int i = 0; i < allBats.Length; i++)
        {
            allBats[i].orbitSpeed = uh.attackSpeed; // Установка скорости вращения
            allBats[i].dmg = uh.curDamage; // Установка урона
        }
    }

    // Метод для создания объектов
    private void CreateObjects(int numberOfObjects)
    {
        allBats = new BatController[numberOfObjects];
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angleIncrement = 360f / numberOfObjects;
            float angle = i * angleIncrement;

            Vector3 offset = Quaternion.Euler(0f, 0f, angle) * Vector3.right * orbitRadius;
            Vector3 spawnPosition = transform.position + offset;

            GameObject newObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity, transform);
            BatController batController = newObject.GetComponent<BatController>();

            if (batController != null)
            {
                allBats[i] = batController;
                batController.SetTarget(orbitTarget); // Устанавливаем цель вращения
                batController.SetOrbitRadius(orbitRadius); // Устанавливаем радиус орбиты
                batController.SetOrbitAngle(angle); // Устанавливаем угол орбиты
            }
        }
    }

    // Метод для удаления всех текущих объектов
    private void DestroyAllObjects()
    {
        if (allBats != null)
        {
            for (int i = 0; i < allBats.Length; i++)
            {
                if (allBats[i] != null)
                {
                    Destroy(allBats[i].gameObject);
                }
            }
        }
    }

    // Отрисовка радиуса орбиты в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, orbitRadius);
    }
}