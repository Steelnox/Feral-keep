using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedBoolAttack : State
{
    private RangedFixed ranged;

    private Vector3 directionAttack;

    private Projectile projectile;


    private float distanceToPlayer;

    public float timerRecharge;

    private float timer;

    private float bulletsShoot;

    public float bulletsCanShoot;

    public bool firstTime = true;


    public override void Enter()
    {

        ranged = GetComponent<RangedFixed>();

        timer = 0;

        bulletsShoot = 0;

        // melee.enemy_animator.enabled = false;

        ranged.enemy_navmesh.isStopped = true;

        if (distanceToPlayer <= ranged.distanceToAttack)
        {
            ranged.enemy_animator.SetBool("Attack", true);
            ranged.enemy_animator.SetBool("Idle", false);

        }
        else
        {
            ranged.enemy_animator.SetBool("Attack", false);
            ranged.enemy_animator.SetBool("Idle", true);
        }


    }

    public override void Execute()
    {
        directionAttack = transform.forward;

        distanceToPlayer = ranged.GetDistance(ranged.player.transform.position);

        if (distanceToPlayer <= ranged.distanceToAttack)
        {
            if (bulletsShoot < bulletsCanShoot)
            {
                if (firstTime)
                {
                    ranged.enemy_animator.SetBool("Attack", true);
                    ranged.enemy_animator.SetBool("Idle", false);
                    firstTime = false;
                }
                

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

                    ranged.enemy_animator.SetBool("Idle", false);
                    ranged.enemy_animator.SetBool("Attack", true);


                }
            }
        }
        else
        {
            bulletsShoot = 0;

            ranged.enemy_animator.SetBool("Attack", false);
            ranged.enemy_animator.SetBool("Idle", true);
        }
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
