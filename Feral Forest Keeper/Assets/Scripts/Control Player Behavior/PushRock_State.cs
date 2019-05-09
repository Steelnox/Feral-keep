using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushRock_State : State
{
    [SerializeField]
    private Vector3 pushingMovement;
    private float constactDistance;
    private float actualContactDistance;
    private MovableRocks lastMovableRock;

    public override void Enter()
    {
        PlayerController.instance.pushing = true;
        PlayerController.instance.pushDirection = GenericSensUtilities.instance.Transform2DTo3DMovement(PlayerSensSystem.instance.nearestRock.CheckSideToPush()).normalized;
        PlayerController.instance.pushDirection = GenericSensUtilities.instance.Transform2DTo3DMovement(GenericSensUtilities.instance.Transform3DTo2DMovement(PlayerController.instance.pushDirection));
        PlayerController.instance.characterModel.transform.forward = PlayerController.instance.pushDirection;
        PlayerController.instance.imGrounded = true;
        PlayerSensSystem.instance.nearestRock.SetLastNoPushPosition(PlayerSensSystem.instance.nearestRock.transform.position);
        PlayerSensSystem.instance.nearestRock.SetBeingPushed(true);
        PlayerController.instance.MovingInSlowZone(true);
        constactDistance = GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerController.instance.transform.position, PlayerSensSystem.instance.nearestRock.FindContactPoint(PlayerController.instance.transform.position));
        lastMovableRock = PlayerSensSystem.instance.nearestRock;
    }
    public override void Execute()
    {
        actualContactDistance = GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerController.instance.transform.position, PlayerSensSystem.instance.nearestRock.FindContactPoint(PlayerController.instance.transform.position));
        
        Debug.Log("First Contact Distance = " + constactDistance);
        Debug.Log("Actual Contact Distance = " + GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerController.instance.transform.position, PlayerSensSystem.instance.nearestRock.FindContactPoint(PlayerController.instance.transform.position)));
        if (Input.GetButtonUp("RB") || Input.GetKeyUp(KeyCode.E) || PlayerSensSystem.instance.CheckGroundDistance() > 0.4f || PlayerSensSystem.instance.nearestRock.CheckIfFalling() || actualContactDistance < constactDistance - 0.1f || actualContactDistance > constactDistance + 0.1f || lastMovableRock != PlayerSensSystem.instance.nearestRock)
        {
            //Debug.Log("GroundDistance = " + PlayerSensSystem.instance.CheckGroundDistance());
            //Debug.Log("Exit PushRock State");
            PlayerController.instance.ChangeState(PlayerController.instance.movementState);
        }
        if (Input.GetButton("RB") || Input.GetKey(KeyCode.E))
        {
            //Debug.Log("pushDirection: " + pushDirection);
            //PlayerAnimationController.instance.SetPushinAnim(true);
            if (PlayerController.instance.pushDirection.z < 0)
            {
                Debug.Log("pushDirection.y < 0");
                PlayerSensSystem.instance.nearestRock.PushRock(PlayerController.instance.pushDirection * -PlayerController.instance.Z_Input);
                pushingMovement = (PlayerController.instance.pushDirection * -PlayerController.instance.Z_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestRock.weight);
                pushingMovement = new Vector3(pushingMovement.x, PlayerController.instance.movement.y, pushingMovement.z);
                PlayerController.instance.p_controller.Move(pushingMovement);
            }
            else
            if (PlayerController.instance.pushDirection.z > 0)
            {
                Debug.Log("pushDirection.y > 0");
                PlayerSensSystem.instance.nearestRock.PushRock(PlayerController.instance.pushDirection * PlayerController.instance.Z_Input);
                pushingMovement = (PlayerController.instance.pushDirection * PlayerController.instance.Z_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestRock.weight);
                pushingMovement = new Vector3(pushingMovement.x, PlayerController.instance.movement.y, pushingMovement.z);
                PlayerController.instance.p_controller.Move(pushingMovement);
            }

            if (PlayerController.instance.pushDirection.x < 0)
            {
                Debug.Log("pushDirection.x < 0");
                PlayerSensSystem.instance.nearestRock.PushRock(PlayerController.instance.pushDirection * -PlayerController.instance.X_Input);
                pushingMovement = (PlayerController.instance.pushDirection * -PlayerController.instance.X_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestRock.weight);
                pushingMovement = new Vector3(pushingMovement.x, PlayerController.instance.movement.y, pushingMovement.z);
                PlayerController.instance.p_controller.Move(pushingMovement);
            }
            else
            if (PlayerController.instance.pushDirection.x > 0)
            {
                Debug.Log("pushDirection.x > 0");
                PlayerSensSystem.instance.nearestRock.PushRock(PlayerController.instance.pushDirection * PlayerController.instance.X_Input);
                pushingMovement = (PlayerController.instance.pushDirection * PlayerController.instance.X_Input) * (Time.deltaTime / PlayerSensSystem.instance.nearestRock.weight);
                pushingMovement = new Vector3(pushingMovement.x, PlayerController.instance.movement.y, pushingMovement.z);
                PlayerController.instance.p_controller.Move(pushingMovement);
            }
        }
        //if (PlayerSensSystem.instance.nearestRock.GetLastNoPushingPosition().y != PlayerSensSystem.instance.nearestRock.transform.position.y)
        //{
        //    PlayerSensSystem.instance.nearestRock.SetLastNoPushPosition(PlayerSensSystem.instance.nearestRock.transform.position);
        //}
        Debug.Log("Push Direction = " + PlayerController.instance.pushDirection);
        Debug.Log("Rock-Grass Direction = " + GenericSensUtilities.instance.GetDirectionFromTo_N(PlayerSensSystem.instance.nearestRock.bodyMeshrenderer.bounds.ClosestPoint(PlayerSensSystem.instance.FindNearestGrassBushToPushingRock(PlayerSensSystem.instance.nearestRock).transform.position), PlayerSensSystem.instance.FindNearestGrassBushToPushingRock(PlayerSensSystem.instance.nearestRock).transform.position));
        Debug.Log("Player-Rock Direction = " + GenericSensUtilities.instance.GetDirectionFromTo_N(PlayerController.instance.playerRoot.transform.position, PlayerSensSystem.instance.nearestRock.bodyMeshrenderer.bounds.ClosestPoint(PlayerController.instance.transform.position)));
        Debug.DrawLine(PlayerSensSystem.instance.nearestRock.transform.position, PlayerSensSystem.instance.FindNearestGrassBushToPushingRock(PlayerSensSystem.instance.nearestRock).transform.position, Color.black);
        Debug.DrawLine(PlayerController.instance.playerRoot.transform.position, PlayerSensSystem.instance.nearestRock.transform.position, Color.yellow);
    }
    public override void Exit()
    {
        PlayerController.instance.no_Y_Input = false;
        PlayerController.instance.no_X_Input = false;
        PlayerController.instance.pushDirection = Vector3.zero;
        //PlayerAnimationController.instance.SetPushinAnim(false);
        //PlayerSensSystem.instance.nearestRock.ResetLastNoPushingPos();
        PlayerSensSystem.instance.nearestRock.SetBeingPushed(false);
        PlayerController.instance.MovingInSlowZone(false);
        PlayerController.instance.pushing = false;
    }
}
