using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMoving : MonoBehaviour
{
    public Transform targetObject; // ������� ������, � �������� ����� �������������
    public float moveDuration = 2.0f; // ������������ �����������
    public iTween.EaseType easeType = iTween.EaseType.easeOutExpo; // ��� ������
    public int gold;

    private bool isMoving = false; // ����, �����������, ��������� �� ������

    void Start()
    {
        targetObject = GameManager.Instance.goldPos;
    }
    private void Update()
    {
        if (targetObject != null)
        {
            MoveToTarget();
        }

    }

    private void MoveToTarget()
    {
        isMoving = true;

        // ���������� ��������� ��� ����������� iTween
        Hashtable moveParams = new Hashtable();
        moveParams.Add("position", targetObject.position);
        moveParams.Add("time", moveDuration);
        moveParams.Add("easetype", easeType);
        moveParams.Add("oncomplete", "OnMoveComplete");
        moveParams.Add("oncompletetarget", gameObject);

        // ��������� iTween
        iTween.MoveTo(gameObject, moveParams);
    }

    private void OnMoveComplete()
    {
        isMoving = false;
        Debug.Log("������ � ������� �������!");
        GameManager.Instance.Gold += gold;
        Destroy(gameObject);
    }
}