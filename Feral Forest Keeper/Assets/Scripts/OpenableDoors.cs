using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableDoors : MonoBehaviour
{
    public GameObject movableDoorPivot;
    public GameObject doorBody;
    public Item[] doorKeys;
    public Switch_Behavior activationSwitch;
    public float interactionDistance;
    public float setupTimeLaps;
    public float scripteMovementsRepetitions;
    public float smoothmovement;
    public bool cameraFeedback;
    public bool startOpened;
    public bool holdSwitchToOpen;
    public bool lockState;
    public enum AxisPivot { X, Y, Z}
    public AxisPivot axisPivot;
    public int maxGrades;

    private bool activated;
    private bool opened;
    private bool locked;
    private Vector3 closeRot;
    private Vector3 openRot;
    private bool justUnlocked;
    private bool justOpen;
    private bool justClose;
    [SerializeField]
    private float actualSetupTimeLaps;
    private int numKeys;

    void Start()
    {
        actualSetupTimeLaps = setupTimeLaps;

        if (doorKeys.Length > 0 && activationSwitch == null)
        {
            closeRot = movableDoorPivot.transform.localRotation.eulerAngles;
            switch (axisPivot)
            {
                case AxisPivot.X:
                    openRot = new Vector3(maxGrades, 0, 0);
                    break;
                case AxisPivot.Y:
                    openRot = new Vector3(0, maxGrades, 0);
                    break;
                case AxisPivot.Z:
                    openRot = new Vector3(0, 0, maxGrades);
                    break;
            }

            numKeys = doorKeys.Length;
            locked = lockState;
            opened = false;
            justUnlocked = true;
        }
        if (activationSwitch != null && doorKeys.Length == 0)
        {
            closeRot = movableDoorPivot.transform.localRotation.eulerAngles;
            switch (axisPivot)
            {
                case AxisPivot.X:
                    openRot = new Vector3(maxGrades, 0, 0);
                    break;
                case AxisPivot.Y:
                    openRot = new Vector3(0, maxGrades, 0);
                    break;
                case AxisPivot.Z:
                    openRot = new Vector3(0, 0, maxGrades);
                    break;
            }

            if (startOpened)
            {
                movableDoorPivot.transform.localRotation = Quaternion.Euler(openRot);
                justOpen = false;
                justClose = true;
            }
            if (!startOpened)
            {
                movableDoorPivot.transform.localRotation = Quaternion.Euler(closeRot);
                justOpen = true;
                justClose = false;
            }
        }
    }

    void Update()
    {
        if (actualSetupTimeLaps > 0)
        {
            actualSetupTimeLaps -= Time.deltaTime;
        }
        else
        {
            if (actualSetupTimeLaps != 0) actualSetupTimeLaps = 0;
        }
        if (doorKeys.Length > 0 && activationSwitch == null)
        {
            if (!locked)
            {
                if (cameraFeedback && actualSetupTimeLaps == 0 && justUnlocked)
                {
                    CameraController.instance.StartScriptedMovement(this.gameObject);
                }
                if (opened && movableDoorPivot.transform.rotation.eulerAngles != openRot)
                {
                    //Debug.Log("Opennig Door with key");
                    movableDoorPivot.transform.localRotation = Quaternion.Lerp(movableDoorPivot.transform.localRotation, Quaternion.Euler(openRot), Time.deltaTime * smoothmovement);
                }
            }
        }
        if (activationSwitch != null && doorKeys.Length == 0)
        {
            if (holdSwitchToOpen)
            {
                if (activationSwitch.IsHoldedSwitched())
                {
                    if (cameraFeedback && actualSetupTimeLaps == 0 && justOpen && scripteMovementsRepetitions > 0)
                    {
                        //Debug.Log("openning door camera feedback");
                        justClose = true;
                        justOpen = false;
                        scripteMovementsRepetitions--;
                        CameraController.instance.StartScriptedMovement(this.gameObject);
                    }
                    //Debug.Log("SwitchedActivated");
                    if (movableDoorPivot.transform.localRotation.eulerAngles != openRot)
                    {
                        //Debug.Log("Opennig Door");
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
                    if (cameraFeedback && actualSetupTimeLaps == 0 && justClose && scripteMovementsRepetitions > 0)
                    {
                        //Debug.Log("closing door camera feedback");
                        justOpen = true;
                        justClose = false;
                        scripteMovementsRepetitions--;
                        CameraController.instance.StartScriptedMovement(this.gameObject);
                    }
                    if (movableDoorPivot.transform.localRotation.eulerAngles != closeRot)
                    {
                        //Debug.Log("Closing Door");
                        movableDoorPivot.transform.localRotation = Quaternion.Lerp(movableDoorPivot.transform.localRotation, Quaternion.Euler(closeRot), Time.deltaTime * smoothmovement);
                    }
                    else
                    {
                        movableDoorPivot.transform.localRotation = Quaternion.Euler(closeRot);
                    }
                }
            }
            if (!holdSwitchToOpen)
            {
                if (activationSwitch.IsSwitched())
                {
                    if (cameraFeedback && actualSetupTimeLaps == 0 && justOpen && scripteMovementsRepetitions > 0)
                    {
                        //Debug.Log("openning door camera feedback");
                        justClose = true;
                        justOpen = false;
                        scripteMovementsRepetitions--;
                        CameraController.instance.StartScriptedMovement(this.gameObject);
                    }
                    //Debug.Log("SwitchedActivated");
                    if (movableDoorPivot.transform.localRotation.eulerAngles != openRot)
                    {
                        //Debug.Log("Opennig Door");
                        movableDoorPivot.transform.localRotation = Quaternion.Lerp(movableDoorPivot.transform.localRotation, Quaternion.Euler(openRot), Time.deltaTime * smoothmovement);
                    }
                    else
                    {
                        movableDoorPivot.transform.localRotation = Quaternion.Euler(openRot);
                    }
                }
                else
                if (!activationSwitch.IsSwitched())
                {
                    if (cameraFeedback && actualSetupTimeLaps == 0 && justClose && scripteMovementsRepetitions > 0)
                    {
                        //Debug.Log("closing door camera feedback");
                        justOpen = true;
                        justClose = false;
                        scripteMovementsRepetitions--;
                        CameraController.instance.StartScriptedMovement(this.gameObject);
                    }
                    if (movableDoorPivot.transform.localRotation.eulerAngles != closeRot)
                    {
                        //Debug.Log("Closing Door");
                        movableDoorPivot.transform.localRotation = Quaternion.Lerp(movableDoorPivot.transform.localRotation, Quaternion.Euler(closeRot), Time.deltaTime * smoothmovement);
                    }
                    else
                    {
                        movableDoorPivot.transform.localRotation = Quaternion.Euler(closeRot);
                    }
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
    public int GetNumKeys()
    {
        return numKeys;
    }
}
