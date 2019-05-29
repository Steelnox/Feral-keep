using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Melee : Enemy
{
    public State patrol;
    public State chase;
    public State attack;
    public State rest;
    public State damaged;
    public State rage;

    public CapsuleCollider C_collider;

    public bool finishAttack;
    public bool startexplosion;

    void Start()
    {
        player = PlayerController.instance;

        enemyType = EnemyType.MELEE;

        enemy_navmesh = GetComponent<NavMeshAgent>();


        C_collider = GetComponent<CapsuleCollider>();

        enemy_rb = GetComponent<Rigidbody>();

        currentHealth = maxHealth;

        startexplosion = false;

        ChangeState(patrol);
    }

    void Update()
    {
        
        if (move)
        {
            enemy_navmesh.isStopped = false;
        }

        else
        {
            enemy_navmesh.isStopped = true;

        }

        stateMachine.ExecuteState();
        //Debug.Log(currentState);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerWeapon" && currentState != damaged)
        {
            ChangeState(damaged);
            HealthBar.SetActive(true);
            float x = HealthBar.transform.localScale.x * 0.5f;
            HealthBar.transform.localScale = new Vector3(x, 1, 1);

        }
    }


}
