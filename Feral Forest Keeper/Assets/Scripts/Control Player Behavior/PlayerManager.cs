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

    public Item branchWeapon;
    public Item leafSwordSlot;
    public Item powerGauntaletItem;
    public int actualLeafQuantity;
    public int actualKeyQuantity;

    void Start()
    {
        powerGauntaletItem = null;
        if (PlayerController.instance.startWithAllSkills)
        {
            PlayerAnimationController.instance.SetLeafWeaponAnim(true);
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
                branchWeapon = item;
            }
        }
        if (branchWeapon != null)
        {
            PlayerAnimationController.instance.SetLeafWeaponAnim(true);
            Player_GUI_System.instance.SetOnScreenLeafWeaponIcon(true);
        }
        else
        {
            PlayerAnimationController.instance.SetLeafWeaponAnim(false);
            Player_GUI_System.instance.SetOnScreenLeafWeaponIcon(false);
        }
    }
    public void CheckIfHaveLeafWeaponItem()
    {
        foreach(Item item in items)
        {
            if (item.itemType == Item.ItemType.LEAF_WEAPON)
            {
                leafSwordSlot = item;
            }
        }
        if (leafSwordSlot != null)
        {
            PlayerAnimationController.instance.SetLeafWeaponAnim(true);
            Player_GUI_System.instance.SetOnScreenLeafWeaponIcon(true);
        }
        else
        {
            PlayerAnimationController.instance.SetLeafWeaponAnim(false);
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
    public bool FindKeyInInventory(Item key)
    {
        foreach (Item item in items)
        {
            if (item == key)
            {
                return true;
            }
        }
        return false;
    }
}
