using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    private float CamOffset;
    private Vector2 MousePos;
    [SerializeField] private MouseSensitivity mouseSense;
    private CameraRotation camRotation;
    [SerializeField] private CameraAngle camAngle;

    private void Start()
    {
        CamOffset = Vector3.Distance(transform.position,target.position);
    }

    public void Camera(InputAction.CallbackContext context)
    {
        MousePos = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        camRotation.Yaw += MousePos.x*mouseSense.x*Time.deltaTime;
        camRotation.Pitch += MousePos.y*mouseSense.y*Time.deltaTime*-1f;
        camRotation.Pitch = Mathf.Clamp(camRotation.Pitch,camAngle.min,camAngle.max);
    }

    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(camRotation.Pitch,camRotation.Yaw, 0.0f);
        transform.position = target.position - transform.forward * CamOffset;
    }
}

[Serializable]
public struct MouseSensitivity
{
    public float x;
    public float y;
}

public struct CameraRotation
{
    public float Pitch;
    public float Yaw;
}

[Serializable]
public struct CameraAngle
{
    public float min;
    public float max;
}