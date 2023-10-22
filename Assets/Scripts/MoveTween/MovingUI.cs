using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
public class MovingUI : MonoBehaviour
{
    public RectTransform targetObject; // Объект, который мы будем перемещать
    public Vector3 localStartPosition;  // Начальная локальная позиция
    public Vector3 localEndPosition;    // Конечная локальная позиция
    public iTween.EaseType easeType = iTween.EaseType.easeOutExpo; // Тип перемещения
    public float moveTime = 1.0f; // Время перемещения в секундах

    void Start()
    {
        Vector3 targetStartPosition = targetObject.localPosition + localStartPosition;
        Vector3 targetEndPosition = targetObject.localPosition + localEndPosition;

        targetObject.localPosition = targetStartPosition; // Устанавливаем начальную позицию

        iTween.MoveTo(targetObject.gameObject, iTween.Hash(
            "position", targetEndPosition,
            "time", moveTime,
            "easetype", easeType,
            "islocal", true, // Сделать перемещение локальным
            "oncomplete", "OnMoveComplete" // Вызывать метод OnMoveComplete по завершении перемещения
        ));
    }
}