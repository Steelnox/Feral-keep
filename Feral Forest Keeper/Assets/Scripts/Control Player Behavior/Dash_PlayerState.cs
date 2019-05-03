using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_PlayerState : State
{
    public float dashForce;
    public Vector3 dashDirection;
    public float dashLifeTime;
    public float dashSmooth;

    private float acumulatedExpTime;
    private float count;

    public override void Enter()
    {
        count = dashLifeTime;
        PlayerController.instance.dashing = true;
        dashDirection = GenericSensUtilities.instance.Transform2DTo3DMovement(PlayerController.instance.direction).normalized;
        dashDirection.y = 0;
        PlayerController.instance.dashTrail.enabled = true;
        PlayerController.instance.SpendDashCharge();
        PlayerController.instance.imGrounded = true;
    }
    public override void Execute()
    {
        count -= Time.deltaTime;
        if (count <= 0) PlayerController.instance.ChangeState(PlayerController.instance.movementState);

        if (count < (count * 0.7f))
        {
            acumulatedExpTime += Mathf.Exp(Time.deltaTime);
            PlayerController.instance.p_controller.Move(dashDirection * Mathf.Abs(dashForce - ((dashSmooth * 1000f) * acumulatedExpTime)) * Time.deltaTime);
        }
        else
        {
            PlayerController.instance.p_controller.Move(dashDirection * dashForce * Time.deltaTime);
        }
        //PlayerController.instance.p_controller.Move(dashDirection * Mathf.Abs(dashForce - (dashSmooth * Mathf.Exp(Time.deltaTime))) * Time.deltaTime);
        PlayerController.instance.imGrounded = PlayerController.instance.p_controller.isGrounded;
    }
    public override void Exit()
    {
        dashDirection.x = 0;
        dashDirection.y = 0;
        dashDirection.z = 0;
        PlayerController.instance.dashTrail.enabled = false;
        PlayerController.instance.dashing = false;
    }
}
