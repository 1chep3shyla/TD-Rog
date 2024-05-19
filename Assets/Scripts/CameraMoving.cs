using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
  public float moveSpeed = 5f; // Скорость движения камеры
    public float zoomSpeed = 2f; // Скорость зума
    public float maxZoom = 5f; // Максимальный зум
    public float minZoom = 1f; // Минимальный зум

    public Camera mainCamera;
    public Vector3 targetPosition; // Целевая позиция камеры

    void Start()
    {
        mainCamera = Camera.main;
        targetPosition = transform.position;
    }

    void Update()
    {
        // Передвижение камеры с помощью стрелок
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveInput = new Vector3(horizontalInput, verticalInput, 0f);
        
        // Ограничение передвижения камеры за пределы границы экрана
        Vector3 newPosition = transform.position + moveInput * moveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -mainCamera.orthographicSize * mainCamera.aspect, mainCamera.orthographicSize * mainCamera.aspect);
        newPosition.y = Mathf.Clamp(newPosition.y, -mainCamera.orthographicSize, mainCamera.orthographicSize);
        targetPosition = newPosition;

        // Передвижение камеры к точке (0, 0, 0) при увеличении зума
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - scrollInput * zoomSpeed, minZoom, maxZoom);
        if (mainCamera.orthographicSize == maxZoom)
        {
            targetPosition = Vector3.zero;
        }

        // Передвижение камеры курсором
        if (Input.mousePosition.x <= 0 || Input.mousePosition.x >= Screen.width ||
            Input.mousePosition.y <= 0 || Input.mousePosition.y >= Screen.height)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            targetPosition = new Vector3(mousePos.x, mousePos.y, transform.position.z);
        }
    }

    void LateUpdate()
    {
        // Плавное перемещение камеры к целевой позиции
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}