using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangedFixed : State
{
    private RangedFixed ranged;

    private Vector3 directionAttack;

    private Projectile projectile;


    private float distanceToPlayer;

    public float timerRecharge;

    private float timer;

    private float bulletsShoot;

    public float bulletsCanShoot;


    public override void Enter()
    {

        ranged = GetComponent<RangedFixed>();


        timer = 0;

        bulletsShoot = 0;

        // melee.enemy_animator.enabled = false;

        ranged.enemy_navmesh.isStopped = true;
        ranged.enemy_animator.SetBool("Attack", true);

        ranged.chasing = true;


    }

    public override void Execute()
    {
        distanceToPlayer = ranged.GetDistance(ranged.player.transform.position);

        directionAttack = (ranged.enemy_navmesh.transform.position - ranged.player.transform.position).normalized;

        Quaternion lookOnLook = Quaternion.LookRotation(-directionAttack);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, 0.5f);

        if(bulletsShoot < bulletsCanShoot)
        {

            if (ranged.shoot)
            {
                Shoot();
            }

        }
        else
        {
            timer += Time.deltaTime;
            ranged.enemy_animator.SetBool("Idle", true);
            ranged.enemy_animator.SetBool("Attack", false);


            if (timer >= timerRecharge)
            {
                timer = 0;
                bulletsShoot = 0;
                ranged.enemy_animator.SetBool("Attack", true);

                ranged.enemy_animator.SetBool("Idle", false);

            }
        }
        if (distanceToPlayer > ranged.distanceToAttack)
        {
            bulletsShoot = 0;
            ranged.ChangeState(ranged.look);
        }

        if (ranged.enemyUnder && ranged.distanceY < 0)
        {
            bulletsShoot = 0;
            ranged.ChangeState(ranged.look);
        }

        else if (!ranged.enemyUnder && ranged.distanceY > ranged.distanceToAttack)
        {
            bulletsShoot = 0;
            ranged.ChangeState(ranged.look);
        }

    }

    public override void Exit()
    {
        ranged.enemy_animator.SetBool("Attack", false);
    }

    private void Shoot()
    {
        Vector3 positionProjectile = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        projectile = ranged.gamemanagerScript.GetProjectileNotActive();
        projectile.transform.position = positionProjectile;
        projectile.transform.rotation = transform.rotation;

        bulletsShoot++;
        ranged.shoot = false;

    }
}
