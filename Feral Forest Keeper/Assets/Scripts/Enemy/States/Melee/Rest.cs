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

        timer = 0;

        melee.move = false;

        melee.enemy_animator.SetBool("Idle", true);



    }

    public override void Execute()
    {
        timer += Time.deltaTime;

        transform.LookAt(melee.player.transform.position);


    

        if (timer >= timeRest)
        {
            if (melee.GetDistance(melee.player.transform.position) <= melee.distanceToChase) melee.ChangeState(melee.chase);
            if (melee.GetDistance(melee.player.transform.position) <= melee.distanceToAttack) melee.ChangeState(melee.attack);
        }
    }

    public override void Exit()
    {

        melee.move = true;
        melee.enemy_animator.SetBool("Idle", false);



    }
}
