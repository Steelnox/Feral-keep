using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_Behavior : MonoBehaviour
{
    public bool playerInteraction;
    public bool rockInteraction;

    public bool switched;
    public bool holdedSwitched;

    public void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            MovableRocks rock = other.GetComponent<MovableRocks>();
            if (playerInteraction && player != null)
            {
                switched = true;
                holdedSwitched = true;
            }
            if (rockInteraction && rock != null)
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

            if (playerInteraction && player != null)
            {
                holdedSwitched = false;
            }
            if (rockInteraction && rock != null)
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
