using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAtack_PlayerState : State
{

    public override void Enter()
    {
        PlayerController.instance.SetCanMove(false);
        PlayerController.instance.attacking = true;
        PlayerAnimationController.instance.AttackAnim();
        PlayerAnimationController.instance.finishAnimationController.StartAttack();
    }
    public override void Execute()
    {
        if (PlayerAnimationController.instance.finishAnimationController.GetAttackFinish()) PlayerController.instance.ChangeState(PlayerController.instance.movementState);
        PlayerController.instance.imGrounded = PlayerController.instance.p_controller.isGrounded;
        PlayerController.instance.p_controller.Move(Vector3.zero);
    }
    public override void Exit()
    {
        PlayerController.instance.SetCanMove(true);
        PlayerController.instance.attacking = false;
    }
}
