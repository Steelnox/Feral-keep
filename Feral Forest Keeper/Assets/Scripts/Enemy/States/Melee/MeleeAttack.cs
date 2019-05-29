using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : State
{

    private float timerAttack;

    private bool dmg_done;

    private Melee melee;

    private Vector3 directionAttack;

    private float timerDash;


    public override void Enter()
    {

        melee = GetComponent<Melee>();

        directionAttack = (melee.enemy_navmesh.transform.position - melee.player.transform.position).normalized;

        timerAttack = 0;

        timerDash = 0;

        dmg_done = false;

        // melee.enemy_animator.enabled = false;

        melee.enemy_animator.SetBool("Attack", true);

        melee.finishAttack = false;

    }

    public override void Execute()
    {
        timerAttack += Time.deltaTime;

        Quaternion lookOnLook = Quaternion.LookRotation(-directionAttack);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, 0.5f);



        if (!melee.finishAttack)
        {

            melee.C_collider.enabled = false;


            melee.enemy_navmesh.Move(-directionAttack * 0.1f);


            Collider[] entities = Physics.OverlapSphere(transform.position, 0.5f);
            
            
            foreach (Collider col in entities)
            {
                
                if (col.gameObject.tag == "Player" && !dmg_done)
                {
                    AddDamage(); 
                }
            }
        }

        else
        {
            FinishAttack();
        }
    }

    

    public override void Exit()
    {
        timerAttack = 0;
        timerDash = 0;
        dmg_done = false;

        melee.enemy_animator.SetBool("Attack", false);

        melee.C_collider.enabled = true;

    }

    #region Functions
   


    private void AddDamage()
    {
        Debug.Log("dmgdone");
        melee.player.GetDamage(melee.damage);
        dmg_done = true;
    }

    private void FinishAttack()
    {
        melee.ChangeState(melee.rest);
    }
    #endregion
}
