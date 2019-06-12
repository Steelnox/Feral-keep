using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAtack_PlayerState : State
{

    public override void Enter()
    {
        //PlayerController.instance.weaponCollider.enabled = true;
        //PlayerController.instance.attackTrail.startColor = new Color(1, 1, 0);
        //PlayerController.instance.attackTrail.endColor = new Color(1, 0, 1);
        PlayerController.instance.SetCanMove(false);
        PlayerController.instance.attacking = true;
        PlayerAnimationController.instance.AttackAnim();
        PlayerAnimationController.instance.finishAnimationController.StartAttack();
    }
    public override void Execute()
    {
        if (PlayerAnimationController.instance.finishAnimationController.GetAttackFinish()) PlayerController.instance.ChangeState(PlayerController.instance.movementState);
        PlayerController.instance.imGrounded = PlayerController.instance.p_controller.isGrounded;
    }
    public override void Exit()
    {
        //Debug.Log("Exit Atacking State");
        //PlayerController.instance.weaponCollider.enabled = false;
        PlayerController.instance.SetCanMove(true);
        PlayerController.instance.attacking = false;
    }
}
