using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushRock_State : State
{
    //public Vector3 pushDirection;

    public override void Enter()
    {
        PlayerController.instance.pushDirection = PlayerSensSystem.instance.nearestRock.transform.TransformDirection(GenericSensUtilities.instance.Transform2DTo3DMovement(PlayerSensSystem.instance.nearestRock.CheckSideToPush()).normalized);
        PlayerController.instance.characterModel.transform.forward = PlayerController.instance.pushDirection;
        PlayerController.instance.imGrounded = true;
        PlayerSensSystem.instance.nearestRock.SetLastNoPushPosition(PlayerSensSystem.instance.nearestRock.transform.position);
        PlayerSensSystem.instance.nearestRock.SetBeingPushed(true);
        PlayerController.instance.MovingInSlowZone(true);
    }
    public override void Execute()
    {
        
        if (Input.GetButtonUp("RB") || Input.GetKeyUp(KeyCode.E) || PlayerSensSystem.instance.CheckGroundDistance() > 0.4f || PlayerSensSystem.instance.nearestRock.CheckIfFalling())
        {
            //Debug.Log("GroundDistance = " + PlayerSensSystem.instance.CheckGroundDistance());
            //Debug.Log("Exit PushRock State");
            PlayerController.instance.ChangeState(PlayerController.instance.movementState);
        }
        if (Input.GetButton("RB") || Input.GetKey(KeyCode.E))
        {
            //Debug.Log("pushDirection: " + pushDirection);
            PlayerAnimationController.instance.SetPushinAnim(true);
            if (PlayerController.instance.pushDirection.z < 0)
            {
                Debug.Log("pushDirection.y < 0");
                PlayerSensSystem.instance.nearestRock.PushRock(PlayerController.instance.pushDirection * -PlayerController.instance.Z_Input);
                PlayerController.instance.p_controller.Move((PlayerController.instance.pushDirection * -PlayerController.instance.Z_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestRock.weight));
            }
            else
            if (PlayerController.instance.pushDirection.z > 0)
            {
                Debug.Log("pushDirection.y > 0");
                PlayerSensSystem.instance.nearestRock.PushRock(PlayerController.instance.pushDirection * PlayerController.instance.Z_Input);
                PlayerController.instance.p_controller.Move((PlayerController.instance.pushDirection * PlayerController.instance.Z_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestRock.weight));
            }
            
            if (PlayerController.instance.pushDirection.x < 0)
            {
                Debug.Log("pushDirection.x < 0");
                PlayerSensSystem.instance.nearestRock.PushRock(PlayerController.instance.pushDirection * -PlayerController.instance.X_Input);
                PlayerController.instance.p_controller.Move((PlayerController.instance.pushDirection * -PlayerController.instance.X_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestRock.weight));
            }
            else
            if (PlayerController.instance.pushDirection.x > 0)
            {
                Debug.Log("pushDirection.x > 0");
                PlayerSensSystem.instance.nearestRock.PushRock(PlayerController.instance.pushDirection * PlayerController.instance.X_Input);
                PlayerController.instance.p_controller.Move((PlayerController.instance.pushDirection * PlayerController.instance.X_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestRock.weight));
            }

        }
        if (PlayerSensSystem.instance.nearestRock.GetLastNoPushingPosition().y != PlayerSensSystem.instance.nearestRock.transform.position.y)
        {
            PlayerSensSystem.instance.nearestRock.SetLastNoPushPosition(PlayerSensSystem.instance.nearestRock.transform.position);
        }
    }
    public override void Exit()
    {
        PlayerController.instance.no_Y_Input = false;
        PlayerController.instance.no_X_Input = false;
        PlayerController.instance.pushDirection = Vector3.zero;
        PlayerAnimationController.instance.SetPushinAnim(false);
        PlayerSensSystem.instance.nearestRock.ResetLastNoPushingPos();
        PlayerSensSystem.instance.nearestRock.SetBeingPushed(false);
        PlayerController.instance.MovingInSlowZone(false);
    }
}
