/*using System.Collections;
using System.Collections.Generic;*/
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelControl[] levelList;
    public float respawnCoolDown;

    [SerializeField]
    private int activeLevelID;
    [SerializeField]
    private LevelGates nextLevelGateIN;
    [SerializeField]
    private LevelGates actualLevelGateOUT;
    [SerializeField]
    private LevelGates actualLevelGateIN;
    [SerializeField]
    private LevelGates previousLevelOUT;
    [SerializeField]
    private bool changingLevel;
    private float actualRespawnCoolDown;

    void Start ()
    {
        actualRespawnCoolDown = respawnCoolDown;
        levelList[0].SetActive(true);
        activeLevelID = 0;
        if (levelList[activeLevelID] != null)
        {
            if (levelList[activeLevelID].levelGateOUT != null) actualLevelGateOUT = levelList[activeLevelID].levelGateOUT;
            if (levelList[activeLevelID].levelGateIN != null) actualLevelGateIN = levelList[activeLevelID].levelGateIN;
        }
        if (levelList.Length > 1 && levelList[activeLevelID + 1] != null)
        {
            if (levelList[activeLevelID + 1].levelGateIN != null) nextLevelGateIN = levelList[activeLevelID + 1].levelGateIN;
        }

        for (int i = 1; i < levelList.Length; i++)
        {
            levelList[i].SetActive(false);
        }
        PlayerController.instance.transform.position = levelList[activeLevelID].levelCheckPoint.transform.position;
    }
	
	void Update ()
    {
		if (actualLevelGateOUT != null && actualLevelGateOUT.IsBeingUsed())
        {
            actualLevelGateOUT.ResetGate();
            PlayerController.instance.MoveBetweenGates(nextLevelGateIN.gameObject);
            NextLevel();
        }
        if (actualLevelGateIN != null && actualLevelGateIN.IsBeingUsed())
        {
            actualLevelGateIN.ResetGate();
            PlayerController.instance.MoveBetweenGates(previousLevelOUT.gameObject);
            PreviousLevel();
        }
        if (changingLevel)
        {
            //_camera.SetActualBehavior(CameraController.Behavior.CHANGE_LEVEL);
            if (CameraController.instance.GetActualBehavior() == CameraController.Behavior.FOLLOW_PLAYER)
            {
                PlayerController.instance.SetCanMove(true);
                changingLevel = false;
            }
        }
        /////// PLAYER DEATH CONTROLER ////////
        if (!PlayerController.instance.playerAlive)
        {
            actualRespawnCoolDown = respawnCoolDown;
            PlayerController.instance.p_controller.enabled = false;
            PlayerController.instance.SetCanMove(false);
            PlayerController.instance.transform.position = levelList[activeLevelID].levelCheckPoint.transform.position;
            CameraController.instance.SetActualBehavior(CameraController.Behavior.PLYER_DEATH);

            if (PlayerController.instance.transform.position == levelList[activeLevelID].levelCheckPoint.transform.position)
            {
                Debug.Log("After death, player on CheckPointPosition");
                PlayerController.instance.actualPlayerLive = PlayerController.instance.playerLive;
                PlayerController.instance.playerAlive = true;
                PlayerController.instance.SetCanMove(true);
                PlayerController.instance.p_controller.enabled = true;
            }
        }
    }

    public void NextLevel()
    {
        if (levelList[activeLevelID] != null)
        {
            if (levelList[activeLevelID].levelGateOUT != null)
            {
                previousLevelOUT = levelList[activeLevelID].levelGateOUT;
            }
            else
            {
                previousLevelOUT = null;
            }
        }
        else
        {
            previousLevelOUT = null;
        }
        if (levelList[activeLevelID + 1] != null)
        {
            if (levelList[activeLevelID + 1].levelGateOUT != null)
            {
                actualLevelGateOUT = levelList[activeLevelID + 1].levelGateOUT;
            }
            else
            {
                actualLevelGateOUT = null;
            }
            if (levelList[activeLevelID + 1].levelGateIN != null)
            {
                actualLevelGateIN = levelList[activeLevelID + 1].levelGateIN;
            }
            else
            {
                actualLevelGateIN = null;
            }
        }
        if (activeLevelID + 2 < levelList.Length)
        {
            if (levelList[activeLevelID + 2] != null)
            {
                if (levelList[activeLevelID + 2].levelGateIN != null)
                {
                    nextLevelGateIN = levelList[activeLevelID + 2].levelGateIN;
                }
                else
                {
                    nextLevelGateIN = null;
                }
            }
        }
        else
        {
            nextLevelGateIN = null;
        }
        
        levelList[activeLevelID].SetActive(false);
        levelList[activeLevelID + 1].SetActive(true);
        activeLevelID++;
        PlayerController.instance.SetCanMove(false);
        CameraController.instance.SetActualBehavior(CameraController.Behavior.CHANGE_LEVEL);
        changingLevel = true;
        //Debug.Log("entro");
    }
    public void PreviousLevel()
    {
        if (activeLevelID - 2 >= 0)
        {
            if (levelList[activeLevelID - 2] != null)
            {
                if (levelList[activeLevelID - 2].levelGateOUT != null)
                {
                    previousLevelOUT = levelList[activeLevelID - 2].levelGateOUT;
                }
                else
                {
                    previousLevelOUT = null;
                }
            }
            else
            {
                previousLevelOUT = null;
            }
        }
        
        if (levelList[activeLevelID - 1] != null)
        {
            if (levelList[activeLevelID - 1].levelGateOUT != null)
            {
                actualLevelGateOUT = levelList[activeLevelID - 1].levelGateOUT;
            }
            else
            {
                actualLevelGateOUT = null;
            }
            if (levelList[activeLevelID - 1].levelGateIN != null)
            {
                actualLevelGateIN = levelList[activeLevelID - 1].levelGateIN;
            }
            else
            {
                actualLevelGateIN = null;
            }
        }
        if (levelList[activeLevelID] != null)
        {
            if (levelList[activeLevelID].levelGateIN != null)
            {
                nextLevelGateIN = levelList[activeLevelID].levelGateIN;
            }
            else
            {
                nextLevelGateIN = null;
            }
        }
        else
        {
            nextLevelGateIN = null;
        }

        levelList[activeLevelID].SetActive(false);
        levelList[activeLevelID - 1].SetActive(true);
        activeLevelID--;
        PlayerController.instance.SetCanMove(false);
        CameraController.instance.SetActualBehavior(CameraController.Behavior.CHANGE_LEVEL);
        changingLevel = true;
    }
}
