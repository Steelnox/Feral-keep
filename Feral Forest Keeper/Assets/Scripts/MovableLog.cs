using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableLog : MonoBehaviour
{
    public GameObject logBody;
    public bool attchAviable;
    public bool beingPushed;
    public bool falling;
    public float angleToContact;
    public float attachDistance;
    public float weight;
    [SerializeField]
    private Vector3 lastNoPushingPos;
    private MeshRenderer bodyMeshrenderer;

    void Start()
    {
        bodyMeshrenderer = logBody.GetComponent<MeshRenderer>();
        lastNoPushingPos = Vector3.zero;
    }

    void Update()
    {
        //CheckSideToPush();
        if (beingPushed)
        {
            if (CheckGroundDistance() > bodyMeshrenderer.bounds.extents.y + 0.2f || logBody.transform.position.y < lastNoPushingPos.y)
            {
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
    }
    public Vector2 CheckSideToPush()
    {
        Vector2 localDirection = new Vector2(0, 0);
        attchAviable = false;

        //float angleBetweenForward = Vector2.Angle(GenericSensUtilities.instance.Transform3DTo2DMovement(GenericSensUtilities.instance.GetDirectionFromTo_N(PlayerController.instance.characterModel.transform.position, FindContactPoint(PlayerController.instance.transform.position))), GenericSensUtilities.instance.Transform3DTo2DMovement(logBody.transform.forward));
        //float angleBetweenRight = Vector2.Angle(GenericSensUtilities.instance.Transform3DTo2DMovement(GenericSensUtilities.instance.GetDirectionFromTo_N(PlayerController.instance.characterModel.transform.position, FindContactPoint(PlayerController.instance.transform.position))), GenericSensUtilities.instance.Transform3DTo2DMovement(logBody.transform.right));
        //float angleBetweenPlayerForwardAndForward = Vector2.Angle(GenericSensUtilities.instance.Transform3DTo2DMovement(PlayerController.instance.characterModel.transform.forward), GenericSensUtilities.instance.Transform3DTo2DMovement(logBody.transform.forward));
        //float angleBetweenPlayerForwardAndRight = Vector2.Angle(GenericSensUtilities.instance.Transform3DTo2DMovement(PlayerController.instance.characterModel.transform.forward), GenericSensUtilities.instance.Transform3DTo2DMovement(logBody.transform.right));
        float angleBetweenPlayerForwardAndContactPointForward = Vector2.Angle(GenericSensUtilities.instance.Transform3DTo2DMovement(PlayerController.instance.characterModel.transform.forward), GenericSensUtilities.instance.Transform3DTo2DMovement(GenericSensUtilities.instance.GetDirectionFromTo_N(PlayerAnimationController.instance.transform.position, FindContactPoint(PlayerAnimationController.instance.transform.position))));

        if (GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerController.instance.characterModel.transform.position, transform.position) < attachDistance)
        {
            localDirection = GenericSensUtilities.instance.Transform3DTo2DMovement(transform.forward);
            if (angleBetweenPlayerForwardAndContactPointForward < angleToContact && angleBetweenPlayerForwardAndContactPointForward > 0)
                attchAviable = true;
            //if (angleBetweenForward > 180 - angleToContact / 2 && angleBetweenForward < 180 + angleToContact / 2) //PLAYER ON FORWARDS SIDE
            //{
            //    //localDirection.y = 1;
            //    localDirection = GenericSensUtilities.instance.Transform3DTo2DMovement(transform.forward);
            //    if (angleBetweenPlayerForwardAndForward > 180 - angleToContact / 2 && angleBetweenPlayerForwardAndForward < 180 + angleToContact / 2)
            //        attchAviable = true;
            //    //Debug.Log("PLAYER ON FORWARD: " + angleBetweenPlayerForwardAndForward);
            //    //Debug.Log("PLAYER ON FORWARD SIDE: " + angleBetweenForward);
            //}
            //else
            //if (angleBetweenForward < angleToContact / 2) //PLAYERS ON BACK SIDE
            //{
            //    //localDirection.y = -1;
            //    localDirection = GenericSensUtilities.instance.Transform3DTo2DMovement(-transform.forward);
            //    if (angleBetweenPlayerForwardAndForward < angleToContact / 2)
            //        attchAviable = true;
            //    //Debug.Log("PLAYER ON FORWARD: " + angleBetweenPlayerForwardAndForward);
            //    //Debug.Log("PLAYER ON BACK SIDE: " + angleBetweenForward);
            //}
            //else
            //if (angleBetweenRight > 180 - angleToContact / 2 && angleBetweenRight < 180 + angleToContact / 2) //PLAYERS ON RIGHT SIDE
            //{
            //    //localDirection.x = 1;
            //    localDirection = GenericSensUtilities.instance.Transform3DTo2DMovement(transform.right);
            //    if (angleBetweenPlayerForwardAndRight > 180 - angleToContact / 2 && angleBetweenPlayerForwardAndRight < 180 + angleToContact / 2)
            //        attchAviable = true;
            //    //Debug.Log("PLAYER ON RIGHT: " + angleBetweenPlayerForwardAndRight);
            //    //Debug.Log("PLAYER ON RIGHT SIDE " + angleBetweenRight);
            //}
            //else
            //if (angleBetweenRight < angleToContact / 2) //PLAYERS ON LEFT SIDE
            //{
            //    //localDirection.x = -1;
            //    localDirection = GenericSensUtilities.instance.Transform3DTo2DMovement(-transform.right);
            //    if (angleBetweenPlayerForwardAndRight < angleToContact / 2)
            //        attchAviable = true;
            //    //Debug.Log("PLAYER ON RIGHT: " + angleBetweenPlayerForwardAndRight);
            //    //Debug.Log("PLAYER ON LEFT SIDE " + angleBetweenRight);
            //}
        }

        //Debug.Log("LocalDirection on rock: " + localDirection);
        return localDirection;
    }
    public void PushLog(Vector3 force)
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
        Ray ray = new Ray(logBody.transform.position, Vector3.down);
        RaycastHit rayHit;
        Physics.Raycast(ray, out rayHit);

        if (rayHit.collider != null)
        {
            dis = GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, rayHit.point);
            //Debug.Log("Ground Distance = " + dis);
            return dis;
        }
        return 0;
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
