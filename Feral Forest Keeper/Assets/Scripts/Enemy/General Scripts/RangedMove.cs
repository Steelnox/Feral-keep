﻿using System.Collections;
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

    public bool shoot;

    public CapsuleCollider C_collider;
    public Projectile projectile;

    EnemyHitFeedbackController feedbackController;

    void Start()
    {
        player = PlayerController.instance;

        gamemanagerScript = GameManager.instance;

        enemyType = EnemyType.RANGED;

        enemy_navmesh = GetComponent<NavMeshAgent>();

        C_collider = GetComponent<CapsuleCollider>();

        enemy_rb = GetComponent<Rigidbody>();

        currentHealth = maxHealth;

        shoot = false;

        ChangeState(patrol);


        feedbackController = GetComponentInChildren<EnemyHitFeedbackController>();
        if (feedbackController == null) Debug.Log("Cant find EnemyFeedbackController");
    }

    void Update()
    {

        enemy_animator.SetFloat("Speed", enemy_navmesh.speed);

        if (move)
        {
            enemy_navmesh.isStopped = false;
        }

        else
        {
            enemy_navmesh.isStopped = true;

        }
        stateMachine.ExecuteState();

        if(currentHealth <= 0)
        {
            this.gameObject.SetActive(false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerWeapon" && currentState != damaged)
        {
            ChangeState(damaged);
            HealthBar.SetActive(true);
            float x = HealthBar.transform.localScale.x * 0.5f;
            HealthBar.transform.localScale = new Vector3(x, 1, 1);
            feedbackController.Hit();
        }
    }

}
