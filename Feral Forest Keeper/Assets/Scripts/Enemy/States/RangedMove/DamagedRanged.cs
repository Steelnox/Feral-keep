using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedRanged : State
{

    float timer;
    public float timeKnockback;

    private RangedMove ranged;

    private Vector3 directionKnockback;



    public override void Enter()
    {

        ranged = GetComponent<RangedMove>();

        ranged.currentHealth -= ranged.player.damagePlayer;

        directionKnockback = (ranged.enemy_navmesh.transform.position - ranged.player.transform.position).normalized;

        timer = 0;


        ranged.enemy_navmesh.isStopped = true;

        ranged.enemy_rb.isKinematic = false;

        ranged.enemy_rb.AddForce(directionKnockback * 3.5f, ForceMode.Impulse);


    }

    public override void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= timeKnockback)
        {
            if(ranged.currentHealth == 1)
            {
                ranged.ChangeState(ranged.rage);
            }
            if (ranged.currentHealth <= 0) this.gameObject.SetActive(false);
           /* if (ranged.currentHealth > 0)
            {
                if (ranged.GetDistance(ranged.player.transform.position) <= ranged.distanceToChase) ranged.ChangeState(ranged.chase);
                //if (ranged.GetDistance(ranged.player.transform.position) <= ranged.distanceToAttack) ranged.ChangeState(ranged.attack);
            }*/
            
        }
        Debug.Log(ranged.currentHealth);

    }

    public override void Exit()
    {
        if (ranged.currentHealth > 0)
        {
            ranged.enemy_navmesh.isStopped = false;

            ranged.enemy_rb.isKinematic = true;
        }

    }
}
