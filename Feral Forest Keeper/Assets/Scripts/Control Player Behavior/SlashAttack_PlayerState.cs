using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack_PlayerState : State
{

    public override void Enter()
    {
        //if (!PlayerController.instance.useWeaponColllider)PlayerController.instance.weaponCollider.enabled = true;
        PlayerController.instance.attackTrail.startColor = new Color(1,0,0);
        PlayerController.instance.attackTrail.endColor = new Color(0, 0, 1);
        PlayerController.instance.RangedAttack();
        //PlayerController.instance.attackTrail.enabled = true;
        PlayerController.instance.attacking = true;
        PlayerAnimationController.instance.AttackAnim();
    }
    public override void Execute()
    {
        if (!PlayerAnimationController.instance.IsAnimationPlaying("Attack")) PlayerController.instance.ChangeState(PlayerController.instance.movementState);
        PlayerController.instance.imGrounded = PlayerController.instance.p_controller.isGrounded;
    }
    public override void Exit()
    {
        //if (!PlayerController.instance.useWeaponColllider) PlayerController.instance.weaponCollider.enabled = false;
        //PlayerController.instance.attackTrail.enabled = false;
        PlayerController.instance.attacking = false;
    }
}
