using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : State
{
    float timer;
    public float timeRest;
    private Melee melee;


    public override void Enter()
    {
        melee = GetComponent<Melee>();

        transform.localScale = new Vector3(1, 0.5f, 1);
        timer = 0;

        melee.enemy_navmesh.isStopped = true;


    }

    public override void Execute()
    {
        timer += Time.deltaTime;
       if(timer >= timeRest)
        {
            if (melee.GetDistance(melee.player.transform.position) <= melee.distanceToChase) melee.ChangeState(melee.chase);
            if (melee.GetDistance(melee.player.transform.position) <= melee.distanceToAttack) melee.ChangeState(melee.attack);
        }
    }

    public override void Exit()
    {
        transform.localScale = new Vector3(1, 1, 1);


        melee.enemy_navmesh.isStopped = false;

    }
}
