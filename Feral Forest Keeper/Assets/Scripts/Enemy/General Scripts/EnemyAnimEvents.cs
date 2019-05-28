using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvents : MonoBehaviour
{

    public Melee melee_Main;

    public RangedMove move_Main;

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

    public void FinishAttack()
    {
        melee_Main.finishAttack = true;
    }

    public void Shoot()
    {
        move_Main.shoot = true;
    }
}
