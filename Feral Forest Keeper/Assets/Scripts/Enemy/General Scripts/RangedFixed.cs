using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedFixed : Enemy
{
    public State look;
    public State attack;
    public State damaged;
    public State fixAttack;
    public State underground;

    public CapsuleCollider C_collider;

    public bool onedirectionAttack;

    public float distanceY;

    public float distanceYForAttack;

    public float velocityRockToKill;

    public bool shoot;

    public bool finishHit;

    public bool enemyUnder;

    EnemyHitFeedbackController feedbackController;

    void Start()
    {
        player = PlayerController.instance;

        gamemanagerScript = GameManager.instance;


        enemyType = EnemyType.MELEE;

        enemy_navmesh = GetComponent<NavMeshAgent>();

        C_collider = GetComponent<CapsuleCollider>();

        enemy_rb = GetComponent<Rigidbody>();

        currentHealth = maxHealth;

        enemy_navmesh.isStopped = true;

        shoot = false;

        finishHit = false;

        if (onedirectionAttack)
        {
            ChangeState(fixAttack);
        }
        else
        {
            ChangeState(underground);
        }

        feedbackController = GetComponentInChildren<EnemyHitFeedbackController>();
        if (feedbackController == null) Debug.Log("Cant find EnemyFeedbackController");
    }

    void Update()
    {

        stateMachine.ExecuteState();

        distanceY = transform.position.y - player.transform.position.y;

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

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "MovableRock" && collision.relativeVelocity.magnitude > velocityRockToKill)
        {
            gameObject.SetActive(false);
        }
    }
}
