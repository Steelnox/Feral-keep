using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RangedMove : Enemy
{
    public State patrol;
    public State chase;
    public State attack;
    public State flee;
    public State damaged;
    public State rage;

    public CapsuleCollider C_collider;
    public Projectile projectile;

    void Start()
    {
        player = PlayerController.instance;
        enemyType = EnemyType.MELEE;

        enemy_navmesh = GetComponent<NavMeshAgent>();

        C_collider = GetComponent<CapsuleCollider>();

        enemy_rb = GetComponent<Rigidbody>();

        currentHealth = maxHealth;

        ChangeState(patrol);
    }

    void Update()
    {

        stateMachine.ExecuteState();
        //Debug.Log(currentState);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerWeapon" && currentState != damaged)
        {
            ChangeState(damaged);
            HealthBar.SetActive(true);
            float x = HealthBar.transform.localScale.x * 0.5f;
            HealthBar.transform.localScale = new Vector3(x, 1, 1);
        }
    }
}
