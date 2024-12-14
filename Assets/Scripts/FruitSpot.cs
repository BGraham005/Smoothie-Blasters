using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FruitSpot : MonoBehaviour
{
    private GameObject player;
    private CharControl playerscript;
    [SerializeField] private GameObject cannon;
    private CannonBrain cannonscript;
    [SerializeField] private SpriteRenderer MyImg;
    public int MyFruit;
    public bool IsEnabled;

    void Start()
    {
        MyImg.forceRenderingOff = true;
        cannonscript = cannon.GetComponent<CannonBrain>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && IsEnabled == true)
        {
            player = other.GameObject();
            playerscript = player.GetComponent<CharControl>();
            MyImg.forceRenderingOff = true;
            IsEnabled = false;

            if (cannonscript.FruitsRemaining.Contains(MyFruit))
            {
                cannonscript.FruitsOwned[Array.IndexOf(cannonscript.FruitsRemaining,MyFruit)] = true;
                cannonscript.FruitsRemaining[Array.IndexOf(cannonscript.FruitsRemaining,MyFruit)] = -1;
                cannonscript.OnFruitNum++;
            }
        }
    }

    public void SetMyImg()
    {
        MyImg.forceRenderingOff = false;
        MyImg.sprite = cannonscript.SpriteList[MyFruit];
        IsEnabled = true;
    }

}
