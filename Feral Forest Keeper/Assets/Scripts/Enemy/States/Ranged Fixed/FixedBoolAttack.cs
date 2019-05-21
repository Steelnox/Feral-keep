using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedBoolAttack : State
{
    private float timerAttack;

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

        ranged.gamemanagerScript = GameManager.instance;
        timerAttack = 0;

        timer = 0;

        bulletsShoot = 0;

        // melee.enemy_animator.enabled = false;

        ranged.enemy_navmesh.isStopped = true;
    }

    public override void Execute()
    {
        directionAttack = transform.forward;

        distanceToPlayer = ranged.GetDistance(ranged.player.transform.position);

        if (distanceToPlayer <= ranged.distanceToAttack)
        {
            timerAttack += Time.deltaTime;

            if (bulletsShoot < bulletsCanShoot)
            {
                Invoke("Shoot", 0.1f);
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
        }
        else
        {
            bulletsShoot = 0;
        }
    }

    private void Shoot()
    {
        Vector3 positionProjectile = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
        //Instantiate(ranged.projectile, positionProjectile, transform.rotation);
        projectile = ranged.gamemanagerScript.GetProjectileNotActive();
        projectile.gameObject.SetActive(true);
        projectile.transform.position = positionProjectile;
        projectile.transform.rotation = transform.rotation;

        bulletsShoot++;
        timerAttack = 0;
    }

}
