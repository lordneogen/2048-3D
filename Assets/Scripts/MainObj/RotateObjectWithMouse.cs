using System;
using UnityEngine;

public class RotateObjectWithMouse : MonoBehaviour
{
    public float rotationSpeed = 5f;

    void Update()
    {
        // Your existing Update code (if any) can remain here
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