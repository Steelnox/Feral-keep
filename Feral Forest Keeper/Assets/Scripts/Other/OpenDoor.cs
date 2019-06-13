using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Switch_Behavior switch_door;
    public Animation animationDoor;
    public AnimationClip openDoor;

    public bool anim_done;

    void Start()
    {
        animationDoor = GetComponent<Animation>();
        anim_done = false;
    }

    void Update()
    {
        if (switch_door.switched && !anim_done)
        {
            animationDoor.clip = openDoor;
            animationDoor.Play();
            anim_done = true;
        }
    }
}
