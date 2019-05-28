using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_Behavior : MonoBehaviour
{
    [SerializeField]
    public bool switched;
    [SerializeField]
    public bool holdedSwitched;

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
