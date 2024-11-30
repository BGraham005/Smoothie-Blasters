using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharControl : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 CharDirection;
    private Vector2 ControlInput;
    private float CurrentVel;
    public float JumpStrength = 1.0f;
    public float GravBase = -9.81f;
    public float speed = 5f;
    private float RotateRef;
    private Camera mainCamera;
    public Vector3 DashDir;
    public float DashSpeed;
    private bool DashState = false;
    public float DashTime;
    public bool GrabbingLedge = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        ApplyRot();
        ApplyGrav();
        characterController.Move(CharDirection*Time.deltaTime*speed);
        if (GrabbingLedge == true) Debug.Log("hi");
    }

    private void ApplyRot()
    {
        if (ControlInput.sqrMagnitude == 0) return;

        CharDirection = Quaternion.Euler(0.0f,mainCamera.transform.eulerAngles.y,0.0f)*new Vector3(ControlInput.x,0.0f,ControlInput.y);
        var targetRotation = Quaternion.LookRotation(CharDirection,Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,1000f*Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        ControlInput = context.ReadValue<Vector2>();
        CharDirection = new Vector3(ControlInput.x, 0.0f, ControlInput.y).normalized;
    }

    private void ApplyGrav()
    {
        if (characterController.isGrounded && CurrentVel < 0.0f){
            CurrentVel = -1.0f;
        }
        else{
            CurrentVel += GravBase*Time.deltaTime;
        }
        CharDirection.y = CurrentVel;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!characterController.isGrounded) return;

        CurrentVel += JumpStrength;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (DashState == true) return;
        DashState = true;

        float targetAngle = Mathf.Atan2(ControlInput.x,ControlInput.y) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
        DashDir = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;
        StartCoroutine(DashCoroutine());
    }
    private IEnumerator DashCoroutine()
    {
        float startTime = Time.time;
        while(Time.time < startTime + DashTime)
        {
            characterController.Move(new Vector3(transform.forward.x,transform.forward.y+0.25f,transform.forward.z) * DashSpeed * Time.deltaTime);
            yield return null; // this will make Unity stop here and continue next frame
        }
        DashState = false;
    }
}