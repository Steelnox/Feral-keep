using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseRanged : State
{
    private RangedMove ranged;
    private float distanceToPlayer;
    private float timer;

    public override void Enter()
    {
        ranged = GetComponent<RangedMove>();

        timer = 0;

        ranged.enemy_navmesh.ResetPath();
    }

    public override void Execute()
    {

        if (distanceToPlayer > ranged.distanceToAttack)
        {

            ranged.enemy_navmesh.SetDestination(ranged.player.transform.position);
        }
     


        timer += Time.deltaTime;
        distanceToPlayer = ranged.GetDistance(ranged.player.transform.position);
        if (distanceToPlayer <= ranged.distanceToFlee) ranged.ChangeState(ranged.flee);
        if (distanceToPlayer >= ranged.distanceToChase) ranged.ChangeState(ranged.patrol);
        if (distanceToPlayer <= ranged.distanceToAttack && distanceToPlayer > ranged.distanceToFlee) ranged.ChangeState(ranged.attack);

        
    }

    public override void Exit()
    {
        

    }
}
