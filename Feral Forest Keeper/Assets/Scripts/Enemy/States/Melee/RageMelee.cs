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

        capsuleMesh.SetActive(false);

        explosionScript = explosion.GetComponent<ExplosionScript>();

        if(explosionScript.dmg_done)
        {
            explosionScript.dmg_done = false;
        }
    }

    public override void Execute()
    {

        explosion.SetActive(true);
        timerforDisabled += Time.deltaTime;
        if(timerforDisabled >= 1.2f)
        {
            this.gameObject.SetActive(false);
        }
    }

    public override void Exit()
    {
        explosion.SetActive(false);
        capsuleMesh.SetActive(true);

    }

}
