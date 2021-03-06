﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    #region Variables
    float randomPosX;
    float randomPosZ;

    Vector3 patrolPoint;
    float distanceToPoint;
    [Space(10)]

    public GameObject PosRight;
    public GameObject PosLeft;

    float timer;
    #endregion

    private Melee melee;

    public override void Enter()
    {
        melee = GetComponent<Melee>();
        distanceToPoint = 1.0f;

        //melee.enemy_animator.SetTrigger("Walk");

        AssignRandom();
        melee.enemy_navmesh.speed = melee.speed;

        melee.enemy_animator.SetBool("Move", true);

        melee.chasing = false;
        timer = 0;
    }

    public override void Execute()
    {

        timer += Time.deltaTime;
        if(timer >= 5.0f)
        {
            AssignRandom();
            timer = 0;
        }
        if (melee.GetDistance(patrolPoint) < distanceToPoint)
        {
            AssignRandom();
            timer = 0;
        }

        if (melee.GetDistance(melee.player.transform.position) <= melee.distanceToChase) melee.ChangeState(melee.chase);
    }

    public override void Exit()
    {
        melee.enemy_animator.SetBool("Move", false);
    }

    public void AssignRandom()
    {
        randomPosX = Random.Range(PosLeft.transform.position.x, PosRight.transform.position.x);
        randomPosZ = Random.Range(PosLeft.transform.position.z, PosRight.transform.position.z);

        patrolPoint = new Vector3(randomPosX, transform.position.y, randomPosZ);

        melee.enemy_navmesh.SetDestination(patrolPoint);
    }

   

    public float GetDistance(Vector3 patrolPoint)
    {
        return Vector3.Distance(patrolPoint, transform.position);
    }
}

