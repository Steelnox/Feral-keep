using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageMelee : State
{
    public GameObject explosion;
    public GameObject capsuleMesh;
    public ExplosionScript explosionScript;

    private Melee melee;

    private float timerforDisabled;

    public override void Enter()
    {
        timerforDisabled = 0;

        melee = GetComponent<Melee>();

        melee.enemy_navmesh.isStopped = true;


        explosionScript = explosion.GetComponent<ExplosionScript>();

        melee.enemy_animator.SetBool("Destruction", true);

        if(explosionScript.dmg_done )
        {
            explosionScript.dmg_done = false;
        }
    }

    public override void Execute()
    {

        if (melee.startexplosion)
        {
            explosion.SetActive(true);

            capsuleMesh.SetActive(false);

        }
    }

    public override void Exit()
    {
        explosion.SetActive(false);
        capsuleMesh.SetActive(true);
    }

}
