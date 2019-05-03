using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushRock_State : State
{
    public Vector3 pushDirection;

    public override void Enter()
    {
        pushDirection = PlayerSensSystem.instance.nearestRock.transform.TransformDirection(GenericSensUtilities.instance.Transform2DTo3DMovement(PlayerSensSystem.instance.nearestRock.CheckSideToPush()).normalized);
        PlayerController.instance.characterModel.transform.forward = pushDirection;
        PlayerController.instance.imGrounded = true;
        PlayerSensSystem.instance.nearestRock.SetBeingPushed(true);
        PlayerController.instance.MovingInSlowZone(true);
    }
    public override void Execute()
    {
        
        if (Input.GetButtonUp("RB") || Input.GetKeyUp(KeyCode.E) || PlayerSensSystem.instance.CheckGroundDistance() > 0.4f || PlayerSensSystem.instance.nearestRock.CheckIfFalling())
        {
            Debug.Log("GroundDistance = " + PlayerSensSystem.instance.CheckGroundDistance());
            Debug.Log("Exit PushRock State");
            PlayerController.instance.ChangeState(PlayerController.instance.movementState);
        }
        if (Input.GetButton("RB") || Input.GetKey(KeyCode.E))
        {
            //Debug.Log("pushDirection: " + pushDirection);

            if (pushDirection.z < 0)
            {
                //Debug.Log("pushDirection.y < 0");
                PlayerSensSystem.instance.nearestRock.PushRock(pushDirection * -PlayerController.instance.Z_Input);
                PlayerController.instance.p_controller.Move((pushDirection * -PlayerController.instance.Z_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestRock.weight));
            }
            if (pushDirection.z > 0)
            {
                //Debug.Log("pushDirection.y > 0");
                PlayerSensSystem.instance.nearestRock.PushRock(pushDirection * PlayerController.instance.Z_Input);
                PlayerController.instance.p_controller.Move((pushDirection * PlayerController.instance.Z_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestRock.weight));
            }
            if (pushDirection.x < 0)
            {
                PlayerSensSystem.instance.nearestRock.PushRock(pushDirection * -PlayerController.instance.X_Input);
                PlayerController.instance.p_controller.Move((pushDirection * -PlayerController.instance.X_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestRock.weight));
            }
            if (pushDirection.x > 0)
            {
                PlayerSensSystem.instance.nearestRock.PushRock(pushDirection * PlayerController.instance.X_Input);
                PlayerController.instance.p_controller.Move((pushDirection * PlayerController.instance.X_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestRock.weight));
            }
        }
    }
    public override void Exit()
    {
        PlayerSensSystem.instance.nearestRock.SetBeingPushed(false);
        PlayerController.instance.MovingInSlowZone(false);
    }
}
