using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLock_PlayerState : State
{
    public bool attack;
    public Vector3 attackSpot;

    public override void Enter()
    {
        CameraController.instance.player = PlayerController.instance.characterModel;
        PlayerController.instance.targetLocked = PlayerSensSystem.instance.nearestEnemy.gameObject;
        PlayerController.instance.actualSpeedMultipler = PlayerController.instance.movementLockTargetSpeed;
        PlayerAnimationController.instance.SetTargetLockAnim(true);
        PlayerController.instance.actualSpeedMultipler = PlayerController.instance.movementLockTargetSpeed;
    }
    public override void Execute()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("X"))
        {
            attack = true;
            attackSpot = PlayerController.instance.targetLocked.transform.position;
            PlayerController.instance.SetCanMove(false);
        }
        if (Input.GetButtonUp("LB") || Input.GetKeyUp(KeyCode.LeftShift) || PlayerSensSystem.instance.onRangeEnemyList.Count == 0 || 
            GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, PlayerController.instance.targetLocked.transform.position) > PlayerController.instance.lockTargetDistance ||
            PlayerSensSystem.instance.nearestEnemy == null || !Input.anyKey)
        {
            PlayerController.instance.ChangeState(PlayerController.instance.movementState);
            //TargetLockVisualFeedbackSystem.instance.ChangeMode(TargetLockVisualFeedbackSystem.TargetFeedback_Mode.NONE_MODE);
            return;
        }

        if (PlayerController.instance.targetLocked.transform.position.z > CameraController.instance.p_Camera.transform.position.z)
        {
            Vector3 middlePoint = (PlayerController.instance.targetLocked.transform.position + PlayerController.instance.playerRoot.transform.position) / 2;
            CameraController.instance.FaceTarget(middlePoint);
        }
        CameraController.instance.SetFieldOfViewSmoothness(10.0f, CameraController.instance.lockTargetFOV);

        if (!attack)
        {
            Vector3 targetLockedXZVector3 = GenericSensUtilities.instance.Transform2DTo3DMovement(GenericSensUtilities.instance.Transform3DTo2DMovement(PlayerController.instance.targetLocked.transform.position));
            Vector3 thisPositionXZVector3 = GenericSensUtilities.instance.Transform2DTo3DMovement(GenericSensUtilities.instance.Transform3DTo2DMovement(PlayerController.instance.transform.position));
            PlayerController.instance.playerRoot.transform.forward = Vector3.Lerp(PlayerController.instance.playerRoot.transform.forward, GenericSensUtilities.instance.GetDirectionFromTo_N(thisPositionXZVector3, targetLockedXZVector3), PlayerController.instance.smooth * Time.deltaTime);

            PlayerController.instance.p_controller.Move(PlayerController.instance.movement * Time.deltaTime);
        }
        else if (attack)
        {
            PlayerController.instance.noInput = true;
            if (!PlayerAnimationController.instance.IsAnimationPlaying("Base.LockedTargetAttackAnim"))
            {
                PlayerController.instance.weaponCollider.enabled = true;
                PlayerAnimationController.instance.AttackAnim();
            }
            //PlayerController.instance.playerRoot.transform.forward = Vector3.Lerp(PlayerController.instance.playerRoot.transform.forward, attackSpot, PlayerController.instance.smooth * Time.deltaTime);
        }

        if (attack && !PlayerAnimationController.instance.IsAnimationPlaying("Base.LockedTargetAttackAnim"))
        {
            PlayerController.instance.SetCanMove(true);
            PlayerController.instance.weaponCollider.enabled = false;
            PlayerController.instance.noInput = false;
            attack = false;
        }

        /*if (TargetLockVisualFeedbackSystem.instance.actualMode != TargetLockVisualFeedbackSystem.TargetFeedback_Mode.LOCKED_TARGET)
        {
            //TargetLockVisualFeedbackSystem.instance.ChangeMode(TargetLockVisualFeedbackSystem.TargetFeedback_Mode.LOCKED_TARGET);
        }*/
    }
    public override void Exit()
    {
        //TargetLockVisualFeedbackSystem.instance.ChangeMode(TargetLockVisualFeedbackSystem.TargetFeedback_Mode.NONE_MODE);
        PlayerController.instance.actualSpeedMultipler = PlayerController.instance.movementSpeed;
        PlayerController.instance.targetLocked = null;
        PlayerAnimationController.instance.SetTargetLockAnim(false);
        CameraController.instance.player = PlayerController.instance.playerRoot;
    }
}
