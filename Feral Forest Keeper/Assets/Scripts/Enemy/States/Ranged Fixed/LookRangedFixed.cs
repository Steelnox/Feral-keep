using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRangedFixed : State
{

    private RangedFixed ranged;

    private Vector3 directionAttack;

    private float distanceToPlayer;



    public override void Enter()
    {

        ranged = GetComponent<RangedFixed>();

        ranged.enemy_animator.SetBool("Idle", true);


        ranged.enemy_navmesh.isStopped = true;

        ranged.chasing = false;

    }

    public override void Execute()
    {

        distanceToPlayer = ranged.GetDistance(ranged.player.transform.position);

        directionAttack = (ranged.enemy_navmesh.transform.position - ranged.player.transform.position).normalized;

        Quaternion lookOnLook = Quaternion.LookRotation(-directionAttack);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, 0.5f);


        if (distanceToPlayer <= ranged.distanceToAttack)
        {
            if (ranged.enemyUnder && ranged.distanceY > 0)
            {
                ranged.ChangeState(ranged.attack);
            }

            else if (!ranged.enemyUnder && ranged.distanceY < ranged.distanceYForAttack)
            {
                ranged.ChangeState(ranged.attack);
            }
        }


    }



    public override void Exit()
    {


        ranged.enemy_animator.SetBool("Idle", false);


    }

}
