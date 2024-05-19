using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CursorWork : MonoBehaviour
{
    public Button[] buttons; // Массив кнопок
    public Transform cursor;
    public Transform parent; // Родительский объект для поиска кнопок

    private void Start()
    {
        if (parent != null)
        {
            // Если parent не пустой, извлекаем все кнопки из его дочерних объектов
            buttons = parent.GetComponentsInChildren<Button>();
        }

        // Добавляем слушатели на кнопки
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Сохраняем индекс в замыкании
            buttons[i].onClick.AddListener(() => MoveObjectToButton(index));
        }
    }

    public void MoveObjectToButton(int buttonIndex)
    {
        // ������� ��������� ������
        transform.gameObject.SetActive(true);
        Button selectedButton = buttons[buttonIndex];

        // ������ ������ �������� �� ��������� � ������
        cursor.SetParent(selectedButton.transform);

        // ������������� ������� ������� � (0, 0, 0) ������������ ������������ ������
        cursor.localPosition = Vector3.zero;
    }
}