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
            if (script.GrabbingLedge == false && script.JumpingFromLedge == false)
            {
                script.GrabbingLedge = true;
                script.LedgeDir = transform.rotation * Vector3.forward;
                script.LedgePos = transform.position - (script.LedgeDir/2);
            }
        }
    }
}
