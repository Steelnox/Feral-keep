using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushLog_State : State
{
    //public Vector3 pushDirection;

    public override void Enter()
    {
        PlayerController.instance.pushDirection = PlayerSensSystem.instance.nearestLog.transform.TransformDirection(GenericSensUtilities.instance.Transform2DTo3DMovement(PlayerSensSystem.instance.nearestLog.CheckSideToPush()).normalized);
        PlayerController.instance.characterModel.transform.forward = PlayerController.instance.pushDirection;
        PlayerController.instance.imGrounded = true;
        PlayerSensSystem.instance.nearestLog.SetLastNoPushPosition(PlayerSensSystem.instance.nearestLog.transform.position);
        PlayerSensSystem.instance.nearestLog.SetBeingPushed(true);
        PlayerController.instance.MovingInSlowZone(true);
    }
    public override void Execute()
    {
        if (Input.GetButtonUp("RB") || Input.GetKeyUp(KeyCode.E) || PlayerSensSystem.instance.CheckGroundDistance() > 0.4f /*|| PlayerSensSystem.instance.nearestLog.CheckIfFalling()*/)
        {
            PlayerController.instance.ChangeState(PlayerController.instance.movementState);
        }
        if (Input.GetButton("RB") || Input.GetKey(KeyCode.E))
        {
            //Debug.Log("pushDirection: " + pushDirection);

            if (PlayerController.instance.pushDirection.z < 0)
            {
                //Debug.Log("pushDirection.y < 0");
                PlayerSensSystem.instance.nearestLog.PushLog(PlayerController.instance.pushDirection * -PlayerController.instance.Z_Input);
                PlayerController.instance.p_controller.Move((PlayerController.instance.pushDirection * -PlayerController.instance.Z_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestLog.weight));
            }
            if (PlayerController.instance.pushDirection.z > 0)
            {
                //Debug.Log("pushDirection.y > 0");
                PlayerSensSystem.instance.nearestLog.PushLog(PlayerController.instance.pushDirection * PlayerController.instance.Z_Input);
                PlayerController.instance.p_controller.Move((PlayerController.instance.pushDirection * PlayerController.instance.Z_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestLog.weight));
            }
            if (PlayerController.instance.pushDirection.x < 0)
            {
                PlayerSensSystem.instance.nearestLog.PushLog(PlayerController.instance.pushDirection * -PlayerController.instance.X_Input);
                PlayerController.instance.p_controller.Move((PlayerController.instance.pushDirection * -PlayerController.instance.X_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestLog.weight));
            }
            if (PlayerController.instance.pushDirection.x > 0)
            {
                PlayerSensSystem.instance.nearestLog.PushLog(PlayerController.instance.pushDirection * PlayerController.instance.X_Input);
                PlayerController.instance.p_controller.Move((PlayerController.instance.pushDirection * PlayerController.instance.X_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestLog.weight));
            }
        }
    }
    public override void Exit()
    {
        PlayerController.instance.no_Y_Input = false;
        PlayerController.instance.no_X_Input = false;
        PlayerController.instance.pushDirection = Vector3.zero;
        PlayerAnimationController.instance.SetPushinAnim(false);
        PlayerSensSystem.instance.nearestLog.ResetLastNoPushingPos();
        PlayerSensSystem.instance.nearestLog.SetBeingPushed(false);
        PlayerController.instance.MovingInSlowZone(false);
    }
}
