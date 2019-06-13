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
        PlayerAnimationController.instance.SetDashing(true);
        //PlayerController.instance.SetCanMove(false);
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

        if (GenericSensUtilities.instance.DistanceBetween2Vectors(GenericSensUtilities.instance.Transform2DTo3DMovement(GenericSensUtilities.instance.Transform3DTo2DMovement(PlayerController.instance.transform.position)), GenericSensUtilities.instance.Transform2DTo3DMovement(GenericSensUtilities.instance.Transform3DTo2DMovement(endPosition))) < 0.1f 
            || (PlayerController.instance.p_controller.collisionFlags & CollisionFlags.Sides) != 0)
        {
            //if (PlayerSensSystem.instance.CheckGroundDistance() > PlayerController.instance.deathHeight)
            //{
            //    PlayerController.instance.fallingToDeath = true;
            //}
            //else
            //    if (PlayerSensSystem.instance.CheckGroundDistance() > 0.5f)
            //{
            //    PlayerController.instance.falling = true;
            //}
            if (PlayerController.instance.flyingDashFinished != true) PlayerController.instance.flyingDashFinished = true;
            PlayerAnimationController.instance.SetDashing(false);
            if (PlayerAnimationController.instance.finishAnimationController.GetDashArriveIsDone())
            {
                PlayerController.instance.ChangeState(PlayerController.instance.movementState);
            }
        }
        else
        {
            actualDashTime += Time.deltaTime;
            evaluateTime = actualDashTime / dashTime;
            evaluateTime = Mathf.Clamp(evaluateTime, 0, 1);
            //PlayerController.instance.transform.position = Vector3.Lerp(startPosition, endPosition, evaluateTime);
            PlayerController.instance.p_controller.Move(GenericSensUtilities.instance.GetDirectionFromTo_N(startPosition, endPosition) * 0.3f);
        }
        //actualDashTime += Time.deltaTime;
        //evaluateTime = actualDashTime / dashTime;
        //evaluateTime = Mathf.Clamp(evaluateTime, 0, 1);
        //PlayerController.instance.transform.position = Vector3.Lerp(startPosition, endPosition, evaluateTime);

        PlayerController.instance.imGrounded = PlayerController.instance.p_controller.isGrounded;
        DashTrail_Control.instance.SetTrailIOnScene(PlayerController.instance.characterModel.transform.position, PlayerController.instance.characterModel.transform.forward);
        PlayerParticlesSystemController.instance.GetDashTrailComposite().transform.position = PlayerController.instance.playerRoot.transform.position;
        //PlayerController.instance.ApplyGravity();
    }
    public override void Exit()
    {
        PlayerController.instance.p_controller.Move(Vector3.zero);
        dashDirection.x = 0;
        dashDirection.y = 0;
        dashDirection.z = 0;
        //PlayerController.instance.dashTrail.enabled = false;
        PlayerController.instance.dashing = false;
        PlayerController.instance.flyingDashFinished = false;
        //PlayerController.instance.SetCanMove(true);
        //PlayerParticlesSystemController.instance.SetDashParticlesOnScene(PlayerController.instance.transform.position);
        PlayerParticlesSystemController.instance.GetDashTrailComposite().StopComposite();
    }
}
