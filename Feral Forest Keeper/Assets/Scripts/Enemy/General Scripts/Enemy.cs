using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public abstract class Enemy : MonoBehaviour
{
    //Properties that ALL enemis will have, add if needed
    protected StateMachine stateMachine = new StateMachine();
    public State currentState;

    public Animator enemy_animator;

    //public Animator enemy_animator;

    public PlayerController player;

    public NavMeshAgent enemy_navmesh;
    public Rigidbody enemy_rb;


    public enum EnemyType { MELEE, RANGED } //If other types needed, just add
    public EnemyType enemyType;

    public GameManager gamemanagerScript;

    public bool move;


    #region Stats

    public int maxHealth;
    public int currentHealth;
    public int damage;
    public float speed;

    public float distanceToChase;
    public float distanceToAttack;
    public float distanceToFlee;

    public float m_ConeAngle = 60.0f;

    public LayerMask m_CollisionLayerMask;

    public GameObject HealthBar;




    #endregion

    private void Start()
    {
        player = PlayerController.instance;

        gamemanagerScript = GameManager.instance;

        move = false;


    }

    //Functions that ALL enemies will have, add if needed (override later with it's own implementation)
    public virtual void OnHit(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
    }

    public void ChangeState(State state)
    {
        stateMachine.ChangeState(state);

        currentState = stateMachine.currentState;
    }


    public float GetDistance(Vector3 targetpos)
    {
        return Vector3.Distance(targetpos, transform.position);
    }

    public bool SeesPlayer()
    {
        Vector3 l_Direction = (player.transform.position + Vector3.up * 0.2f) - transform.position;
        Ray l_Ray = new Ray(transform.position + Vector3.up * 0.5f, l_Direction);
        float l_Distance = l_Direction.magnitude;
        l_Direction /= l_Distance;
        bool l_Collides = Physics.Raycast(l_Ray, l_Distance, m_CollisionLayerMask.value);
        float l_DotAngle = Vector3.Dot(l_Direction, transform.forward);

        Debug.DrawRay(transform.position + Vector3.up * 0.5f, l_Direction * l_Distance, l_Collides ? Color.red : Color.yellow);
        return !l_Collides && l_DotAngle > Mathf.Cos(m_ConeAngle * 0.5f * Mathf.Deg2Rad);
    }

}
