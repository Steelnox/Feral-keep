using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundFixed : State
{
    private RangedFixed ranged;

    private Vector3 directionAttack;

    private float distanceToPlayer;



    public override void Enter()
    {

        ranged = GetComponent<RangedFixed>();

        Vector3 temp = transform.position;
        temp.y -= 0.25f;
        transform.position = temp; 

        // melee.enemy_animator.enabled = false;

        ranged.enemy_navmesh.isStopped = true;
    }

    public override void Execute()
    {

        if (distanceToPlayer <= ranged.distanceToAttack) ranged.ChangeState(ranged.look);
    }

    public override void Exit()
    {
        Vector3 temp = transform.position;
        temp.y += 0.25f;
        transform.position = temp;
    }
}
