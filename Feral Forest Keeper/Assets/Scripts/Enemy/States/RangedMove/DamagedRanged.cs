using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedRanged : State
{

    float timer;
    public float timeKnockback;

    private RangedMove ranged;

    private Vector3 directionKnockback;

    [FMODUnity.EventRef]
    public string dieEvent;

    [FMODUnity.EventRef]
    public string hitEvent;


    public override void Enter()
    {

        ranged = GetComponent<RangedMove>();

        ranged.currentHealth -= ranged.player.damagePlayer;

        directionKnockback = (ranged.enemy_navmesh.transform.position - ranged.player.transform.position).normalized;

        timer = 0;

        ranged.enemy_animator.SetBool("Hit", true);

        ranged.enemy_rb.isKinematic = false;

        ranged.enemy_rb.AddForce(directionKnockback * 3.5f, ForceMode.Impulse);

        FMODUnity.RuntimeManager.PlayOneShot(hitEvent, transform.position);

        if (ranged.currentHealth <= 0)
        {
            FMODUnity.RuntimeManager.PlayOneShot(dieEvent, transform.position);
        }
    }

    public override void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= timeKnockback)
        {
            if(ranged.currentHealth <= 1 && ranged.currentHealth > 0)
            {
                ranged.ChangeState(ranged.rage);
            }
            if (ranged.currentHealth <= 0)
            {
                ranged.chasing = false;
                this.gameObject.SetActive(false);
            }
            if (ranged.currentHealth > 1 )
            {
                if (ranged.GetDistance(ranged.player.transform.position) <= ranged.distanceToChase) ranged.ChangeState(ranged.chase);
                if (ranged.GetDistance(ranged.player.transform.position) <= ranged.distanceToAttack) ranged.ChangeState(ranged.attack);
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
