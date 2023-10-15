using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMoving : MonoBehaviour
{
    public Transform targetObject; // Целевой объект, к которому нужно переместиться
    public float moveDuration = 2.0f; // Длительность перемещения
    public iTween.EaseType easeType = iTween.EaseType.easeOutExpo; // Тип кривой
    public int gold;

    private bool isMoving = false; // Флаг, указывающий, двигается ли объект

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

        // Определите параметры для перемещения iTween
        Hashtable moveParams = new Hashtable();
        moveParams.Add("position", targetObject.position);
        moveParams.Add("time", moveDuration);
        moveParams.Add("easetype", easeType);
        moveParams.Add("oncomplete", "OnMoveComplete");
        moveParams.Add("oncompletetarget", gameObject);

        // Запустите iTween
        iTween.MoveTo(gameObject, moveParams);
    }

    private void OnMoveComplete()
    {
        isMoving = false;
        Debug.Log("Прибыл к целевой позиции!");
        GameManager.Instance.Gold += gold;
        Destroy(gameObject);
    }
}