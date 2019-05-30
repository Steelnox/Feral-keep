using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    #endregion

    public List<Item> items;

    public Item branchWeaponSlot;
    public Item leafSwordSlot;
    public Item powerGauntaletSlot;
    public int actualLeafQuantity;
    public int actualKeyQuantity;

    public GameObject branchWeaponForAnimations;
    public GameObject leafWeaponForAnimations;

    void Start()
    {
        branchWeaponSlot = GameManager.instance.branchItem;
        CheckIfHaveBranchWeaponItem();

        //branchWeaponForAnimations.SetActive(false);
        leafWeaponForAnimations.SetActive(false);
        //branchWeaponSlot = null;
        leafSwordSlot = null;
        powerGauntaletSlot = null;
        if (PlayerController.instance.startWithAllSkills)
        {
            leafSwordSlot = GameManager.instance.swordItem;
            CheckIfHaveLeafWeaponItem();

            Player_GUI_System.instance.SetOnScreenStrenfthForestIcon(true);

            powerGauntaletSlot = GameManager.instance.gantletItem;
        }

    }

    void Update()
    {
        /*if (leafSwordSlot == null) PlayerAnimationController.instance.SetLeafWeaponAnim(false);
        if (leafSwordSlot != null) PlayerAnimationController.instance.SetLeafWeaponAnim(true);*/
    }
    public void AddItemToInventary(Item i)
    {
        items.Add(i);
    }
    public void QuitItemElementFromItemsList(Item _i)
    {
        foreach(Item item in items)
        {
            if (item == _i)
            {
                items.Remove(item);
            }
        }
    }
    public void CheckIfHaveBranchWeaponItem()
    {
        foreach (Item item in items)
        {
            if (item.itemType == Item.ItemType.BRANCH_WEAPON)
            {
                branchWeaponSlot = item;
            }
        }
        if (branchWeaponSlot != null)
        {
            branchWeaponForAnimations.SetActive(true);
            PlayerAnimationController.instance.SetWeaponAnim(true);
            Player_GUI_System.instance.SetOnScreenBranchWeaponIcon(true);
        }
        else
        {
            PlayerAnimationController.instance.SetWeaponAnim(false);
            Player_GUI_System.instance.SetOnScreenBranchWeaponIcon(false);
        }
    }
    public void CheckIfHaveLeafWeaponItem()
    {
        foreach(Item item in items)
        {
            if (item.itemType == Item.ItemType.LEAF_WEAPON)
            {
                leafSwordSlot = item;
                branchWeaponSlot = null;
            }
        }
        if (leafSwordSlot != null)
        {
            branchWeaponForAnimations.SetActive(false);
            leafWeaponForAnimations.SetActive(true);
            Player_GUI_System.instance.SetOnScreenBranchWeaponIcon(false);
            PlayerAnimationController.instance.SetWeaponAnim(true);
            Player_GUI_System.instance.SetOnScreenLeafWeaponIcon(true);
        }
        else
        {
            PlayerAnimationController.instance.SetWeaponAnim(false);
            Player_GUI_System.instance.SetOnScreenLeafWeaponIcon(false);
        }
    }
    public void CountKeys()
    {
        int count = 0;
        foreach(Item item in items)
        {
            if (item.itemType == Item.ItemType.KEY) count++;
        }
        actualKeyQuantity = count;
    }
    public void CountLeafs()
    {
        int count = 0;
        foreach (Item item in items)
        {
            if (item.itemType == Item.ItemType.BRANCH_WEAPON) count++;
        }
        actualLeafQuantity = count;
    }
    public bool FindKeysInInventory(Item[] keysList)
    {
        bool result = false;
        int numKeys = 0;

        for (int i = 0; i < keysList.Length; i++)
        {
            foreach (Item item in items)
            {
                if (item == keysList[i])
                {
                    numKeys++;
                }
            }
        }
        if (numKeys == keysList.Length) result = true;
        return result;
    }
}
