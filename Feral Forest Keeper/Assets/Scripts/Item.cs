using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { BRANCH_WEAPON, LEAF_WEAPON, POWER_GANTLET, KEY, LIVE_UP};

    public ItemType itemType;
    public float interactionDistance;

    public Vector3 hidePos;
    public bool collected;

    public void Start()
    {

    }
    public void CollectItem()
    {
        collected = true;
        transform.position = hidePos;
    }
    public void SetItem(Vector3 pos, Vector3 rot)
    {
        if (collected == true) collected = false;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(rot);
    }
}
