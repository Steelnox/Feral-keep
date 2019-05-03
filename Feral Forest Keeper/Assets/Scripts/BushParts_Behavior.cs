using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushParts_Behavior : MonoBehaviour
{
    public GameObject bodyPivot;
    public Vector3 blendDirection;
    public bool playerInteraction;
    public bool playerInRange;
    public float interactionDistance;
    public Vector3 desiredUpVector;
    public float smoothMovement;
    public Vector3 initPivotUpVect;

    void Start()
    {
        initPivotUpVect = bodyPivot.transform.up;
        playerInteraction = false;
    }
    void Update()
    {
        if (playerInRange && GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerController.instance.playerRoot.transform.position, bodyPivot.transform.position) < interactionDistance)
        {
            if (playerInteraction != true)playerInteraction = true;
        }
        else
        {
            if (playerInteraction != false) playerInteraction = false;
        }
        if (playerInteraction)
        {
            blendDirection = GenericSensUtilities.instance.Transform2DTo3DMovement(GenericSensUtilities.instance.Transform3DTo2DMovement(GenericSensUtilities.instance.GetDirectionFromTo_N(PlayerController.instance.gameObject.transform.position, bodyPivot.transform.position)));
            desiredUpVector = Vector3.up + blendDirection * ( 1 - ((GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerController.instance.playerRoot.transform.position, bodyPivot.transform.position) / interactionDistance)));
            bodyPivot.transform.up = Vector3.Lerp(bodyPivot.transform.up, desiredUpVector, smoothMovement * Time.deltaTime);
        }
        if (!playerInteraction && bodyPivot.transform.up != initPivotUpVect)
        {
            bodyPivot.transform.up = Vector3.Lerp(bodyPivot.transform.up, initPivotUpVect, smoothMovement * Time.deltaTime);
        }
    }

    /*public void OnTriggerStay(Collider other)
    {
        if (other != null)
        {
            PlayerController p = other.GetComponent<PlayerController>();
            if (p != null && !playerInteraction)
            {
                playerInRange = true;
                
            }
            else
            {
                blendDirection = Vector3.zero;
                playerInRange = false;
            }
        }
        else
        {
            blendDirection = Vector3.zero;
            playerInRange = false;
        }
    }*/
}
