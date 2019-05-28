using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangedMove : State
{
    private float timerAttack;

    private bool projectile_done;

    private RangedMove ranged;

    private Vector3 directionAttack;

    private Projectile projectile;



    public override void Enter()
    {

        ranged = GetComponent<RangedMove>();

        timerAttack = 0;

        projectile_done = false;

        ranged.shoot = false;
        
        ranged.enemy_animator.SetBool("Attack", true);

        // melee.enemy_animator.enabled = false;

    }

    public override void Execute()
    {
        //timerAttack += Time.deltaTime;

        directionAttack = (ranged.enemy_navmesh.transform.position - ranged.player.transform.position).normalized;

        Quaternion lookOnLook = Quaternion.LookRotation(-directionAttack);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, 0.5f);

        if (!projectile_done && ranged.shoot)
        {
            Shoot();
        }

    }



    public override void Exit()
    {
        timerAttack = 0;


        ranged.enemy_animator.SetBool("Attack", false);


    }

    private void Shoot()
    {
        Vector3 positionProjectile = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        projectile = ranged.gamemanagerScript.GetProjectileNotActive();
        projectile.transform.position = positionProjectile;
        projectile.transform.rotation = transform.rotation;
        projectile_done = true;
        ranged.ChangeState(ranged.chase);

    }


}
