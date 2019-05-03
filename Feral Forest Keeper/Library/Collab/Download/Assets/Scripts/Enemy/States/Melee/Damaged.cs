using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damaged : State
{

    float timer;
    public float timeKnockback;

    private Melee melee;

    private Vector3 directionKnockback;



    public override void Enter()
    {

        melee = GetComponent<Melee>();

        melee.currentHealth -= melee.player.damagePlayer;

        directionKnockback = (melee.enemy_navmesh.transform.position - melee.player.transform.position).normalized;

        timer = 0;
        Debug.Log(melee.currentHealth);

        melee.enemy_navmesh.isStopped = true;

        melee.enemy_rb.isKinematic = false;

        melee.enemy_rb.AddForce(directionKnockback * 3.5f, ForceMode.Impulse);


    }

    public override void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= timeKnockback)
        {
            if (melee.currentHealth <= 0) Destroy(this.gameObject);
            if(melee.currentHealth > 0)
            {
                if (melee.GetDistance(melee.player.transform.position) <= melee.distanceToChase) melee.ChangeState(melee.chase);
                if (melee.GetDistance(melee.player.transform.position) <= melee.distanceToAttack) melee.ChangeState(melee.attack);
            }
        }

    }

    public override void Exit()
    {
        if(melee.currentHealth > 0)
        {
            melee.enemy_navmesh.isStopped = false;

            melee.enemy_rb.isKinematic = true;
        }
        
    }
}
