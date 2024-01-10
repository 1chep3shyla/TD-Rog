using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTween : MonoBehaviour
{
   public float rotationSpeed = 90f; // Скорость вращения (градусы в секунду)

    void Start()
    {
        // Вызываем метод, который начнет вращение объекта
        RotateObjectZ();
    }

    void RotateObjectZ()
    {
        // Используем iTween для вращения объекта по Z-координате
        iTween.RotateBy(gameObject, iTween.Hash(
            "z", 1.0f, // На сколько оборотов вращаться (1.0f = 360 градусов)
            "speed", rotationSpeed,
            "easetype", iTween.EaseType.linear,
            "looptype", iTween.LoopType.loop
        ));
    }
}