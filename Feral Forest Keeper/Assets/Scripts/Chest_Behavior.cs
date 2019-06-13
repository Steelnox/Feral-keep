using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest_Behavior : MonoBehaviour
{
    public Animator chestAnimator;

    private bool open;

    void Start()
    {
        open = false;
    }

    void Update()
    {
        chestAnimator.SetBool("Open", open);
    }
    public void SetOpen(bool b)
    {
        open = b;
    }
}
