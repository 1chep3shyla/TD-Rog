using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGameMoving : MonoBehaviour
{
     public float panSpeed = 20f;  // Скорость перемещения камеры
    public Vector2 panLimitMin;   // Минимальные ограничения по x и y для камеры
    public Vector2 panLimitMax;   // Максимальные ограничения по x и y для камеры

    private Vector3 dragOrigin;   // Точка начала перетаскивания

    void Update()
    {
        // Проверяем, нажата ли левая кнопка мыши
        if (Input.GetMouseButtonDown(0))
        {
            // Сохраняем начальную позицию курсора при нажатии
            dragOrigin = Input.mousePosition;
            return;
        }

        // Проверяем, удерживается ли левая кнопка мыши
        if (!Input.GetMouseButton(0)) return;

        // Вычисляем разницу в позиции курсора
        Vector3 difference = Input.mousePosition - dragOrigin;

        // Вычисляем новое положение камеры на основе разницы курсора
        Vector3 newPosition = transform.position - difference * panSpeed * Time.deltaTime;

        // Ограничиваем новое положение камеры заданными пределами
        newPosition.x = Mathf.Clamp(newPosition.x, panLimitMin.x, panLimitMax.x);
        newPosition.y = Mathf.Clamp(newPosition.y, panLimitMin.y, panLimitMax.y);

        // Применяем новое положение к камере
        transform.position = newPosition;

        // Обновляем начальную точку для следующего кадра
        dragOrigin = Input.mousePosition;
    }
}