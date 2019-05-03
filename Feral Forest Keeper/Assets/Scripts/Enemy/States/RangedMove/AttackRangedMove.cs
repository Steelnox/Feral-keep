using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangedMove : State
{
    private float timerAttack;

    private bool projectile_done;

    private RangedMove ranged;

    private Vector3 directionAttack;




    public override void Enter()
    {

        ranged = GetComponent<RangedMove>();

        timerAttack = 0;

        projectile_done = false;

        // melee.enemy_animator.enabled = false;

        ranged.enemy_navmesh.isStopped = true;
    }

    public override void Execute()
    {
        timerAttack += Time.deltaTime;

        directionAttack = (ranged.enemy_navmesh.transform.position - ranged.player.transform.position).normalized;

        Quaternion lookOnLook = Quaternion.LookRotation(-directionAttack);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, 0.5f);

        if (!projectile_done && timerAttack >= 1)
        {
            Vector3 positionProjectile = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
            Instantiate(ranged.projectile, positionProjectile, transform.rotation);
            projectile_done = true;
            ranged.ChangeState(ranged.chase);
        }



    }



    public override void Exit()
    {
        timerAttack = 0;


        // melee.enemy_animator.SetBool("Attack", false);

        ranged.enemy_navmesh.isStopped = false;



    }

   
}
