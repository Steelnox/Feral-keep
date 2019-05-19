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
    public Projectile projectile;

    public bool onedirectionAttack;

    public float distanceY;

    public float distanceYForAttack;

    public float velocityRockToKill;


    void Start()
    {
        player = PlayerController.instance;

        enemyType = EnemyType.MELEE;

        enemy_navmesh = GetComponent<NavMeshAgent>();

        C_collider = GetComponent<CapsuleCollider>();

        enemy_rb = GetComponent<Rigidbody>();

        currentHealth = maxHealth;

        enemy_navmesh.isStopped = true;

        if (onedirectionAttack)
        {
            ChangeState(fixAttack);
        }
        else
        {
            ChangeState(underground);
        }
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
