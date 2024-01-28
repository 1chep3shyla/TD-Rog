using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromToTween : MonoBehaviour
{
  public Transform targetPosition; // Позиция, к которой нужно переместить объект
    public float moveDuration = 2.0f; // Длительность движения
    public iTween.EaseType easeType = iTween.EaseType.easeOutExpo; // Тип движения

    void Start()
    {
        // Вызов функции для перемещения объекта
        MoveObject();
    }

    private void MoveObject()
    {
        // Параметры движения для iTween
        Hashtable moveParams = new Hashtable();
        moveParams.Add("position", targetPosition.position);
        moveParams.Add("time", moveDuration);
        moveParams.Add("easetype", easeType);
        moveParams.Add("oncomplete", "OnMoveComplete");
        moveParams.Add("oncompletetarget", gameObject);

        // Вызов iTween для перемещения объекта
        iTween.MoveTo(gameObject, moveParams);
    }

    // Функция, вызываемая по завершению движения
    private void OnMoveComplete()
    {
        Debug.Log("Движение завершено!");
    }
}