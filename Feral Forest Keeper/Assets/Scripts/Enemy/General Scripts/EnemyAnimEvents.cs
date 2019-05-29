using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvents : MonoBehaviour
{

    public Melee melee_Main;

    public RangedMove move_Main;

    public RangedFixed fixed_Main;


    public void Move()
    {
        if (melee_Main != null) melee_Main.move = true;
        if (move_Main != null) move_Main.move = true;
    }

    public void StopMove()
    {
        if (melee_Main != null) melee_Main.move = false;
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
        if(move_Main!= null) move_Main.shoot = true;
        if (fixed_Main != null) fixed_Main.shoot = true;

    }

    public void FinishHit()
    {
        fixed_Main.finishHit = true;
    }
}
