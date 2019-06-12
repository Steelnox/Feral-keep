using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : State
{

    private float timerAttack;

    private bool dmg_done;

    private Melee melee;

    private Vector3 directionAttack;

    private Vector3 startPosition;
    private Vector3 endPosition;

    public float dashLength;

    private bool dashDone;


    public override void Enter()
    {

        melee = GetComponent<Melee>();

        directionAttack = (melee.enemy_navmesh.transform.position - melee.player.transform.position).normalized;

        timerAttack = 0;

        dashDone = false;

        startPosition = transform.position;

        endPosition = startPosition + (-directionAttack * dashLength);


        dmg_done = false;

        // melee.enemy_animator.enabled = false;

        melee.enemy_animator.SetBool("Attack", true);

        melee.finishAttack = false;

        melee.chasing = true;



    }

    public override void Execute()
    {
        timerAttack += Time.deltaTime;

        Quaternion lookOnLook = Quaternion.LookRotation(-directionAttack);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, 2f);
        

        melee.C_collider.enabled = false;


        //melee.enemy_navmesh.Move(transform.forward * 0.1f);
        if (!melee.finishAttack)
        {

            melee.enemy_navmesh.Move(GenericSensUtilities.instance.GetDirectionFromTo_N(startPosition, endPosition) * 0.05f);

            dashDone = true;
        }




        Collider[] entities = Physics.OverlapSphere(transform.position, 0.5f);
            
            
        foreach (Collider col in entities)
        {
                
            if (col.gameObject.tag == "Player" && !dmg_done)
            {
                AddDamage(); 
            }
        }
        

        if(melee.finishAttack)
        {
            FinishAttack();
        }
    }

    

    public override void Exit()
    {
        timerAttack = 0;
        dmg_done = false;

        melee.enemy_animator.SetBool("Attack", false);

        melee.C_collider.enabled = true;

    }

    #region Functions
   


    private void AddDamage()
    {
        //Debug.Log("dmgdone");
        melee.player.GetDamage(melee.damage);
        dmg_done = true;
    }

    private void FinishAttack()
    {
        melee.ChangeState(melee.rest);
    }
    #endregion
}
