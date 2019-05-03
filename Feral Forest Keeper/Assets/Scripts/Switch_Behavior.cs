using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_Behavior : MonoBehaviour
{
    //public enum SwitchFunctionType { OPEN_DOOR, OPEN_DOOR_HOLD};

    //public SwitchFunctionType functionType;

    //public GameObject mechanicalScenarioPart;
    //public OpenableDoors door;
    //public Vector3 mechanicalPartMovementDirection;
    [SerializeField]
    private bool switched;
    [SerializeField]
    private bool holdedSwitched;

    //void Start()
    //{
    //    switch (functionType)
    //    {
    //        case SwitchFunctionType.OPEN_DOOR:
    //            break;
    //        case SwitchFunctionType.OPEN_DOOR_HOLD:
    //            break;
    //    }
    //}

    //void Update()
    //{
    //    if (switched)
    //    {
    //        switch (functionType)
    //        {
    //            case SwitchFunctionType.OPEN_DOOR:
    //                break;
    //            case SwitchFunctionType.OPEN_DOOR_HOLD:
    //                break;
    //        }
    //    }       
    //}
    public void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            MovableRocks rock = other.GetComponent<MovableRocks>();
            if (player != null || rock != null)
            {
                switched = true;
                holdedSwitched = true;
            }
        }
    }

    /*public void OnTriggerStay(Collider other)
    {
        //if (other != null)
        //{
        //    PlayerController player = other.GetComponent<PlayerController>();
        //    MovableRocks rock = other.GetComponent<MovableRocks>();
        //    if (player != null || rock != null)
        //    {
        //        holdedSwitched = true;
        //    }
        //}
    }*/

    public void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            MovableRocks rock = other.GetComponent<MovableRocks>();
            if (player != null || rock != null)
            {
                holdedSwitched = false;
            }
        }
    }
    public bool IsSwitched()
    {
        return switched;
    }
    public bool IsHoldedSwitched()
    {
        return holdedSwitched;
    }
}
