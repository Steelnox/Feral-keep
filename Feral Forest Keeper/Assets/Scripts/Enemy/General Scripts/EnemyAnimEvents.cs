using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvents : MonoBehaviour
{
    public Melee melee_Main;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Move()
    {
        melee_Main.move = true;
    }

    public void StopMove()
    {
        melee_Main.move = false;
    }

    public void FinishAttack()
    {
        melee_Main.finishAttack = true;
    }
}
