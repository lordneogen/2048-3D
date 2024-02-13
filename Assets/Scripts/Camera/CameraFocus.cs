using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public float zoomSpeed = 5f; // Скорость изменения масштаба
    public float minZoom = 30f; // Минимальный масштаб
    public float maxZoom = 90f; // Максимальный масштаб

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.fieldOfView -= scroll * zoomSpeed;

        // Ограничиваем масштаб в заданных пределах
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minZoom, maxZoom);
    }
}
