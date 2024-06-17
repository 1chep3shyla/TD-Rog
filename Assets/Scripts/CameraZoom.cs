using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
   public float scaleSpeed = 0.5f;          // Скорость изменения масштаба
    public float minScale = 0.5f;            // Минимальный масштаб
    public float maxScale = 2.0f;            // Максимальный масштаб
    public float originalScale = 1.0f;       // Исходный масштаб

    private Vector3 originalScaleVector;     // Вектор для хранения исходного масштаба

    void Awake()
    {
        originalScaleVector = transform.localScale;
    }

    void Update()
    {
        // Получаем входные данные от колесика мыши
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Изменяем масштаб объекта на основе входных данных
        if (scrollInput != 0)
        {
            float newScale = Mathf.Clamp(transform.localScale.x + scrollInput * scaleSpeed, minScale, maxScale);
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }

    }
    public void ResetScale()
    {
        transform.localScale = originalScaleVector * originalScale;
    }
}