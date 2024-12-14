using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharControl : MonoBehaviour
{
    private CharacterController characterController;
    private UnityEngine.Vector3 CharDirection;
    private UnityEngine.Vector2 ControlInput;
    private float CurrentVel;
    public float JumpStrength = 1.0f;
    public float GravBase = -9.81f;
    public float speed = 5f;
    private float RotateRef;
    private Camera mainCamera;
    public UnityEngine.Vector3 DashDir;
    public float DashSpeed;
    public float DashGroundSpeed;
    public bool DashState = false;
    public float DashTime;
    public bool GrabbingLedge = false;
    public bool JumpingFromLedge = false;
    public UnityEngine.Vector3 LedgeDir;
    public UnityEngine.Vector3 LedgePos;
    public bool DamageState = false;
    public UnityEngine.Vector3 KnockbackDir;
    [SerializeField] private float KnockStrength;
    [SerializeField] private float InvTime;
    [SerializeField] private MeshRenderer MyMeshVisibility;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        ApplyRot();
        ApplyGrav();
        ApplyMove();
        ApplyGrab();
    }

    private void ApplyRot()
    {
        if (ControlInput.sqrMagnitude == 0 || GrabbingLedge == true) return;

        CharDirection = UnityEngine.Quaternion.Euler(0.0f,mainCamera.transform.eulerAngles.y,0.0f)*new UnityEngine.Vector3(ControlInput.x,0.0f,ControlInput.y);
        var targetRotation = UnityEngine.Quaternion.LookRotation(CharDirection,UnityEngine.Vector3.up);
        transform.rotation = UnityEngine.Quaternion.RotateTowards(transform.rotation,targetRotation,1000f*Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        ControlInput = context.ReadValue<UnityEngine.Vector2>();
        CharDirection = new UnityEngine.Vector3(ControlInput.x, 0.0f, ControlInput.y).normalized;
    }
    public void ApplyMove()
    {
        if (GrabbingLedge == true) return;
        
        characterController.Move(CharDirection*Time.deltaTime*speed);
    }
    public void ApplyGrab()
    {
        if (GrabbingLedge == true)
        {
            transform.rotation = UnityEngine.Quaternion.LookRotation(LedgeDir,UnityEngine.Vector3.up);
            transform.position = LedgePos;
            CurrentVel = -1.0f;
        }
    }

    public void ApplyHurt()
    {
        Debug.Log("SpikeDetected, vector: " + KnockbackDir);
        StartCoroutine(Invincibility());
    }

    private void ApplyGrav()
    {
        if (GrabbingLedge == true || Time.timeScale < 1f) return;
        if (characterController.isGrounded && CurrentVel < 0.0f)
        {
            CurrentVel = -1.0f;
        }
        else
        {
            CurrentVel += GravBase*Time.deltaTime;
        }
        CharDirection.y = CurrentVel;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started || Time.timeScale < 1f) return;
        if (!characterController.isGrounded && GrabbingLedge == false) return;
        if (GrabbingLedge == true)
        {
            GrabbingLedge = false;
            JumpingFromLedge = true;
            StartCoroutine(JumpFromLedge());
            CurrentVel += JumpStrength*1.3f;
        }
        else
        {
            CurrentVel += JumpStrength;
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (!context.started || Time.timeScale < 1f) return;
        if (DashState == true || GrabbingLedge == true) return;
        DashState = true;

        float targetAngle = Mathf.Atan2(ControlInput.x,ControlInput.y) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
        DashDir = UnityEngine.Quaternion.Euler(0f,targetAngle,0f) * UnityEngine.Vector3.forward;
        StartCoroutine(DashCoroutine());
    }
    private IEnumerator DashCoroutine()
    {
        float startTime = Time.time;
        while(Time.time < startTime + DashTime && GrabbingLedge == false)
        {
            if (!characterController.isGrounded)
            {
                characterController.Move
                (new UnityEngine.Vector3(transform.forward.x,transform.forward.y+0.125f,transform.forward.z)
                * DashSpeed * Time.deltaTime);
            }
            else
            {
                characterController.Move
                (new UnityEngine.Vector3(transform.forward.x,transform.forward.y,transform.forward.z)
                * DashGroundSpeed * Time.deltaTime);
            }
            yield return null; // this will make Unity stop here and continue next frame
        }
        DashState = false;
    }
    /*public void DashWait()
    {
        DashState = true;
        StartCoroutine(MenuCooldown());
    }
    private IEnumerator MenuCooldown()
    {
        yield return new WaitForSeconds(0.25f);
        DashState = false;
        yield return null;
    }*/

    private IEnumerator JumpFromLedge()
    {
        yield return new WaitForSeconds(0.6f);
        JumpingFromLedge = false;
        yield return null;
    }

    private IEnumerator Invincibility()
    {
        float startTime = Time.time;
        while(Time.time < startTime + InvTime/6)
        {
            characterController.Move(KnockbackDir*Time.deltaTime*KnockStrength);
            if (MyMeshVisibility.forceRenderingOff == true)
            {
                MyMeshVisibility.forceRenderingOff = false;
            }
            else
            {
                MyMeshVisibility.forceRenderingOff = true;
            }
            yield return null;
        }
        while(Time.time < startTime + (InvTime*5)/6)
        {
            if (MyMeshVisibility.forceRenderingOff == true)
            {
                MyMeshVisibility.forceRenderingOff = false;
            }
            else
            {
                MyMeshVisibility.forceRenderingOff = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
        MyMeshVisibility.forceRenderingOff = false;
        //yield return new WaitForSeconds((InvTime*5)/6);
        DamageState = false;
    }
}