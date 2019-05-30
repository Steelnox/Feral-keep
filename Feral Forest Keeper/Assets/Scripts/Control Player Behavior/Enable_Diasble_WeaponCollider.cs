using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enable_Diasble_WeaponCollider : MonoBehaviour
{
    public BoxCollider weaponCollider;

    void Start()
    {
        DisableWeaponCollider();
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
    public void EnableWeaponCollider()
    {
        weaponCollider.enabled = true;
    }
    public void DisableWeaponCollider()
    {
        weaponCollider.enabled = false;
    }
    public void EnableWeaponTrail()
    {
        PlayerController.instance.attackTrail.enabled = true;
    }
    public void DisableWeaponTrail()
    {
        PlayerController.instance.attackTrail.enabled = false;
    }
    public void PlayerCanMove()
    {
        PlayerController.instance.SetCanMove(true);
    }
}
