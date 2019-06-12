using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedRangedFixed : State
{

    private RangedFixed ranged;

    private float distanceToPlayer;

    public override void Enter()
    {

        ranged = GetComponent<RangedFixed>();

        ranged.currentHealth -= ranged.player.damagePlayer;

        ranged.enemy_navmesh.isStopped = true;

        ranged.enemy_rb.isKinematic = false;

        ranged.enemy_animator.SetBool("Hit", true);

        ranged.finishHit = false;


    }

    public override void Execute()
    {

        distanceToPlayer = ranged.GetDistance(ranged.player.transform.position);

        if (ranged.finishHit)
        {
            if (ranged.currentHealth <= 0)
            {
                ranged.chasing = false;
                this.gameObject.SetActive(false);
            }

            if (ranged.currentHealth > 0)
            {
                if (distanceToPlayer <= ranged.distanceToAttack) ranged.ChangeState(ranged.attack);
                if (distanceToPlayer > ranged.distanceToAttack) ranged.ChangeState(ranged.look);

            }
        }
        

    }

    public override void Exit()
    {
        if (ranged.currentHealth > 0)
        {
            ranged.enemy_animator.SetBool("Hit", false);

            ranged.enemy_rb.isKinematic = true;
        }

    }
}
