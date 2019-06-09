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

    public float dashTime;
    public float dashLenght;

    private float actualDashTime;
    private float evaluateTime;
    private Vector3 startPosition;
    private Vector3 endPosition;

    public override void Enter()
    {
        count = dashLifeTime;
        PlayerController.instance.dashing = true;
        PlayerController.instance.SetCanMove(false);
        dashDirection = GenericSensUtilities.instance.Transform2DTo3DMovement(GenericSensUtilities.instance.Transform3DTo2DMovement(PlayerController.instance.characterModel.transform.forward)).normalized;
        dashDirection.y = 0;
        PlayerController.instance.dashCooldown = 0;
        PlayerController.instance.imGrounded = true;
        PlayerParticlesSystemController.instance.SetDashParticlesOnScene(PlayerController.instance.playerRoot.transform.position);
        PlayerParticlesSystemController.instance.SetDashDustTrailParticlesOnScene(PlayerController.instance.playerRoot.transform.position);
        PlayerAnimationController.instance.DashAnim();

        actualDashTime = 0;
        evaluateTime = 0;
        startPosition = PlayerController.instance.transform.position;
        endPosition = startPosition + (dashDirection * dashLenght);
    }
    public override void Execute()
    {
        //count -= Time.deltaTime;
        //if (count <= 0) PlayerController.instance.ChangeState(PlayerController.instance.movementState);

        //if (count < (count * 0.7f))
        //{
        //    acumulatedExpTime += Mathf.Exp(Time.deltaTime);
        //    PlayerController.instance.p_controller.Move(dashDirection * Mathf.Abs(dashForce - ((dashSmooth * 1000f) * acumulatedExpTime)) * Time.deltaTime);
        //}
        //else
        //{
        //    PlayerController.instance.p_controller.Move(dashDirection * dashForce * Time.deltaTime);
        //}
        //PlayerController.instance.p_controller.Move(dashDirection * Mathf.Abs(dashForce - (dashSmooth * Mathf.Exp(Time.deltaTime))) * Time.deltaTime);

        //if (PlayerAnimationController.instance.finishAnimationController.GetIsDone())
        //{
        //    PlayerController.instance.transform.position = PlayerAnimationController.instance.modelRootBone.transform.position;
        //    PlayerController.instance.ChangeState(PlayerController.instance.movementState);
        //}
        PlayerController.instance.p_controller.Move(Vector3.zero);

        if (GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerController.instance.transform.position, endPosition) < 0.1f 
            || (PlayerController.instance.p_controller.collisionFlags & CollisionFlags.Sides) != 0)
        {
            PlayerController.instance.ChangeState(PlayerController.instance.movementState);
        }
        actualDashTime += Time.deltaTime;
        evaluateTime = actualDashTime / dashTime;
        evaluateTime = Mathf.Clamp(evaluateTime, 0, 1);
        PlayerController.instance.transform.position = Vector3.Lerp(startPosition, endPosition, evaluateTime);

        PlayerController.instance.imGrounded = PlayerController.instance.p_controller.isGrounded;
        DashTrail_Control.instance.SetTrailIOnScene(PlayerController.instance.characterModel.transform.position, PlayerController.instance.characterModel.transform.forward);
        PlayerParticlesSystemController.instance.GetDashTrailComposite().transform.position = PlayerController.instance.playerRoot.transform.position;
        //if ((PlayerController.instance.p_controller.collisionFlags & CollisionFlags.CollidedSides) != 0)
        //{
        //    endPosition.y++;
        //}
        
    }
    public override void Exit()
    {
        dashDirection.x = 0;
        dashDirection.y = 0;
        dashDirection.z = 0;
        //PlayerController.instance.dashTrail.enabled = false;
        PlayerController.instance.dashing = false;
        //PlayerParticlesSystemController.instance.SetDashParticlesOnScene(PlayerController.instance.transform.position);
        PlayerParticlesSystemController.instance.GetDashTrailComposite().StopComposite();
    }
}
