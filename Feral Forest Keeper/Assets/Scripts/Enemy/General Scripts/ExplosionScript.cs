using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    private PlayerController player;

    public bool dmg_done;

    private float timer;

    public GameObject melee_enemy;
    

    void Start()
    {
        player = PlayerController.instance;
        dmg_done = false;

        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 0.7f)
        {
            melee_enemy.GetComponent<Melee>().chasing = false;
            melee_enemy.SetActive(false);
        }
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
