using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangedFixed : State
{
    private float timerAttack;

    private RangedFixed ranged;

    private Vector3 directionAttack;

    private float distanceToPlayer;

    public float timerRecharge;

    private float timer;

    private float bulletsShoot;

    public float bulletsCanShoot;


    public override void Enter()
    {

        ranged = GetComponent<RangedFixed>();

        timerAttack = 0;

        timer = 0;

        // melee.enemy_animator.enabled = false;

        ranged.enemy_navmesh.isStopped = true;
    }

    public override void Execute()
    {
        timerAttack += Time.deltaTime;

        distanceToPlayer = ranged.GetDistance(ranged.player.transform.position);

        directionAttack = (ranged.enemy_navmesh.transform.position - ranged.player.transform.position).normalized;

        Quaternion lookOnLook = Quaternion.LookRotation(-directionAttack);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, 0.5f);
        if(bulletsShoot < bulletsCanShoot)
        {
            if (timerAttack >= 0.1f)
            {
                Vector3 positionProjectile = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
                Instantiate(ranged.projectile, positionProjectile, transform.rotation);

                bulletsShoot++;
                timerAttack = 0;
            }

        }
        else
        {
            timer += Time.deltaTime;

            if (timer >= timerRecharge)
            {
                timer = 0;
                bulletsShoot = 0;
            }
        }
        if (distanceToPlayer > ranged.distanceToAttack || ranged.distanceY > ranged.distanceYForAttack) ranged.ChangeState(ranged.look);

    }



    public override void Exit()
    {



    }
}
