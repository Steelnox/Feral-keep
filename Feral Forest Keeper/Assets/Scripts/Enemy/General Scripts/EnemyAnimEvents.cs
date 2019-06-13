using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvents : MonoBehaviour
{

    public Melee melee_Main;

    public RangedMove move_Main;

    public RangedFixed fixed_Main;

    [FMODUnity.EventRef]
    public string bounceEvent;

    [FMODUnity.EventRef]
    public string shootEvent;

    [FMODUnity.EventRef]
    public string walkEvent;

    public void Move()
    {
        if (melee_Main != null) melee_Main.move = true;
        if (move_Main != null) move_Main.move = true;
    }

    public void StopMove()
    {
        if (melee_Main != null)
        {
            melee_Main.move = false;
            FMODUnity.RuntimeManager.PlayOneShot(bounceEvent, transform.position);
        }

        if (move_Main != null) move_Main.move = false;
    }

    public void StartExplosion()
    {
        melee_Main.startexplosion = true;
    }
    public void FinishAttack()
    {
        melee_Main.finishAttack = true;
    }

    public void Shoot()
    {
        if (move_Main != null)
        {
            FMODUnity.RuntimeManager.PlayOneShot(shootEvent, transform.position);
            move_Main.shoot = true;
        }
        if (fixed_Main != null)
        {
            FMODUnity.RuntimeManager.PlayOneShot(shootEvent, transform.position);
            fixed_Main.shoot = true;
        }


    }

    public void FinishHit()
    {
        fixed_Main.finishHit = true;
    }

    public void StepSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(walkEvent, transform.position);
    }

}
