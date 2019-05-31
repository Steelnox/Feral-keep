/*using System.Collections;
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
    public GameObject player;
    public Vector3 cameraOffSet;
    public float smoothValue;
    public float standardFOV;
    public float lockTargetFOV;
    public float pushFOV;
    public float largeFOV;
    public Quaternion initCameraRotation;
    public float scriptedCooldownTime;

    public enum Behavior {FOLLOW_PLAYER, CHANGE_LEVEL, SCRIPT_MOVEMENT, PLYER_DEATH};
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
    
    void Start ()
    {
        player = PlayerController.instance.playerRoot;
        desiredPosition = player.transform.position + cameraOffSet;
        transform.position = desiredPosition;
        SetActualBehavior(Behavior.FOLLOW_PLAYER);
        initCameraRotation = p_Camera.transform.rotation;
        p_Camera.fieldOfView = standardFOV;
        actualScriptedCooldown = scriptedCooldownTime;
	}
	
	void Update ()
    {
        switch (actualBehavior)
        {
            case Behavior.FOLLOW_PLAYER:
                //FaceTarget();
                FollowTarget(player);
                break;
            case Behavior.CHANGE_LEVEL:
                desiredPosition = player.transform.position + cameraOffSet;
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
            case Behavior.PLYER_DEATH:
                if (transform.position == PlayerController.instance.transform.position + cameraOffSet) SetActualBehavior(Behavior.FOLLOW_PLAYER);
                transform.position = PlayerController.instance.transform.position + cameraOffSet;
                //if (PlayerController.instance.playerAlive) SetActualBehavior(Behavior.FOLLOW_PLAYER);
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
        mov3D.y = player.transform.position.y + cameraOffSet.y;
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

        Vector3 DOWN_MIDDLE_NearPlanePoint = new Vector3(0.5f, 0.1f, p_Camera.nearClipPlane);
        DOWN_MIDDLE_NearPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_MIDDLE_NearPlanePoint);
        Vector3 DOWN_MIDDLE_FarPlanePoint = new Vector3(0.5f, 0.1f, p_Camera.farClipPlane);
        DOWN_MIDDLE_FarPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_MIDDLE_FarPlanePoint);

        Vector3 DOWN_MIDDLE_Checker_NearPlanePoint = new Vector3(0.5f, 0, p_Camera.nearClipPlane);
        DOWN_MIDDLE_Checker_NearPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_MIDDLE_Checker_NearPlanePoint);
        Vector3 DOWN_MIDDLE_Checker_FarPlanePoint = new Vector3(0.5f, 0, p_Camera.farClipPlane);
        DOWN_MIDDLE_Checker_FarPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_MIDDLE_Checker_FarPlanePoint);

        Vector3 UP_LEFT_NearPlanePoint = new Vector3(0, 1, p_Camera.nearClipPlane);
        UP_LEFT_NearPlanePoint = p_Camera.ViewportToWorldPoint(UP_LEFT_NearPlanePoint);
        Vector3 UP_LEFT_FarPlanePoint = new Vector3(0, 1, p_Camera.farClipPlane);
        UP_LEFT_FarPlanePoint = p_Camera.ViewportToWorldPoint(UP_LEFT_FarPlanePoint);

        Vector3 UP_RIGHT_NearPlanePoint = new Vector3(1, 1, p_Camera.nearClipPlane);
        UP_RIGHT_NearPlanePoint = p_Camera.ViewportToWorldPoint(UP_RIGHT_NearPlanePoint);
        Vector3 UP_RIGHT_FarPlanePoint = new Vector3(1, 1, p_Camera.farClipPlane);
        UP_RIGHT_FarPlanePoint = p_Camera.ViewportToWorldPoint(UP_RIGHT_FarPlanePoint);

        Vector3 DOWN_LEFT_NearPlanePoint = new Vector3(0, 0.1f, p_Camera.nearClipPlane);
        DOWN_LEFT_NearPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_LEFT_NearPlanePoint);
        Vector3 DOWN_LEFT_FarPlanePoint = new Vector3(0, 0.1f, p_Camera.farClipPlane);
        DOWN_LEFT_FarPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_LEFT_FarPlanePoint);

        Vector3 DOWN_RIGHT_NearPlanePoint = new Vector3(1, 0.1f, p_Camera.nearClipPlane);
        DOWN_RIGHT_NearPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_RIGHT_NearPlanePoint);
        Vector3 DOWN_RIGHT_FarPlanePoint = new Vector3(1, 0.1f, p_Camera.farClipPlane);
        DOWN_RIGHT_FarPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_RIGHT_FarPlanePoint);

        Vector3 fieldOfViewVectorUP_MIDDLE = UP_MIDDLE_FarPlanePoint - UP_MIDDLE_NearPlanePoint;
        Vector3 fieldOfViewVectorDOWN_MIDDLE = DOWN_MIDDLE_FarPlanePoint - DOWN_MIDDLE_NearPlanePoint;
        Vector3 fieldOfViewVectorDOWN_MIDDLE_Checker = DOWN_MIDDLE_Checker_FarPlanePoint - DOWN_MIDDLE_Checker_NearPlanePoint;
        Vector3 fieldOfViewVectorUP_LEFT = UP_LEFT_FarPlanePoint - UP_LEFT_NearPlanePoint;
        Vector3 fieldOfViewVectorUP_RIGHT = UP_RIGHT_FarPlanePoint - UP_RIGHT_NearPlanePoint;
        Vector3 fieldOfViewVectorDOWN_LEFT = DOWN_LEFT_FarPlanePoint - DOWN_LEFT_NearPlanePoint;
        Vector3 fieldOfViewVectorDOWN_RIGHT = DOWN_RIGHT_FarPlanePoint - DOWN_RIGHT_NearPlanePoint;

        /*Debug.Log("Camera FieldOFView: " + p_Camera.fieldOfView);
        Debug.Log("FieldOFView UP_LEFT_vector: " + fieldOfViewVectorUP_LEFT);*/
        Ray rayoUP_MIDDLE = new Ray(UP_MIDDLE_NearPlanePoint, fieldOfViewVectorUP_MIDDLE);
        Ray rayoDOWN_MIDDLE = new Ray(DOWN_MIDDLE_NearPlanePoint, fieldOfViewVectorDOWN_MIDDLE);
        Ray rayoDOWN_MIDDLE_Checker = new Ray(DOWN_MIDDLE_Checker_NearPlanePoint, fieldOfViewVectorDOWN_MIDDLE_Checker);
        Ray rayoUP_LEFT = new Ray(UP_LEFT_NearPlanePoint, fieldOfViewVectorUP_LEFT);
        Ray rayoUP_RIGHT = new Ray(UP_RIGHT_NearPlanePoint, fieldOfViewVectorUP_RIGHT);
        Ray rayoDOWN_LEFT = new Ray(DOWN_LEFT_NearPlanePoint, fieldOfViewVectorDOWN_LEFT);
        Ray rayoDOWN_RIGHT = new Ray(DOWN_RIGHT_NearPlanePoint, fieldOfViewVectorDOWN_RIGHT);

        RaycastHit rayoUP_MIDDLE_Hit;
        RaycastHit rayoDOWN_MIDDLE_Hit;
        RaycastHit rayoDOWN_MIDDLE_Checker_Hit;
        RaycastHit rayoUP_LEFT_Hit;
        RaycastHit rayoUP_RIGHT_Hit;
        RaycastHit rayoDOWN_LEFT_Hit;
        RaycastHit rayoDOWN_RIGHT_Hit;

        Physics.Raycast(rayoUP_MIDDLE, out rayoUP_MIDDLE_Hit, 100, 1 << 15);
        Physics.Raycast(rayoDOWN_MIDDLE, out rayoDOWN_MIDDLE_Hit, 100, 1 << 15);
        Physics.Raycast(rayoDOWN_MIDDLE_Checker, out rayoDOWN_MIDDLE_Checker_Hit, 100, 1 << 15);
        Physics.Raycast(rayoUP_LEFT, out rayoUP_LEFT_Hit, 100, 1 << 15);
        Physics.Raycast(rayoUP_RIGHT, out rayoUP_RIGHT_Hit, 100, 1 << 15);
        Physics.Raycast(rayoDOWN_LEFT, out rayoDOWN_LEFT_Hit, 100, 1 << 15);
        Physics.Raycast(rayoDOWN_RIGHT, out rayoDOWN_RIGHT_Hit, 100, 1 << 15);

        Debug.DrawLine(UP_MIDDLE_NearPlanePoint, rayoUP_MIDDLE_Hit.point, Color.red);
        Debug.DrawLine(DOWN_MIDDLE_NearPlanePoint, rayoDOWN_MIDDLE_Hit.point, Color.green);
        Debug.DrawLine(DOWN_MIDDLE_Checker_NearPlanePoint, rayoDOWN_MIDDLE_Checker_Hit.point, Color.red);
        Debug.DrawLine(UP_LEFT_NearPlanePoint, rayoUP_LEFT_Hit.point, Color.red);
        Debug.DrawLine(UP_RIGHT_NearPlanePoint, rayoUP_RIGHT_Hit.point, Color.red);
        Debug.DrawLine(DOWN_LEFT_NearPlanePoint, rayoDOWN_LEFT_Hit.point, Color.yellow);
        Debug.DrawLine(DOWN_RIGHT_NearPlanePoint, rayoDOWN_RIGHT_Hit.point, Color.yellow);

        /*Debug.DrawRay(UP_LEFT_NearPlanePoint, fieldOfViewVectorUP_LEFT, Color.blue);
        Debug.DrawRay(UP_RIGHT_NearPlanePoint, fieldOfViewVectorUP_RIGHT, Color.red);
        Debug.DrawRay(DOWN_LEFT_NearPlanePoint, fieldOfViewVectorDOWN_LEFT, Color.green);
        Debug.DrawRay(DOWN_RIGHT_NearPlanePoint, fieldOfViewVectorDOWN_RIGHT, Color.black);*/
        bool outOfBorderUP_MIDDLE = false;
        bool outOfBorderDOWN_MIDDLE = false;
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

        if (rayoDOWN_MIDDLE_Hit.collider != null && rayoDOWN_MIDDLE_Hit.collider.gameObject.GetComponentInParent<LevelControl>().IsActive())
        {
            outOfBorderDOWN_MIDDLE = false;
        }
        else
        {
            outOfBorderDOWN_MIDDLE = true;
        }

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
        actualBehavior = b;
    }
    public Behavior GetActualBehavior()
    {
        return actualBehavior;
    }
}
