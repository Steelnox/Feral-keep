using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableDoors : MonoBehaviour
{
    public GameObject movableDoorPivot;
    public GameObject doorBody;
    public float interactionDistance;
    public bool cameraFeedback;
    public float scripteMovementsRepetitions;
    public bool startOpened;
    public float setupTimeLaps;

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
    private bool justUnlocked;
    private bool justOpen;
    private bool justClose;
    [SerializeField]
    private float actualSetupTimeLaps;

    void Start()
    {
        actualSetupTimeLaps = setupTimeLaps;

        if (doorKey != null && activationSwitch == null)
        {
            closeRot = movableDoorPivot.transform.localRotation.eulerAngles;
            openRot = new Vector3(0, 120, 0);

            locked = lockState;
            opened = false;
            justUnlocked = true;
        }
        if (activationSwitch != null && doorKey == null)
        {
            closeRot = movableDoorPivot.transform.localRotation.eulerAngles;
            openRot = new Vector3(0, 120, 0);
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
        if (doorKey != null && activationSwitch == null)
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
        if (activationSwitch != null && doorKey == null)
        {
            if (activationSwitch.IsHoldedSwitched())
            {
                if (cameraFeedback && actualSetupTimeLaps == 0 && justOpen && scripteMovementsRepetitions > 0)
                {
                    Debug.Log("openning door camera feedback");
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
                    Debug.Log("closing door camera feedback");
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
