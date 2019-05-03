using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRangedMove : State
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

    private RangedMove ranged;
    private float distanceToPlayer;

    public override void Enter()
    {
        ranged = GetComponent<RangedMove>();
        distanceToPoint = 1.0f;

        //melee.enemy_animator.SetTrigger("Walk");
        AssignRandom();
        ranged.enemy_navmesh.speed = ranged.speed;

        timer = 0;
    }

    public override void Execute()
    {
        distanceToPlayer = ranged.GetDistance(ranged.player.transform.position);
        timer += Time.deltaTime;
        if (timer >= 5.0f)
        {
            AssignRandom();
            timer = 0;
        }
        if (ranged.GetDistance(patrolPoint) < distanceToPoint)
        {
            AssignRandom();
            timer = 0;
        }

        if (ranged.SeesPlayer() &&  distanceToPlayer <= ranged.distanceToChase && distanceToPlayer > ranged.distanceToAttack) ranged.ChangeState(ranged.chase);
        if (distanceToPlayer <= ranged.distanceToFlee) ranged.ChangeState(ranged.flee);
        if (distanceToPlayer <= ranged.distanceToAttack && distanceToPlayer > ranged.distanceToFlee) ranged.ChangeState(ranged.attack);

    }

    public void AssignRandom()
    {
        randomPosX = Random.Range(PosLeft.transform.position.x, PosRight.transform.position.x);
        randomPosZ = Random.Range(PosLeft.transform.position.z, PosRight.transform.position.z);

        patrolPoint = new Vector3(randomPosX, transform.position.y, randomPosZ);

        ranged.enemy_navmesh.SetDestination(patrolPoint);
    }



    public float GetDistance(Vector3 patrolPoint)
    {
        return Vector3.Distance(patrolPoint, transform.position);
    }
}


