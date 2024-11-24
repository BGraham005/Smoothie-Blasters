using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharControl : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 CharDirection;
    private Vector2 ControlInput;
    private float CurrentVel;
    [SerializeField] private float JumpStrength = 1.0f;

    private float GravBase = -9.81f;
    public float speed = 5f;
    public float RotateRef;
    private Camera mainCamera;

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
    }

    private void ApplyRot()
    {
        if (ControlInput.sqrMagnitude == 0) return;

        CharDirection = Quaternion.Euler(0.0f,mainCamera.transform.eulerAngles.y,0.0f)*new Vector3(ControlInput.x,0.0f,ControlInput.y);
        var targetRotation = Quaternion.LookRotation(CharDirection,Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,1000f*Time.deltaTime);

        //var targetAngle = Mathf.Atan2(CharDirection.x,CharDirection.z) * Mathf.Rad2Deg;
        //var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle,ref RotateRef,0.05f);
        //transform.rotation = Quaternion.Euler(0.0f,angle,0.0f);
    }

    public void Move(InputAction.CallbackContext context)
    {
        ControlInput = context.ReadValue<Vector2>();
        CharDirection = new Vector3(ControlInput.x, 0.0f, ControlInput.y);
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
}
