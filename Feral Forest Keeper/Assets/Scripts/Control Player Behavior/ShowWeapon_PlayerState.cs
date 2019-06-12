using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWeapon_PlayerState : State
{
    public float showingTime;
    [SerializeField]
    private Vector3 showingWeaponInitForward;
    [SerializeField]
    private Vector3 showingDirection;
    private float showingWeaponCount;

    public override void Enter()
    {
        PlayerController.instance.showingWeapon = true;
        PlayerController.instance.SetCanMove(false);
        showingWeaponInitForward = PlayerController.instance.playerRoot.transform.forward;
        showingDirection = GenericSensUtilities.instance.Transform2DTo3DMovement(GenericSensUtilities.instance.Transform3DTo2DMovement(GenericSensUtilities.instance.GetDirectionFromTo_N(transform.position, CameraController.instance.transform.position)));
        CameraController.instance.SetActualBehavior(CameraController.Behavior.PLAYER_SHOW_WEAPON);
    }
    public override void Execute()
    {
        showingWeaponCount += Time.deltaTime;

        if (showingWeaponCount > showingTime)
        {
            PlayerController.instance.ChangeState(PlayerController.instance.movementState);
        }
        else
        {
            float time = showingWeaponCount / 0.5f;
            PlayerController.instance.playerRoot.transform.forward = Vector3.Lerp(showingWeaponInitForward, showingDirection, time);
        }
        PlayerController.instance.p_controller.Move(Vector3.zero);
    }
    public override void Exit()
    {
        CameraController.instance.SetActualBehavior(CameraController.Behavior.TRANSITION_TO_FOLLOW);
        showingWeaponCount = 0;
        PlayerController.instance.SetCanMove(true);
        PlayerController.instance.showingWeapon = false;
    }
}
