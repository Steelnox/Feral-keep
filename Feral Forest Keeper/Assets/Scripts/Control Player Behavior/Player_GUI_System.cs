using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player_GUI_System : MonoBehaviour
{
    #region Singleton

    public static Player_GUI_System instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    #endregion

    public Image healthBar;
    public Image dashBar;
    public RectTransform pushIcon;
    public RectTransform pickupIcon;
    public RectTransform unlockDoorIcon;
    public RectTransform activateSanctuaryIcon;
    public RectTransform leafWeaponIcon;
    public RectTransform branchWeaponIcon;
    public Text keysCount;

    private Vector2 leafWeaponIconOnScrenPos;
    private Vector2 actionIconOnScreenPos;
    private Vector2 hidePos;

    void Start()
    {
        leafWeaponIconOnScrenPos = leafWeaponIcon.anchoredPosition;
        actionIconOnScreenPos = pushIcon.anchoredPosition;
        hidePos = Vector2.right * 1000;
        SetOnScreenPushIcon(false);
        SetOnScreenPickUpIcon(false);
        SetOnScreenUnlockDoorIcon(false);
        SetOnScreenLeafWeaponIcon(false);
        SetOnScreenBranchWeaponIcon(false);
        SetOnScreenActivateSanctuaryIcon(false);
    }

    void Update()
    {
        dashBar.fillAmount = PlayerController.instance.dashCooldown / PlayerController.instance.dashCooldownTime;
        healthBar.fillAmount = PlayerController.instance.actualPlayerLive / PlayerController.instance.playerLive;
    }
    public void SetOnScreenPushIcon(bool b)
    {
        if (b)
        {
            pushIcon.anchoredPosition = actionIconOnScreenPos;
        }
        else
        {
            pushIcon.anchoredPosition = hidePos;
        }
    }
    public void SetOnScreenPickUpIcon(bool b)
    {
        if (b)
        {
            pickupIcon.anchoredPosition = actionIconOnScreenPos;
        }
        else
        {
            pickupIcon.anchoredPosition = hidePos;
        }
    }
    public void SetOnScreenUnlockDoorIcon(bool b)
    {
        if (b)
        {
            unlockDoorIcon.anchoredPosition = actionIconOnScreenPos;
        }
        else
        {
            unlockDoorIcon.anchoredPosition = hidePos;
        }
    }
    public void SetOnScreenLeafWeaponIcon(bool b)
    {
        if (b)
        {
            leafWeaponIcon.anchoredPosition = leafWeaponIconOnScrenPos;
        }
        else
        {
            leafWeaponIcon.anchoredPosition = hidePos;
        }
    }
    public void SetOnScreenBranchWeaponIcon(bool b)
    {
        if (b)
        {
            branchWeaponIcon.anchoredPosition = leafWeaponIconOnScrenPos;
        }
        else
        {
            branchWeaponIcon.anchoredPosition = hidePos;
        }
    }
    public void SetOnScreenActivateSanctuaryIcon(bool b)
    {
        if (b)
        {
            activateSanctuaryIcon.anchoredPosition = actionIconOnScreenPos;
        }
        else
        {
            activateSanctuaryIcon.anchoredPosition = hidePos;
        }
    }
    public void SetKeysCount(int keys)
    {
        keysCount.text = "" + keys;
    }
}

