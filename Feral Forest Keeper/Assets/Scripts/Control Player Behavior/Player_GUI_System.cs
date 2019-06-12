using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player_GUI_System : MonoBehaviour
{
    #region Singleton

    public static Player_GUI_System instance;

    //private void Awake()
    //{
        
    //}

    #endregion

    public Image healthBar;
    public Image dashBar;
    public RectTransform pushIcon;
    public RectTransform pickupIcon;
    public RectTransform unlockDoorIcon;
    public RectTransform activateSanctuaryIcon;
    public RectTransform buttonBColor;
    public RectTransform buttonBSimon;
    public RectTransform buttonBWoodSign;

    public RectTransform leafWeaponIcon;
    public RectTransform branchWeaponIcon;
    public RectTransform strengthForestIcon;
    public Text keysCount;

    private Vector2 leafWeaponIconOnScrenPos;
    private Vector2 actionIconOnScreenPos;
    private Vector2 strengthIconOnScreenPos;
    private Vector2 hidePos;

    void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);

        leafWeaponIconOnScrenPos = leafWeaponIcon.anchoredPosition;
        actionIconOnScreenPos = buttonBColor.anchoredPosition;
        strengthIconOnScreenPos = strengthForestIcon.anchoredPosition;

        hidePos = Vector2.right * 1000;
        SetOnScreenPushIcon(false);
        SetOnScreenPickUpIcon(false);
        SetOnScreenUnlockDoorIcon(false);
        SetOnScreenLeafWeaponIcon(false);
        SetOnScreenBranchWeaponIcon(false);
        SetOnScreenActivateSanctuaryIcon(false);
        SetOnScreenStrenfthForestIcon(false);
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
    public void SetOnScreenStrenfthForestIcon(bool b)
    {
        if (b)
        {
            strengthForestIcon.anchoredPosition = strengthIconOnScreenPos;
        }
        else
        {
            strengthForestIcon.anchoredPosition = hidePos;
        }
    }

    public void SetOnScreenButtonBColor(bool b)
    {
        if (b)
        {
            buttonBColor.anchoredPosition = actionIconOnScreenPos;
        }
        else
        {
            buttonBColor.anchoredPosition = hidePos;
        }
    }

    public void SetOnScreenButtonBSimon(bool b)
    {
        if (b)
        {
            buttonBSimon.anchoredPosition = actionIconOnScreenPos;
        }
        else
        {
            buttonBSimon.anchoredPosition = hidePos;
        }
    }

    public void SetOnScreenButtonBWoodSign(bool b)
    {
        if (b)
        {
            buttonBWoodSign.anchoredPosition = actionIconOnScreenPos;
        }
        else
        {
            buttonBWoodSign.anchoredPosition = hidePos;
        }
    }

    public void SetKeysCount(int keys)
    {
        keysCount.text = "" + keys;
    }
}

