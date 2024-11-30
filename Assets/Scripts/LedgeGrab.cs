using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    private GameObject player;
    private CharControl script;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.GameObject();
            script = player.GetComponent<CharControl>();
            script.GrabbingLedge = true;
        }
    }
}
