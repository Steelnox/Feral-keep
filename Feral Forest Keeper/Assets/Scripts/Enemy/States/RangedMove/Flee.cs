using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : State
{
    private RangedMove ranged;
    private float distanceToPlayer;

    public override void Enter()
    {
        ranged = GetComponent<RangedMove>();

    }

    public override void Execute()
    {
        distanceToPlayer = ranged.GetDistance(ranged.player.transform.position);

        Vector3 dirToPlayer = transform.position - ranged.player.transform.position;

        Vector3 newpos = transform.position + dirToPlayer;

        ranged.enemy_navmesh.SetDestination(newpos);

        if (ranged.SeesPlayer() && distanceToPlayer <= ranged.distanceToChase && distanceToPlayer > ranged.distanceToAttack) ranged.ChangeState(ranged.chase);
        if (distanceToPlayer >= ranged.distanceToAttack) ranged.ChangeState(ranged.attack);


    }

    public override void Exit()
    {


    }
}
