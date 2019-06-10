using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_PlayerState : State
{

    public override void Enter()
    {

    }
    public override void Execute()
    {
        CameraController.instance.SetFieldOfViewSmoothness(2.0f, CameraController.instance.standardFOV);
        if (PlayerController.instance.playerRoot.transform.forward != PlayerController.instance.modelForwardDirection)
        {
            PlayerController.instance.playerRoot.transform.forward = Vector3.Slerp(PlayerController.instance.playerRoot.transform.forward, PlayerController.instance.modelForwardDirection, Time.deltaTime * PlayerController.instance.smooth);
        }
        CameraController.instance.p_Camera.transform.rotation = Quaternion.Lerp(CameraController.instance.p_Camera.transform.rotation, CameraController.instance.initCameraRotation, CameraController.instance.smoothValue * Time.deltaTime);
        //Debug.Log("Player Velocity: " + movement.magnitude);
        PlayerController.instance.p_controller.Move(PlayerController.instance.movement * Time.deltaTime);
        PlayerController.instance.imGrounded = PlayerController.instance.p_controller.isGrounded;
    }
    public override void Exit()
    {

    }
}