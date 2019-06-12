using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableRocks : MonoBehaviour
{
    public GameObject rockBody;
    public bool attchAviable;
    public bool beingPushed;
    public bool falling;
    public float angleToContact;
    public float attachDistance;
    public float weight;
    [SerializeField]
    private Vector3 lastNoPushingPos;
    public MeshRenderer bodyMeshrenderer;
    public Rigidbody myRigidbody;

    void Start()
    {
        //previousPos = rockBody.transform.position;
        lastNoPushingPos = Vector3.zero;
        //bodyMeshrenderer = rockBody.GetComponent<MeshRenderer>();
        myRigidbody.isKinematic = true;
    }

    void Update()
    {
        //if (CheckGroundDistance() > bodyMeshrenderer.bounds.extents.y / 2 + 0.5f || rockBody.transform.position.y < lastNoPushingPos.y - 0.5f)
        //{
        //    //Debug.Log("Rock Distance To Ground = " + CheckGroundDistance());
        //    //myRigidbody.isKinematic = false;
        //    falling = true;
        //}
        //else
        //{
        //    falling = false;
        //}
        CheckSideToPush();

        if (!beingPushed)
        {
            if (CheckGroundDistance() > bodyMeshrenderer.bounds.extents.y / 2 + 0.5f || rockBody.transform.position.y < lastNoPushingPos.y - 0.5f)
            {
                myRigidbody.isKinematic = false;
                falling = true;
            }
            else
            {
                falling = false;
            }
        }
        else
        {
            falling = false;
        }

        //previousPos = rockBody.transform.position;
    }
    public Vector2 CheckSideToPush()
    {
        Vector2 localDirection = new Vector2(0,0);
        attchAviable = false;
        float angleBetweenPlayerForwardAndContactPointForward = Vector2.Angle(GenericSensUtilities.instance.Transform3DTo2DMovement(PlayerController.instance.characterModel.transform.forward), GenericSensUtilities.instance.Transform3DTo2DMovement(GenericSensUtilities.instance.GetDirectionFromTo_N(PlayerAnimationController.instance.transform.position, FindContactPoint(PlayerAnimationController.instance.transform.position))));

        if (GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerController.instance.characterModel.transform.position, transform.position) < attachDistance)
        {
            //Debug.Log("PLAYER ON FORWARD: " + angleBetweenPlayerForwardAndContactPointForward);
            localDirection = GenericSensUtilities.instance.Transform3DTo2DMovement(GenericSensUtilities.instance.GetDirectionFromTo_N(PlayerAnimationController.instance.transform.position, FindContactPoint(PlayerAnimationController.instance.transform.position)));
            if (angleBetweenPlayerForwardAndContactPointForward < angleToContact && angleBetweenPlayerForwardAndContactPointForward > 0)
                attchAviable = true;
        }
        //Debug.Log("Rock Body Center Bonunds: " + bodyMeshrenderer.bounds.center);
        //Debug.Log("LocalDirection on rock: " + localDirection);
        return localDirection;
    }
    public void PushRock(Vector3 force)
    {
        /*playerForceInput = force;*/
        Vector3 newPosition = this.transform.position + force;
        //newPosition = transform.TransformDirection(newPosition);
        transform.position = Vector3.Lerp(this.transform.position, newPosition, Time.deltaTime / weight);
    }
    public void SetBeingPushed(bool b)
    {
        beingPushed = b;
    }
    public bool CheckIfFalling()
    {
        return falling;
    }
    public float CheckGroundDistance()
    {
        float dis;
        Ray ray = new Ray(rockBody.transform.position, Vector3.down * bodyMeshrenderer.bounds.extents.y / 2);
        RaycastHit rayHit;
        Physics.Raycast(ray, out rayHit);

        if (rayHit.collider != null)
        {
            Debug.DrawLine(rockBody.transform.position, rayHit.point, Color.red);
            dis = GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, rayHit.point);
            //Debug.Log("Ground Distance = " + dis);
            return dis;
        }
        else
        {
            return 0;
        }
    }
    public void SetLastNoPushPosition(Vector3 _pos)
    {
        lastNoPushingPos = _pos;
    }
    public Vector3 GetLastNoPushingPosition()
    {
        return lastNoPushingPos;
    }
    public void ResetLastNoPushingPos()
    {
        lastNoPushingPos = Vector3.zero;
    }
    public Vector3 FindContactPoint(Vector3 worldPosition)
    {
        Vector3 point;

        point = bodyMeshrenderer.bounds.ClosestPoint(worldPosition);
        Debug.DrawLine(point, worldPosition, Color.cyan);
        return point;
    }
}
