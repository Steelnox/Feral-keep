using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : State
{
    private Melee melee;

    public override void Enter()
    {
        melee = GetComponent<Melee>();


        melee.enemy_animator.SetFloat("Speed", melee.enemy_navmesh.speed);

    }

    public override void Execute()
    {

        melee.enemy_navmesh.SetDestination(melee.player.transform.position);

        if (melee.GetDistance(melee.player.transform.position) >= melee.distanceToChase) melee.ChangeState(melee.patrol);
        if (melee.GetDistance(melee.player.transform.position) <= melee.distanceToAttack) melee.ChangeState(melee.attack);

    }

    public override void Exit()
    {


    }

}
