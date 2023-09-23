using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CursorWork : MonoBehaviour
{
    public Button[] buttons; // Массив кнопок
    public Transform cursor;

    private void Start()
    {
        // Добавляем обработчик события для каждой кнопки
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Необходимо создать локальную переменную для правильной передачи индекса внутрь обработчика
            buttons[i].onClick.AddListener(() => MoveObjectToButton(index));
        }
    }

    private void MoveObjectToButton(int buttonIndex)
    {
        // Находим выбранную кнопку
        transform.gameObject.SetActive(true);
        Button selectedButton = buttons[buttonIndex];

        // Делаем объект дочерним по отношению к кнопке
        cursor.SetParent(selectedButton.transform);

        // Устанавливаем позицию объекта в (0, 0, 0) относительно родительской кнопки
        cursor.localPosition = Vector3.zero;
    }
}