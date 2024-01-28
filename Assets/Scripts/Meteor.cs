using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
   public float destroyDelay = 3.0f; // Время до уничтожения текущего объекта
    public GameObject replacementObject; // Объект, который будет создан вместо текущего

    void Start()
    {
        // Вызов функции для уничтожения объекта и создания нового
        DestroyAndCreateObject();
    }

    private void DestroyAndCreateObject()
    {
        // Запуск таймера для уничтожения объекта через заданное время
        Invoke("DestroyCurrentObject", destroyDelay);
    }

    private void DestroyCurrentObject()
    {
        // Уничтожение текущего объекта
        Destroy(gameObject);

        // Проверка, есть ли объект для создания вместо текущего
        if (replacementObject != null)
        {
            // Создание нового объекта вместо уничтоженного
            Instantiate(replacementObject, transform.position, transform.rotation);
        }
    }
}