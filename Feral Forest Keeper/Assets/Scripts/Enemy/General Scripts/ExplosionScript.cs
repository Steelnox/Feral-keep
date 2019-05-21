using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    private PlayerController player;

    public bool dmg_done;

    

    void Start()
    {
        player = PlayerController.instance;
        dmg_done = false;


    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !dmg_done)
        {
            player.GetDamage(1);

            dmg_done = true;
        }
    }

}
