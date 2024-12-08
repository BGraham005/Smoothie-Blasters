using Unity.VisualScripting;
using UnityEngine;

public class SpikeHurtbox : MonoBehaviour
{
    private GameObject player;
    private CharControl script;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.GameObject();
            script = player.GetComponent<CharControl>();
            if (script.DamageState == false)
            {
                script.DamageState = true;
                script.KnockbackDir = script.transform.position - transform.position;
                script.ApplyHurt();
            }
        }
    }
}
