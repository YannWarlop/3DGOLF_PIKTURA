using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ThirdPersonCameraBehaviour : MonoBehaviour
{
    public Transform ball;
    public Transform cameraTarget;
    float mouseX, mouseY;
    [Range(0.01f, 1.0f)] public float influence;

    private void Start()
    {
        //Cursor.visible = false;
    }

    void Update()
    { 
        cameraTarget.transform.position = Vector3.MoveTowards(cameraTarget.transform.position, ball.transform.position, influence);
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            CameraControl();
        }
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void CameraControl()
    {
        mouseX += Input.GetAxis("Mouse X");
        mouseY -= Input.GetAxis("Mouse Y");
        mouseY = Mathf.Clamp(mouseY, -90, 90);
        
        transform.LookAt(cameraTarget);
        cameraTarget.rotation = Quaternion.Euler(mouseY, mouseX, 0);
    }
}
