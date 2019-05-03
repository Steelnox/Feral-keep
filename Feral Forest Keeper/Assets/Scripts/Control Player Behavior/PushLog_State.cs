using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushLog_State : State
{
    public Vector3 pushDirection;

    public override void Enter()
    {
        pushDirection = PlayerSensSystem.instance.nearestLog.transform.TransformDirection(GenericSensUtilities.instance.Transform2DTo3DMovement(PlayerSensSystem.instance.nearestLog.CheckSideToPush()).normalized);
        PlayerController.instance.characterModel.transform.forward = pushDirection;
        PlayerController.instance.imGrounded = true;
        PlayerSensSystem.instance.nearestLog.SetBeingPushed(true);
        PlayerController.instance.MovingInSlowZone(true);
    }
    public override void Execute()
    {
        if (Input.GetButtonUp("RB") || Input.GetKeyUp(KeyCode.E) || PlayerSensSystem.instance.CheckGroundDistance() > 0.2f /*|| PlayerSensSystem.instance.nearestLog.CheckIfFalling()*/)
        {
            PlayerController.instance.ChangeState(PlayerController.instance.movementState);
        }
        if (Input.GetButton("RB") || Input.GetKey(KeyCode.E))
        {
            //Debug.Log("pushDirection: " + pushDirection);

            if (pushDirection.z < 0)
            {
                //Debug.Log("pushDirection.y < 0");
                PlayerSensSystem.instance.nearestLog.PushLog(pushDirection * -PlayerController.instance.Z_Input);
                PlayerController.instance.p_controller.Move((pushDirection * -PlayerController.instance.Z_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestLog.weight));
            }
            if (pushDirection.z > 0)
            {
                //Debug.Log("pushDirection.y > 0");
                PlayerSensSystem.instance.nearestLog.PushLog(pushDirection * PlayerController.instance.Z_Input);
                PlayerController.instance.p_controller.Move((pushDirection * PlayerController.instance.Z_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestLog.weight));
            }
            if (pushDirection.x < 0)
            {
                PlayerSensSystem.instance.nearestLog.PushLog(pushDirection * -PlayerController.instance.X_Input);
                PlayerController.instance.p_controller.Move((pushDirection * -PlayerController.instance.X_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestLog.weight));
            }
            if (pushDirection.x > 0)
            {
                PlayerSensSystem.instance.nearestLog.PushLog(pushDirection * PlayerController.instance.X_Input);
                PlayerController.instance.p_controller.Move((pushDirection * PlayerController.instance.X_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestLog.weight));
            }
        }
    }
    public override void Exit()
    {
        PlayerSensSystem.instance.nearestLog.SetBeingPushed(false);
        PlayerController.instance.MovingInSlowZone(false);
    }
}
