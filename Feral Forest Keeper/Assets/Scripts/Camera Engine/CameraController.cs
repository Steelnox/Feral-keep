﻿/*using System.Collections;
using System.Collections.Generic;*/
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Singleton
    public static CameraController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }
    #endregion

    public Camera p_Camera;
    public GameObject target;
    public Vector3 cameraOffSet;
    public float smoothValue;
    public float standardFOV;
    public float lockTargetFOV;
    public float pushFOV;
    public float largeFOV;
    public Quaternion initCameraRotation;
    public float scriptedCooldownTime;

    public enum Behavior {FOLLOW_PLAYER, CHANGE_LEVEL, SCRIPT_MOVEMENT, PLAYER_DEATH, STATIC_CAMERA_ZONE, TRANSITION_TO_FOLLOW, PLAYER_SHOW_WEAPON};
    [SerializeField]
    private Behavior actualBehavior;
    private Vector3 desiredPosition;
    private Vector3 cameraMovement;
    private Vector2 cameraMovement2Axis;
    private Vector2 desiredPosition2D;
    private Vector2 relativeCameraMovementVector;
    [SerializeField]
    private GameObject scriptedTarget;
    private float actualScriptedCooldown;
    private Vector3 frontBackTravellingSliderVector;
    [SerializeField]
    //private float deathCount;
    private float scriptedHighDistance;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float time;
    private float blendTime;
    
    void Start ()
    {
        desiredPosition = target.transform.position + cameraOffSet;
        transform.position = desiredPosition;
        SetActualBehavior(Behavior.FOLLOW_PLAYER);
        initCameraRotation = p_Camera.transform.rotation;
        p_Camera.fieldOfView = standardFOV;
        actualScriptedCooldown = scriptedCooldownTime;
        //deathCount = 0;
        blendTime = 0;
	}
	
	void Update ()
    {
        switch (actualBehavior)
        {
            case Behavior.FOLLOW_PLAYER:
                //FaceTarget();
                FollowTarget(target);
                break;
            case Behavior.CHANGE_LEVEL:
                desiredPosition = target.transform.position + cameraOffSet;
                cameraMovement = Vector3.Slerp(cameraMovement, desiredPosition, (smoothValue / 2) * Time.deltaTime);
                p_Camera.transform.position = cameraMovement;
                //Debug.Log("Camera Movement = " + cameraMovement);
                //Debug.Log("Desired Position = " + desiredPosition);
                if ((cameraMovement - desiredPosition).magnitude < 0.2f) SetActualBehavior(Behavior.FOLLOW_PLAYER);
                break;
            case Behavior.SCRIPT_MOVEMENT:
                actualScriptedCooldown -= Time.deltaTime;
                if (actualScriptedCooldown <= 0)
                {
                    PlayerController.instance.SetCanMove(true);
                    actualScriptedCooldown = scriptedCooldownTime;
                    SetActualBehavior(Behavior.FOLLOW_PLAYER);
                    break;
                }
                PlayerController.instance.SetCanMove(false);
                PlayerController.instance.ChangeState(PlayerController.instance.movementState);
                FollowTarget(scriptedTarget);
                break;
            case Behavior.PLAYER_DEATH:
                //deathCount += Time.deltaTime;
                time += Time.deltaTime;
                blendTime = time / GameManager.instance.respawnCoolDown;
                endPosition = startPosition + (frontBackTravellingSliderVector * 0.5f);
                //transform.position = Vector3.Lerp(startPosition, endPosition + (-frontBackTravellingSliderVector * (-deathCount * 0.5f)), Time.deltaTime);
                p_Camera.transform.position = Vector3.Lerp(startPosition, endPosition, time);
                break;
            case Behavior.STATIC_CAMERA_ZONE:
                time += Time.deltaTime;
                blendTime = time / 1.0f;
                //transform.position = Vector3.Lerp(startPosition, endPosition, Time.time * smoothValue / 2);
                transform.position = Vector3.Lerp(startPosition, endPosition, time);
                break;
            case Behavior.TRANSITION_TO_FOLLOW:
                Debug.Log("Distance Between Camera and Target = " + GenericSensUtilities.instance.DistanceBetween2Vectors(target.transform.position + cameraOffSet, p_Camera.transform.position));
                if (GenericSensUtilities.instance.DistanceBetween2Vectors(target.transform.position + cameraOffSet, p_Camera.transform.position) < 0.1f)
                {
                    SetActualBehavior(Behavior.FOLLOW_PLAYER);
                }
                else
                {
                    p_Camera.transform.position = Vector3.Lerp(p_Camera.transform.position, target.transform.position + cameraOffSet, Time.deltaTime * smoothValue / 2);
                }
                break;
            case Behavior.PLAYER_SHOW_WEAPON:
                //FollowTarget(PlayerController.instance.gameObject);
                time += Time.deltaTime;
                blendTime = time / 0.5f;
                blendTime = Mathf.Clamp(blendTime, 0, 1);
                transform.position = Vector3.Lerp(startPosition, endPosition, blendTime);
                break;
            default:
                break;
        }
	}
    public void FaceTarget(Vector3 _target)
    {
        Vector3 camera_target_Dir = _target - p_Camera.transform.position;

        p_Camera.transform.forward = Vector3.Slerp(p_Camera.transform.forward, camera_target_Dir.normalized, (smoothValue / 2) * Time.deltaTime);
    }
    void FollowTarget(GameObject _target)
    {
        desiredPosition = _target.transform.position + cameraOffSet;
        desiredPosition2D.x = desiredPosition.x;
        desiredPosition2D.y = desiredPosition.z;

        cameraMovement2Axis.x = Mathf.Lerp(p_Camera.transform.position.x, desiredPosition2D.x, smoothValue * Time.deltaTime);
        cameraMovement2Axis.y = Mathf.Lerp(p_Camera.transform.position.z, desiredPosition2D.y, smoothValue * Time.deltaTime);
        
        relativeCameraMovementVector = TransformTo2DMovement(desiredPosition) - cameraMovement2Axis;
        //Debug.Log("Relative Camera Movement: " + relativeCameraMovementVector);

        FollowTargetWithinLevelBorders();
        p_Camera.transform.position = cameraMovement;
    }
    
    public void StartScriptedMovement(GameObject tempTarget)
    {
        scriptedTarget = tempTarget;
        SetActualBehavior(Behavior.SCRIPT_MOVEMENT);
    }
    public void StaticCameraZone(GameObject tempTarget, float highDistance)
    {
        scriptedTarget = tempTarget;
        scriptedHighDistance = highDistance;
        SetActualBehavior(Behavior.STATIC_CAMERA_ZONE);
    }

    Vector2 TransformTo2DMovement(Vector3 movVect)
    {
        Vector2 mov2D;
        mov2D.x = movVect.x;
        mov2D.y = movVect.z;

        return mov2D;
    }
    Vector3 TransformTo3DMovement(Vector2 movVect)
    {
        Vector3 mov3D;
        mov3D.x = movVect.x;
        mov3D.y = target.transform.position.y + cameraOffSet.y;
        mov3D.z = movVect.y;

        return mov3D;
    }

    public void SetFieldOfViewSmoothness(float _smooth, float nFOV)
    {
        p_Camera.fieldOfView = Mathf.Lerp(p_Camera.fieldOfView, nFOV, _smooth * Time.deltaTime);
    }

    bool FollowTargetWithinLevelBorders()
    {
        Vector3 UP_MIDDLE_NearPlanePoint = new Vector3(0.5f, 1, p_Camera.nearClipPlane);
        UP_MIDDLE_NearPlanePoint = p_Camera.ViewportToWorldPoint(UP_MIDDLE_NearPlanePoint);
        Vector3 UP_MIDDLE_FarPlanePoint = new Vector3(0.5f, 1, p_Camera.farClipPlane);
        UP_MIDDLE_FarPlanePoint = p_Camera.ViewportToWorldPoint(UP_MIDDLE_FarPlanePoint);

        //Vector3 DOWN_MIDDLE_NearPlanePoint = new Vector3(0.5f, 0.1f, p_Camera.nearClipPlane);
        //DOWN_MIDDLE_NearPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_MIDDLE_NearPlanePoint);
        //Vector3 DOWN_MIDDLE_FarPlanePoint = new Vector3(0.5f, 0.1f, p_Camera.farClipPlane);
        //DOWN_MIDDLE_FarPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_MIDDLE_FarPlanePoint);

        Vector3 DOWN_MIDDLE_Checker_NearPlanePoint = new Vector3(0.5f, 0, p_Camera.nearClipPlane);
        DOWN_MIDDLE_Checker_NearPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_MIDDLE_Checker_NearPlanePoint);
        Vector3 DOWN_MIDDLE_Checker_FarPlanePoint = new Vector3(0.5f, 0, p_Camera.farClipPlane);
        DOWN_MIDDLE_Checker_FarPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_MIDDLE_Checker_FarPlanePoint);

        //Vector3 UP_LEFT_NearPlanePoint = new Vector3(0, 1, p_Camera.nearClipPlane);
        //UP_LEFT_NearPlanePoint = p_Camera.ViewportToWorldPoint(UP_LEFT_NearPlanePoint);
        //Vector3 UP_LEFT_FarPlanePoint = new Vector3(0, 1, p_Camera.farClipPlane);
        //UP_LEFT_FarPlanePoint = p_Camera.ViewportToWorldPoint(UP_LEFT_FarPlanePoint);

        //Vector3 UP_RIGHT_NearPlanePoint = new Vector3(1, 1, p_Camera.nearClipPlane);
        //UP_RIGHT_NearPlanePoint = p_Camera.ViewportToWorldPoint(UP_RIGHT_NearPlanePoint);
        //Vector3 UP_RIGHT_FarPlanePoint = new Vector3(1, 1, p_Camera.farClipPlane);
        //UP_RIGHT_FarPlanePoint = p_Camera.ViewportToWorldPoint(UP_RIGHT_FarPlanePoint);

        Vector3 DOWN_LEFT_NearPlanePoint = new Vector3(0, 0.1f, p_Camera.nearClipPlane);
        DOWN_LEFT_NearPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_LEFT_NearPlanePoint);
        Vector3 DOWN_LEFT_FarPlanePoint = new Vector3(0, 0.1f, p_Camera.farClipPlane);
        DOWN_LEFT_FarPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_LEFT_FarPlanePoint);

        Vector3 DOWN_RIGHT_NearPlanePoint = new Vector3(1, 0.1f, p_Camera.nearClipPlane);
        DOWN_RIGHT_NearPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_RIGHT_NearPlanePoint);
        Vector3 DOWN_RIGHT_FarPlanePoint = new Vector3(1, 0.1f, p_Camera.farClipPlane);
        DOWN_RIGHT_FarPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_RIGHT_FarPlanePoint);

        Vector3 fieldOfViewVectorUP_MIDDLE = UP_MIDDLE_FarPlanePoint - UP_MIDDLE_NearPlanePoint;
        //Vector3 fieldOfViewVectorDOWN_MIDDLE = DOWN_MIDDLE_FarPlanePoint - DOWN_MIDDLE_NearPlanePoint;
        Vector3 fieldOfViewVectorDOWN_MIDDLE_Checker = DOWN_MIDDLE_Checker_FarPlanePoint - DOWN_MIDDLE_Checker_NearPlanePoint;
        //Vector3 fieldOfViewVectorUP_LEFT = UP_LEFT_FarPlanePoint - UP_LEFT_NearPlanePoint;
        //Vector3 fieldOfViewVectorUP_RIGHT = UP_RIGHT_FarPlanePoint - UP_RIGHT_NearPlanePoint;
        Vector3 fieldOfViewVectorDOWN_LEFT = DOWN_LEFT_FarPlanePoint - DOWN_LEFT_NearPlanePoint;
        Vector3 fieldOfViewVectorDOWN_RIGHT = DOWN_RIGHT_FarPlanePoint - DOWN_RIGHT_NearPlanePoint;

        /*Debug.Log("Camera FieldOFView: " + p_Camera.fieldOfView);
        Debug.Log("FieldOFView UP_LEFT_vector: " + fieldOfViewVectorUP_LEFT);*/
        Ray rayoUP_MIDDLE = new Ray(UP_MIDDLE_NearPlanePoint, fieldOfViewVectorUP_MIDDLE);
        //Ray rayoDOWN_MIDDLE = new Ray(DOWN_MIDDLE_NearPlanePoint, fieldOfViewVectorDOWN_MIDDLE);
        Ray rayoDOWN_MIDDLE_Checker = new Ray(DOWN_MIDDLE_Checker_NearPlanePoint, fieldOfViewVectorDOWN_MIDDLE_Checker);
        //Ray rayoUP_LEFT = new Ray(UP_LEFT_NearPlanePoint, fieldOfViewVectorUP_LEFT);
        //Ray rayoUP_RIGHT = new Ray(UP_RIGHT_NearPlanePoint, fieldOfViewVectorUP_RIGHT);
        Ray rayoDOWN_LEFT = new Ray(DOWN_LEFT_NearPlanePoint, fieldOfViewVectorDOWN_LEFT);
        Ray rayoDOWN_RIGHT = new Ray(DOWN_RIGHT_NearPlanePoint, fieldOfViewVectorDOWN_RIGHT);

        RaycastHit rayoUP_MIDDLE_Hit;
        //RaycastHit rayoDOWN_MIDDLE_Hit;
        RaycastHit rayoDOWN_MIDDLE_Checker_Hit;
        //RaycastHit rayoUP_LEFT_Hit;
        //RaycastHit rayoUP_RIGHT_Hit;
        RaycastHit rayoDOWN_LEFT_Hit;
        RaycastHit rayoDOWN_RIGHT_Hit;

        Physics.Raycast(rayoUP_MIDDLE, out rayoUP_MIDDLE_Hit, 100, 1 << 15);
        //Physics.Raycast(rayoDOWN_MIDDLE, out rayoDOWN_MIDDLE_Hit, 100, 1 << 15);
        Physics.Raycast(rayoDOWN_MIDDLE_Checker, out rayoDOWN_MIDDLE_Checker_Hit, 100, 1 << 15);
        //Physics.Raycast(rayoUP_LEFT, out rayoUP_LEFT_Hit, 100, 1 << 15);
        //Physics.Raycast(rayoUP_RIGHT, out rayoUP_RIGHT_Hit, 100, 1 << 15);
        Physics.Raycast(rayoDOWN_LEFT, out rayoDOWN_LEFT_Hit, 100, 1 << 15);
        Physics.Raycast(rayoDOWN_RIGHT, out rayoDOWN_RIGHT_Hit, 100, 1 << 15);

        Debug.DrawLine(UP_MIDDLE_NearPlanePoint, rayoUP_MIDDLE_Hit.point, Color.red);
        //Debug.DrawLine(DOWN_MIDDLE_NearPlanePoint, rayoDOWN_MIDDLE_Hit.point, Color.green);
        Debug.DrawLine(DOWN_MIDDLE_Checker_NearPlanePoint, rayoDOWN_MIDDLE_Checker_Hit.point, Color.red);
        //Debug.DrawLine(UP_LEFT_NearPlanePoint, rayoUP_LEFT_Hit.point, Color.red);
        //Debug.DrawLine(UP_RIGHT_NearPlanePoint, rayoUP_RIGHT_Hit.point, Color.red);
        Debug.DrawLine(DOWN_LEFT_NearPlanePoint, rayoDOWN_LEFT_Hit.point, Color.yellow);
        Debug.DrawLine(DOWN_RIGHT_NearPlanePoint, rayoDOWN_RIGHT_Hit.point, Color.yellow);

        /*Debug.DrawRay(UP_LEFT_NearPlanePoint, fieldOfViewVectorUP_LEFT, Color.blue);
        Debug.DrawRay(UP_RIGHT_NearPlanePoint, fieldOfViewVectorUP_RIGHT, Color.red);
        Debug.DrawRay(DOWN_LEFT_NearPlanePoint, fieldOfViewVectorDOWN_LEFT, Color.green);
        Debug.DrawRay(DOWN_RIGHT_NearPlanePoint, fieldOfViewVectorDOWN_RIGHT, Color.black);*/
        bool outOfBorderUP_MIDDLE = false;
        //bool outOfBorderDOWN_MIDDLE = false;
        bool outOfBorderDOWN_MIDDLE_Checker = false;
        //bool outOfBorderUP_LEFT = false;
        //bool outOfBorderUP_RIGHT = false;
        bool outOfBorderDOWN_LEFT = false;
        bool outOfBorderDOWN_RIGHT = false;

        if (rayoUP_MIDDLE_Hit.collider != null && rayoUP_MIDDLE_Hit.collider.gameObject.GetComponentInParent<LevelControl>().IsActive())
        {
            outOfBorderUP_MIDDLE = false;
        }
        else
        {
            outOfBorderUP_MIDDLE = true;
        }

        //if (rayoDOWN_MIDDLE_Hit.collider != null && rayoDOWN_MIDDLE_Hit.collider.gameObject.GetComponentInParent<LevelControl>().IsActive())
        //{
        //    outOfBorderDOWN_MIDDLE = false;
        //}
        //else
        //{
        //    outOfBorderDOWN_MIDDLE = true;
        //}

        if (rayoDOWN_MIDDLE_Checker_Hit.collider != null && rayoDOWN_MIDDLE_Checker_Hit.collider.gameObject.GetComponentInParent<LevelControl>().IsActive())
        {
            outOfBorderDOWN_MIDDLE_Checker = false;
        }
        else
        {
            outOfBorderDOWN_MIDDLE_Checker = true;
        }
        //if (rayoUP_LEFT_Hit.collider != null && rayoUP_LEFT_Hit.collider.gameObject.GetComponentInParent<LevelControl>().IsActive())
        //{
        //    outOfBorderUP_LEFT = false;
        //}
        //else
        //{
        //    outOfBorderUP_LEFT = true;
        //}
        //if (rayoUP_RIGHT_Hit.collider != null && rayoUP_RIGHT_Hit.collider.gameObject.GetComponentInParent<LevelControl>().IsActive())
        //{
        //    outOfBorderUP_RIGHT = false;
        //}
        //else
        //{
        //    outOfBorderUP_RIGHT = true;
        //}
        if (rayoDOWN_LEFT_Hit.collider != null && rayoDOWN_LEFT_Hit.collider.gameObject.GetComponentInParent<LevelControl>().IsActive())
        {
            outOfBorderDOWN_LEFT = false;
        }
        else
        {
            outOfBorderDOWN_LEFT = true;
        }
        if (rayoDOWN_RIGHT_Hit.collider != null && rayoDOWN_RIGHT_Hit.collider.gameObject.GetComponentInParent<LevelControl>().IsActive())
        {
            outOfBorderDOWN_RIGHT = false;
        }
        else
        {
            outOfBorderDOWN_RIGHT = true;
        }

        //////////Limit Camera Movement///////////
        ///VERTICAL LIMITATIONS
        if (outOfBorderUP_MIDDLE || outOfBorderDOWN_MIDDLE_Checker)
        {
            if (outOfBorderUP_MIDDLE && !outOfBorderDOWN_MIDDLE_Checker)
            {
                if (relativeCameraMovementVector.y < 0)
                    cameraMovement.z = Mathf.Lerp(p_Camera.transform.position.z, desiredPosition.z, smoothValue * Time.deltaTime);
            }
            
            if (outOfBorderDOWN_MIDDLE_Checker && !outOfBorderUP_MIDDLE)
            {
                if (relativeCameraMovementVector.y > 0)
                    cameraMovement.z = Mathf.Lerp(p_Camera.transform.position.z, desiredPosition.z, smoothValue * Time.deltaTime);
            }
        }
        else
        {
            cameraMovement.z = Mathf.Lerp(p_Camera.transform.position.z, desiredPosition.z, smoothValue * Time.deltaTime);
        }

        ///LATERAL LIMITATIONS
        if (outOfBorderDOWN_LEFT || outOfBorderDOWN_RIGHT)
        {
            if (outOfBorderDOWN_LEFT && !outOfBorderDOWN_RIGHT)
            {
                if (relativeCameraMovementVector.x > 0)
                {
                    cameraMovement.x = Mathf.Lerp(p_Camera.transform.position.x, desiredPosition.x, smoothValue * Time.deltaTime);
                }
            }
            if (outOfBorderDOWN_RIGHT && !outOfBorderDOWN_LEFT)
            {
                if (relativeCameraMovementVector.x < 0)
                {
                    cameraMovement.x = Mathf.Lerp(p_Camera.transform.position.x, desiredPosition.x, smoothValue * Time.deltaTime);
                }
            }
        }       
        else
        {
            cameraMovement.x = Mathf.Lerp(p_Camera.transform.position.x, desiredPosition.x, smoothValue * Time.deltaTime);
        }
        
        //if (outOfBorderUP_LEFT && outOfBorderUP_RIGHT)
        //{
        //    if (relativeCameraMovementVector.y < 0)
        //    {
        //        cameraMovement.z = Mathf.Lerp(p_Camera.transform.position.z, desiredPosition.z, smoothValue * Time.deltaTime);
        //    }               
        //}
        //else if (outOfBorderDOWN_LEFT && outOfBorderDOWN_RIGHT)
        //{
        //    if (relativeCameraMovementVector.y > 0)
        //    {
        //        cameraMovement.z = Mathf.Lerp(p_Camera.transform.position.z, desiredPosition.z, smoothValue * Time.deltaTime);
        //    }         
        //}
        //else
        //{
        //    cameraMovement.z = Mathf.Lerp(p_Camera.transform.position.z, desiredPosition.z, smoothValue * Time.deltaTime);
        //}

        //if (outOfBorderUP_LEFT && outOfBorderDOWN_LEFT)
        //{
        //    if (relativeCameraMovementVector.x > 0)
        //    {
        //        cameraMovement.x = Mathf.Lerp(p_Camera.transform.position.x, desiredPosition.x, smoothValue * Time.deltaTime);
        //    }
        //}
        //else if (outOfBorderUP_RIGHT && outOfBorderDOWN_RIGHT)
        //{
        //    if (relativeCameraMovementVector.x < 0)
        //    {
        //        cameraMovement.x = Mathf.Lerp(p_Camera.transform.position.x, desiredPosition.x, smoothValue * Time.deltaTime);
        //    }
        //}
        //else
        //{
        //    cameraMovement.x = Mathf.Lerp(p_Camera.transform.position.x, desiredPosition.x, smoothValue * Time.deltaTime);
        //}
        cameraMovement.y = Mathf.Lerp(cameraMovement.y, desiredPosition.y, (smoothValue/2) * Time.deltaTime);
        
        //if (!outOFBorderUP_LEFT && !outOFBorderUP_RIGHT && !outOFBorderDOWN_LEFT && !outOFBorderDOWN_RIGHT) FaceTarget();
        return false;
    }
    public void SetActualBehavior(Behavior b)
    {
        //EXIT BEHAVIOR
        switch (actualBehavior)
        {
            case Behavior.FOLLOW_PLAYER:
                
                break;
            case Behavior.CHANGE_LEVEL:
                
                break;
            case Behavior.SCRIPT_MOVEMENT:
               
                break;
            case Behavior.PLAYER_DEATH:
                //deathCount = 0;
                p_Camera.transform.position = target.transform.position + cameraOffSet;
                break;
            case Behavior.STATIC_CAMERA_ZONE:
                break;
            case Behavior.TRANSITION_TO_FOLLOW:
                break;
            case Behavior.PLAYER_SHOW_WEAPON:
                break;
            default:
                break;
        }
        //ENTER BEHAVIOR
        switch (b)
        {
            case Behavior.FOLLOW_PLAYER:

                break;
            case Behavior.CHANGE_LEVEL:

                break;
            case Behavior.SCRIPT_MOVEMENT:

                break;
            case Behavior.PLAYER_DEATH:
                time = -0.5f;
                blendTime = 0;
                frontBackTravellingSliderVector = GenericSensUtilities.instance.GetDirectionFromTo_N(p_Camera.transform.position, PlayerController.instance.transform.position);
                startPosition = p_Camera.transform.position;
                break;
            case Behavior.STATIC_CAMERA_ZONE:
                time = 0;
                blendTime = 0;
                frontBackTravellingSliderVector = cameraOffSet.normalized;
                startPosition = p_Camera.transform.position;
                endPosition = scriptedTarget.transform.position + (frontBackTravellingSliderVector * scriptedHighDistance);
                break;
            case Behavior.TRANSITION_TO_FOLLOW:
                break;
            case Behavior.PLAYER_SHOW_WEAPON:
                time = 0;
                blendTime = 0;
                scriptedHighDistance = 2.0f;
                frontBackTravellingSliderVector = GenericSensUtilities.instance.GetDirectionFromTo_N(p_Camera.transform.position, PlayerController.instance.transform.position);
                startPosition = p_Camera.transform.position;
                endPosition = startPosition + (frontBackTravellingSliderVector * scriptedHighDistance);
                break;
            default:
                break;
        }
        actualBehavior = b;
    }
    public Behavior GetActualBehavior()
    {
        return actualBehavior;
    }
}
