using System;
using DG.Tweening;
using UnityEngine;

public class RotateObjectWithMouse : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float rotationSpeedMobile = 1f;
    private Vector2 touchDeltaPosition;

    private void Start()
    { 
        // transform.DORotate(new Vector3(45, 45, 0), 0.5f);
        Rotate();
        EventManager.Instance.RotateObjectWithMouse = this;
    }

    public void Rotate()
    {
        transform.DORotate(new Vector3(45, 45, 0), 0.5f);
    }
    private void Update()
    {
        if(Time.timeScale==0f) return;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            // Вычисляем углы вращения по осям
            float rotationX = touchDeltaPosition.y*rotationSpeedMobile;
            float rotationY = -touchDeltaPosition.x*rotationSpeedMobile; // обратный знак для инверсии направления вращения

            // Rotate the object based on the camera's orientation
            Vector3 camUp = Camera.main.transform.up;
            Vector3 camRight = Camera.main.transform.right;

            Vector3 rotation = rotationX * camRight + rotationY * camUp;
            transform.Rotate(rotation, Space.World);
        }
    }

    private void OnMouseDrag()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Вычисляем углы вращения по осям
        float rotationX = mouseY * rotationSpeed;
        float rotationY = -mouseX * rotationSpeed; // обратный знак для инверсии направления вращения

        // Rotate the object based on the camera's orientation
        Vector3 camUp = Camera.main.transform.up;
        Vector3 camRight = Camera.main.transform.right;

        Vector3 rotation = rotationX * camRight + rotationY * camUp;
        transform.Rotate(rotation, Space.World);
    }
}