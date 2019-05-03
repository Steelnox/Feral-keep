﻿using System.Collections;
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


        // melee.enemy_animator.enabled = false;

        ranged.enemy_navmesh.isStopped = true;
    }

    public override void Execute()
    {
        distanceToPlayer = ranged.GetDistance(ranged.player.transform.position);

        directionAttack = (ranged.enemy_navmesh.transform.position - ranged.player.transform.position).normalized;

        Quaternion lookOnLook = Quaternion.LookRotation(-directionAttack);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, 0.5f);

        if (distanceToPlayer <= ranged.distanceToAttack) ranged.ChangeState(ranged.attack);
    }



    public override void Exit()
    { 




    }

}
