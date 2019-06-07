using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_TargetCamera_Offset_Controller : MonoBehaviour
{
    public GameObject offsetTarget;
    public float x_MaxOffset_Standard;
    public float y_MaxOffset_Standard;
    public float x_MaxOffset_Pushing;
    public float y_MaxOffset_Pushing;

    public float smoothness;
    public float distanceTreshold;
    public float retractCoolDown;
    public float combatDistance;

    public Vector3 desiredPos;
    public float count;
    public float actual_X_MaxOffset;
    public float actual_Y_MaxOffset;

    public enum TargetCamera_Offset_State { STANDARD, COMBAT, PLAYER_DEATH, TRANSLATE_TO_TARGET};
    public TargetCamera_Offset_State actualState;
    [SerializeField]
    private Vector3 actualPlayerVelocity;

    void Start()
    {
        desiredPos = PlayerController.instance.gameObject.transform.position;
        count = 0;
        actual_X_MaxOffset = x_MaxOffset_Standard;
        actual_Y_MaxOffset = y_MaxOffset_Standard;
        ChangeState(TargetCamera_Offset_State.TRANSLATE_TO_TARGET);
    }

    void Update()
    {
        SetActualVelocityInp();
        switch (actualState)
        {
            case TargetCamera_Offset_State.STANDARD:
                if (PlayerController.instance.deathByFall) ChangeState(TargetCamera_Offset_State.PLAYER_DEATH);
                if (PlayerSensSystem.instance.nearestEnemy != null && PlayerSensSystem.instance.nearestEnemy.chasing && GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerSensSystem.instance.nearestEnemy.transform.position, PlayerController.instance.transform.position) < combatDistance)
                {
                    ChangeState(TargetCamera_Offset_State.COMBAT);
                }
                if (PlayerController.instance.pushing || PlayerController.instance.inSlowMovement)
                {
                    actual_X_MaxOffset = x_MaxOffset_Pushing;
                    actual_Y_MaxOffset = y_MaxOffset_Pushing;
                }
                else
                {
                    actual_X_MaxOffset = x_MaxOffset_Standard;
                    actual_Y_MaxOffset = y_MaxOffset_Standard;
                }
                Debug.Log("Player Velocity = " + PlayerController.instance.p_controller.velocity);
                if (!PlayerController.instance.dashing)
                {
                    if (actualPlayerVelocity.z > distanceTreshold)
                    {
                        desiredPos.z = Mathf.Lerp(desiredPos.z, PlayerController.instance.gameObject.transform.position.z + actual_Y_MaxOffset * (Mathf.Abs(PlayerController.instance.p_controller.velocity.z) / PlayerController.instance.movementSpeed), Time.deltaTime * (smoothness * (Mathf.Abs(PlayerController.instance.p_controller.velocity.z) / PlayerController.instance.movementSpeed)));
                    }
                    else
                    if (actualPlayerVelocity.z < -distanceTreshold)
                    {
                        desiredPos.z = Mathf.Lerp(desiredPos.z, PlayerController.instance.gameObject.transform.position.z - actual_Y_MaxOffset * (Mathf.Abs(PlayerController.instance.p_controller.velocity.z) / PlayerController.instance.movementSpeed), Time.deltaTime * (smoothness * (Mathf.Abs(PlayerController.instance.p_controller.velocity.z) / PlayerController.instance.movementSpeed)));
                    }
                    else
                    {
                        if (count <= 0 || actualPlayerVelocity.x != 0)
                        {
                            desiredPos.z = Mathf.Lerp(desiredPos.z, PlayerController.instance.transform.position.z, Time.deltaTime * (smoothness / 2));
                        }
                    }
                    if (actualPlayerVelocity.x > distanceTreshold)
                    {
                        desiredPos.x = Mathf.Lerp(desiredPos.x, PlayerController.instance.gameObject.transform.position.x + actual_X_MaxOffset * (Mathf.Abs(PlayerController.instance.p_controller.velocity.x) / PlayerController.instance.movementSpeed), Time.deltaTime * (smoothness * (Mathf.Abs(PlayerController.instance.p_controller.velocity.x) / PlayerController.instance.movementSpeed)));
                    }
                    else
                        if (actualPlayerVelocity.x < -distanceTreshold)
                    {
                        desiredPos.x = Mathf.Lerp(desiredPos.x, PlayerController.instance.gameObject.transform.position.x - actual_X_MaxOffset * (Mathf.Abs(PlayerController.instance.p_controller.velocity.x) / PlayerController.instance.movementSpeed), Time.deltaTime * (smoothness * (Mathf.Abs(PlayerController.instance.p_controller.velocity.x) / PlayerController.instance.movementSpeed)));
                    }
                    else
                    {
                        if (count <= 0 || actualPlayerVelocity.z != 0)
                        {
                            desiredPos.x = Mathf.Lerp(desiredPos.x, PlayerController.instance.transform.position.x, Time.deltaTime * (smoothness / 2));
                        }
                    }
                    desiredPos.y = Mathf.Lerp(desiredPos.y, PlayerController.instance.transform.position.y, Time.deltaTime * smoothness);

                    if (PlayerController.instance.p_controller.velocity.z < distanceTreshold && PlayerController.instance.p_controller.velocity.z > -distanceTreshold && PlayerController.instance.p_controller.velocity.x < distanceTreshold && PlayerController.instance.p_controller.velocity.x > -distanceTreshold)
                    {
                        if (count >= 0)
                        {
                            count -= Time.deltaTime;
                        }
                        else
                        {
                            count = 0;
                        }
                    }
                    else
                    {
                        if (count != retractCoolDown) count = retractCoolDown;
                    }
                }
                else
                {
                    desiredPos = PlayerController.instance.gameObject.transform.position;
                }
                break;
            case TargetCamera_Offset_State.COMBAT:
                if (PlayerController.instance.deathByFall) ChangeState(TargetCamera_Offset_State.PLAYER_DEATH);
                if (PlayerSensSystem.instance.nearestEnemy == null || !PlayerSensSystem.instance.nearestEnemy.chasing || GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerSensSystem.instance.nearestEnemy.transform.position, PlayerController.instance.transform.position) > combatDistance)
                {
                    ChangeState(TargetCamera_Offset_State.STANDARD);
                }
                float nearestEnemyDistance = GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerSensSystem.instance.nearestEnemy.transform.position, PlayerController.instance.transform.position);
                desiredPos = Vector3.Lerp(desiredPos, PlayerController.instance.transform.position + GenericSensUtilities.instance.GetDirectionFromTo_N(PlayerController.instance.transform.position, PlayerSensSystem.instance.nearestEnemy.transform.position) * (nearestEnemyDistance / 2), Time.deltaTime * smoothness);
                break;
            case TargetCamera_Offset_State.PLAYER_DEATH:
                desiredPos = GameManager.instance.levelCheckPoint.transform.position;
                if (!PlayerController.instance.deathByFall) ChangeState(TargetCamera_Offset_State.STANDARD);
                break;
            case TargetCamera_Offset_State.TRANSLATE_TO_TARGET:
                transform.position = desiredPos;
                if (transform.position == desiredPos) ChangeState(TargetCamera_Offset_State.STANDARD);                
                break;
               
        }
        offsetTarget.transform.position = Vector3.Lerp(offsetTarget.transform.position, desiredPos, Time.deltaTime * smoothness);
    }
    public void ChangeState(TargetCamera_Offset_State newState)
    {
        switch (actualState)
        {
            case TargetCamera_Offset_State.STANDARD:
                break;
            case TargetCamera_Offset_State.COMBAT:
                break;
            case TargetCamera_Offset_State.PLAYER_DEATH:
                count = 0;
                break;
            case TargetCamera_Offset_State.TRANSLATE_TO_TARGET:
                break;
        }
        switch (newState)
        {
            case TargetCamera_Offset_State.STANDARD:
                break;
            case TargetCamera_Offset_State.COMBAT:
                break;
            case TargetCamera_Offset_State.PLAYER_DEATH:
                break;
            case TargetCamera_Offset_State.TRANSLATE_TO_TARGET:
                break;
        }
        actualState = newState;
    }
    private void SetActualVelocityInp()
    {
        //actualVelocity = Vector3.Lerp(actualVelocity, PlayerController.instance.p_controller.velocity, Time.deltaTime * smoothness);
        actualPlayerVelocity = PlayerController.instance.p_controller.velocity;
    }
}
