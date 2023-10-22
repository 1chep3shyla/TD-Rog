using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
public class MovingUI : MonoBehaviour
{
    public RectTransform targetObject; // ������, ������� �� ����� ����������
    public Vector3 localStartPosition;  // ��������� ��������� �������
    public Vector3 localEndPosition;    // �������� ��������� �������
    public iTween.EaseType easeType = iTween.EaseType.easeOutExpo; // ��� �����������
    public float moveTime = 1.0f; // ����� ����������� � ��������

    void Start()
    {
        Vector3 targetStartPosition = targetObject.localPosition + localStartPosition;
        Vector3 targetEndPosition = targetObject.localPosition + localEndPosition;

        targetObject.localPosition = targetStartPosition; // ������������� ��������� �������

        iTween.MoveTo(targetObject.gameObject, iTween.Hash(
            "position", targetEndPosition,
            "time", moveTime,
            "easetype", easeType,
            "islocal", true, // ������� ����������� ���������
            "oncomplete", "OnMoveComplete" // �������� ����� OnMoveComplete �� ���������� �����������
        ));
    }
}