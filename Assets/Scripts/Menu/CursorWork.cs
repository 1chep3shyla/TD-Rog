using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CursorWork : MonoBehaviour
{
    public Button[] buttons; // ������ ������
    public Transform cursor;

    private void Start()
    {
        // ��������� ���������� ������� ��� ������ ������
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // ���������� ������� ��������� ���������� ��� ���������� �������� ������� ������ �����������
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