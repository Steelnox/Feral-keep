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
    public GameObject target;
    public Vector3 cameraOffSet;
    public float smoothValue;
    public float standardFOV;
    public float lockTargetFOV;
    public float pushFOV;
    public float largeFOV;
    public Quaternion initCameraRotation;

    public enum Behavior {FOLLOW_PLAYER, CHANGE_LEVEL, SCRIPT_MOVEMENT};
    [SerializeField]
    private Behavior actualBehavior;
    private Vector3 desiredPosition;
    private Vector3 cameraMovement;
    private Vector2 cameraMovement2Axis;
    private Vector2 desiredPosition2D;
    private Vector2 relativeCameraMovementVector;
    
    void Start ()
    {
        target = PlayerController.instance.playerRoot;
        desiredPosition = target.transform.position + cameraOffSet;
        transform.position = desiredPosition;
        actualBehavior = Behavior.FOLLOW_PLAYER;
        initCameraRotation = p_Camera.transform.rotation;
        p_Camera.fieldOfView = standardFOV;
	}
	
	void Update ()
    {
        switch (actualBehavior)
        {
            case Behavior.FOLLOW_PLAYER:
                //FaceTarget();
                FollowTarget();
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
    void FollowTarget()
    {
        desiredPosition = target.transform.position + cameraOffSet;
        desiredPosition2D.x = desiredPosition.x;
        desiredPosition2D.y = desiredPosition.z;

        cameraMovement2Axis.x = Mathf.Lerp(p_Camera.transform.position.x, desiredPosition2D.x, smoothValue * Time.deltaTime);
        cameraMovement2Axis.y = Mathf.Lerp(p_Camera.transform.position.z, desiredPosition2D.y, smoothValue * Time.deltaTime);
        
        relativeCameraMovementVector = TransformTo2DMovement(desiredPosition) - cameraMovement2Axis;
        //Debug.Log("Relative Camera Movement: " + relativeCameraMovementVector);

        FollowTargetWithinLevelBorders();
        p_Camera.transform.position = cameraMovement;
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
        Vector3 UP_LEFT_NearPlanePoint = new Vector3(0, 1, p_Camera.nearClipPlane);
        UP_LEFT_NearPlanePoint = p_Camera.ViewportToWorldPoint(UP_LEFT_NearPlanePoint);
        Vector3 UP_LEFT_FarPlanePoint = new Vector3(0, 1, p_Camera.farClipPlane);
        UP_LEFT_FarPlanePoint = p_Camera.ViewportToWorldPoint(UP_LEFT_FarPlanePoint);

        Vector3 UP_RIGHT_NearPlanePoint = new Vector3(1, 1, p_Camera.nearClipPlane);
        UP_RIGHT_NearPlanePoint = p_Camera.ViewportToWorldPoint(UP_RIGHT_NearPlanePoint);
        Vector3 UP_RIGHT_FarPlanePoint = new Vector3(1, 1, p_Camera.farClipPlane);
        UP_RIGHT_FarPlanePoint = p_Camera.ViewportToWorldPoint(UP_RIGHT_FarPlanePoint);

        Vector3 DOWN_LEFT_NearPlanePoint = new Vector3(0, 0, p_Camera.nearClipPlane);
        DOWN_LEFT_NearPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_LEFT_NearPlanePoint);
        Vector3 DOWN_LEFT_FarPlanePoint = new Vector3(0, 0, p_Camera.farClipPlane);
        DOWN_LEFT_FarPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_LEFT_FarPlanePoint);

        Vector3 DOWN_RIGHT_NearPlanePoint = new Vector3(1, 0, p_Camera.nearClipPlane);
        DOWN_RIGHT_NearPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_RIGHT_NearPlanePoint);
        Vector3 DOWN_RIGHT_FarPlanePoint = new Vector3(1, 0, p_Camera.farClipPlane);
        DOWN_RIGHT_FarPlanePoint = p_Camera.ViewportToWorldPoint(DOWN_RIGHT_FarPlanePoint);

        Vector3 fieldOfViewVectorUP_LEFT = UP_LEFT_FarPlanePoint - UP_LEFT_NearPlanePoint;
        Vector3 fieldOfViewVectorUP_RIGHT = UP_RIGHT_FarPlanePoint - UP_RIGHT_NearPlanePoint;
        Vector3 fieldOfViewVectorDOWN_LEFT = DOWN_LEFT_FarPlanePoint - DOWN_LEFT_NearPlanePoint;
        Vector3 fieldOfViewVectorDOWN_RIGHT = DOWN_RIGHT_FarPlanePoint - DOWN_RIGHT_NearPlanePoint;

        /*Debug.Log("Camera FieldOFView: " + p_Camera.fieldOfView);
        Debug.Log("FieldOFView UP_LEFT_vector: " + fieldOfViewVectorUP_LEFT);*/

        Ray rayoUP_LEFT = new Ray(UP_LEFT_NearPlanePoint, fieldOfViewVectorUP_LEFT);
        Ray rayoUP_RIGHT = new Ray(UP_RIGHT_NearPlanePoint, fieldOfViewVectorUP_RIGHT);
        Ray rayoDOWN_LEFT = new Ray(DOWN_LEFT_NearPlanePoint, fieldOfViewVectorDOWN_LEFT);
        Ray rayoDOWN_RIGHT = new Ray(DOWN_RIGHT_NearPlanePoint, fieldOfViewVectorDOWN_RIGHT);

        RaycastHit rayoUP_LEFT_Hit;
        RaycastHit rayoUP_RIGHT_Hit;
        RaycastHit rayoDOWN_LEFT_Hit;
        RaycastHit rayoDOWN_RIGHT_Hit;

        Physics.Raycast(rayoUP_LEFT, out rayoUP_LEFT_Hit, 100, 1 << 8);
        Physics.Raycast(rayoUP_RIGHT, out rayoUP_RIGHT_Hit, 100, 1 << 8);
        Physics.Raycast(rayoDOWN_LEFT, out rayoDOWN_LEFT_Hit, 100, 1 << 8);
        Physics.Raycast(rayoDOWN_RIGHT, out rayoDOWN_RIGHT_Hit, 100, 1 << 8);

        Debug.DrawLine(UP_LEFT_NearPlanePoint, rayoUP_LEFT_Hit.point, Color.red);
        Debug.DrawLine(UP_RIGHT_NearPlanePoint, rayoUP_RIGHT_Hit.point, Color.red);
        Debug.DrawLine(DOWN_LEFT_NearPlanePoint, rayoDOWN_LEFT_Hit.point, Color.red);
        Debug.DrawLine(DOWN_RIGHT_NearPlanePoint, rayoDOWN_RIGHT_Hit.point, Color.red);

        /*Debug.DrawRay(UP_LEFT_NearPlanePoint, fieldOfViewVectorUP_LEFT, Color.blue);
        Debug.DrawRay(UP_RIGHT_NearPlanePoint, fieldOfViewVectorUP_RIGHT, Color.red);
        Debug.DrawRay(DOWN_LEFT_NearPlanePoint, fieldOfViewVectorDOWN_LEFT, Color.green);
        Debug.DrawRay(DOWN_RIGHT_NearPlanePoint, fieldOfViewVectorDOWN_RIGHT, Color.black);*/

        bool outOFBorderUP_LEFT = false;
        bool outOFBorderUP_RIGHT = false;
        bool outOFBorderDOWN_LEFT = false;
        bool outOFBorderDOWN_RIGHT = false;

        if (rayoUP_LEFT_Hit.collider != null && rayoUP_LEFT_Hit.collider.gameObject.GetComponent<LevelControl>().IsActive())
        {
            outOFBorderUP_LEFT = false;
        }
        else
        {
            outOFBorderUP_LEFT = true;
        }
        if (rayoUP_RIGHT_Hit.collider != null && rayoUP_RIGHT_Hit.collider.GetComponent<LevelControl>().IsActive())
        {
            outOFBorderUP_RIGHT = false;
        }
        else
        {
            outOFBorderUP_RIGHT = true;
        }
        if (rayoDOWN_LEFT_Hit.collider != null && rayoDOWN_LEFT_Hit.collider.GetComponent<LevelControl>().IsActive())
        {
            outOFBorderDOWN_LEFT = false;
        }
        else
        {
            outOFBorderDOWN_LEFT = true;
        }
        if (rayoDOWN_RIGHT_Hit.collider != null && rayoDOWN_RIGHT_Hit.collider.GetComponent<LevelControl>().IsActive())
        {
            outOFBorderDOWN_RIGHT = false;
        }
        else
        {
            outOFBorderDOWN_RIGHT = true;
        }

        //////////Limit Camera Movement///////////
        if (outOFBorderUP_LEFT && outOFBorderUP_RIGHT)
        {
            if (relativeCameraMovementVector.y < 0)
            {
                cameraMovement.z = Mathf.Lerp(p_Camera.transform.position.z, desiredPosition.z, smoothValue * Time.deltaTime);
            }               
        }
        else if (outOFBorderDOWN_LEFT && outOFBorderDOWN_RIGHT)
        {
            if (relativeCameraMovementVector.y > 0)
            {
                cameraMovement.z = Mathf.Lerp(p_Camera.transform.position.z, desiredPosition.z, smoothValue * Time.deltaTime);
            }         
        }
        else
        {
            cameraMovement.z = Mathf.Lerp(p_Camera.transform.position.z, desiredPosition.z, smoothValue * Time.deltaTime);
        }

        if (outOFBorderUP_LEFT && outOFBorderDOWN_LEFT)
        {
            if (relativeCameraMovementVector.x > 0)
            {
                cameraMovement.x = Mathf.Lerp(p_Camera.transform.position.x, desiredPosition.x, smoothValue * Time.deltaTime);
            }
        }
        else if (outOFBorderUP_RIGHT && outOFBorderDOWN_RIGHT)
        {
            if (relativeCameraMovementVector.x < 0)
            {
                cameraMovement.x = Mathf.Lerp(p_Camera.transform.position.x, desiredPosition.x, smoothValue * Time.deltaTime);
            }
        }
        else
        {
            cameraMovement.x = Mathf.Lerp(p_Camera.transform.position.x, desiredPosition.x, smoothValue * Time.deltaTime);
        }
        cameraMovement.y = desiredPosition.y;
        
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
