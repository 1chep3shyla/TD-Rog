using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscGameObject : MonoBehaviour
{
    public GameObject objectToEnable;   // Объект, который будет включаться
    public GameObject objectToDisable;  // Объект, который будет выключаться

    void Update()
    {
        // Проверка нажатия клавиши ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Включение первого объекта и выключение второго
            objectToEnable.SetActive(true);
            objectToDisable.SetActive(false);
        }
    }
}