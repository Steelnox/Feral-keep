using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushLog_State : State
{
    [SerializeField]
    private Vector3 pushingMovement;
    private int lastDistance;

    public override void Enter()
    {
        PlayerController.instance.pushDirection = GenericSensUtilities.instance.Transform2DTo3DMovement(PlayerSensSystem.instance.nearestLog.CheckSideToPush()).normalized;
        PlayerController.instance.pushDirection = GenericSensUtilities.instance.Transform2DTo3DMovement(GenericSensUtilities.instance.Transform3DTo2DMovement(PlayerController.instance.pushDirection));
        PlayerController.instance.characterModel.transform.forward = PlayerController.instance.pushDirection;
        PlayerController.instance.imGrounded = true;
        PlayerSensSystem.instance.nearestLog.SetLastNoPushPosition(PlayerSensSystem.instance.nearestLog.transform.position);
        PlayerSensSystem.instance.nearestLog.SetBeingPushed(true);
        PlayerController.instance.MovingInSlowZone(true);
        lastDistance = (int)GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, PlayerSensSystem.instance.nearestLog.FindContactPoint(PlayerController.instance.transform.position));
    }
    public override void Execute()
    {
        if (Input.GetButtonUp("RB") || Input.GetKeyUp(KeyCode.E) || PlayerSensSystem.instance.CheckGroundDistance() > 0.4f /*|| PlayerSensSystem.instance.nearestLog.CheckIfFalling()*/
            || (int)GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, PlayerSensSystem.instance.nearestLog.FindContactPoint(PlayerController.instance.transform.position)) != lastDistance)
        {
            PlayerController.instance.ChangeState(PlayerController.instance.movementState);
        }
        if (Input.GetButton("RB") || Input.GetKey(KeyCode.E))
        {
            //Debug.Log("pushDirection: " + pushDirection);
            PlayerAnimationController.instance.SetPushinAnim(true);
            if (PlayerController.instance.pushDirection.z < 0)
            {
                //Debug.Log("pushDirection.y < 0");
                PlayerSensSystem.instance.nearestLog.PushLog(PlayerController.instance.pushDirection * -PlayerController.instance.Z_Input);
                pushingMovement = (PlayerController.instance.pushDirection * -PlayerController.instance.Z_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestLog.weight);
                pushingMovement = new Vector3(pushingMovement.x, PlayerController.instance.movement.y, pushingMovement.z);
                PlayerController.instance.p_controller.Move(pushingMovement);
            }
            if (PlayerController.instance.pushDirection.z > 0)
            {
                //Debug.Log("pushDirection.y > 0");
                PlayerSensSystem.instance.nearestLog.PushLog(PlayerController.instance.pushDirection * PlayerController.instance.Z_Input);
                pushingMovement = (PlayerController.instance.pushDirection * PlayerController.instance.Z_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestLog.weight);
                pushingMovement = new Vector3(pushingMovement.x, PlayerController.instance.movement.y, pushingMovement.z);
                PlayerController.instance.p_controller.Move(pushingMovement);
            }
            if (PlayerController.instance.pushDirection.x < 0)
            {
                PlayerSensSystem.instance.nearestLog.PushLog(PlayerController.instance.pushDirection * -PlayerController.instance.X_Input);
                pushingMovement = (PlayerController.instance.pushDirection * -PlayerController.instance.X_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestLog.weight);
                pushingMovement = new Vector3(pushingMovement.x, PlayerController.instance.movement.y, pushingMovement.z);
                PlayerController.instance.p_controller.Move(pushingMovement);
            }
            if (PlayerController.instance.pushDirection.x > 0)
            {
                PlayerSensSystem.instance.nearestLog.PushLog(PlayerController.instance.pushDirection * PlayerController.instance.X_Input);
                pushingMovement = (PlayerController.instance.pushDirection * PlayerController.instance.X_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestLog.weight);
                pushingMovement = new Vector3(pushingMovement.x, PlayerController.instance.movement.y, pushingMovement.z);
                PlayerController.instance.p_controller.Move(pushingMovement);
            }
        }
        //if (PlayerSensSystem.instance.nearestLog.GetLastNoPushingPosition().y != PlayerSensSystem.instance.nearestLog.transform.position.y)
        //{
        //    PlayerSensSystem.instance.nearestLog.SetLastNoPushPosition(PlayerSensSystem.instance.nearestLog.transform.position);
        //}
    }
    public override void Exit()
    {
        PlayerController.instance.no_Y_Input = false;
        PlayerController.instance.no_X_Input = false;
        PlayerController.instance.pushDirection = Vector3.zero;
        PlayerAnimationController.instance.SetPushinAnim(false);
        //PlayerSensSystem.instance.nearestLog.ResetLastNoPushingPos();
        PlayerSensSystem.instance.nearestLog.SetBeingPushed(false);
        PlayerController.instance.MovingInSlowZone(false);
    }
}
