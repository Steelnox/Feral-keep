using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableDoors : MonoBehaviour
{
    public GameObject movableDoorPivot;
    public GameObject doorBody;
    public float interactionDistance;

    public Item doorKey;
    public Switch_Behavior activationSwitch;

    public float smoothmovement;
    //public bool openedState;
    public bool lockState;

    private bool activated;
    private bool opened;
    private bool locked;
    private Vector3 closeRot;
    private Vector3 openRot;

    void Start()
    {
        if (doorKey != null && activationSwitch == null)
        {
            closeRot = movableDoorPivot.transform.localRotation.eulerAngles;
            openRot = new Vector3(0, 120, 0);

            locked = lockState;
            opened = false;
        }
        if (activationSwitch != null && doorKey == null)
        {
            closeRot = movableDoorPivot.transform.localRotation.eulerAngles;
            openRot = new Vector3(0, 120, 0);
        }
    }

    void Update()
    {        
        if (doorKey != null && activationSwitch == null)
        {
            if (!locked)
            {
                if (opened && movableDoorPivot.transform.rotation.eulerAngles != openRot)
                {
                    Debug.Log("Opennig Door with key");
                    movableDoorPivot.transform.localRotation = Quaternion.Lerp(movableDoorPivot.transform.localRotation, Quaternion.Euler(openRot), Time.deltaTime * smoothmovement);
                }
            }
        }
        if (activationSwitch != null && doorKey == null)
        {
            if (activationSwitch.IsHoldedSwitched())
            {
                Debug.Log("SwitchedActivated");
                if (movableDoorPivot.transform.localRotation.eulerAngles != openRot)
                {
                    Debug.Log("Opennig Door");
                    movableDoorPivot.transform.localRotation = Quaternion.Lerp(movableDoorPivot.transform.localRotation, Quaternion.Euler(openRot), Time.deltaTime * smoothmovement);
                }
                else
                {
                    movableDoorPivot.transform.localRotation = Quaternion.Euler(openRot);
                }
            }
            else
            if (!activationSwitch.IsHoldedSwitched())
            {
                if (movableDoorPivot.transform.localRotation.eulerAngles != closeRot)
                {
                    Debug.Log("Closing Door");
                    movableDoorPivot.transform.localRotation = Quaternion.Lerp(movableDoorPivot.transform.localRotation, Quaternion.Euler(closeRot), Time.deltaTime * smoothmovement);
                }
                else
                {
                    movableDoorPivot.transform.localRotation = Quaternion.Euler(closeRot);
                }
            }
        }
    }
    private void UnlockDoor()
    {
        locked = false;
    }
    private void LockDoor()
    {
        locked = true;
    }
    public void OpenDoor()
    {
        UnlockDoor();
        opened = true;
    }
    /*public void OpenDoorHolding(bool holding)
    {
        opened = holding;
    }*/
    public bool GetLockedActualState()
    {
        return locked;
    }
}
