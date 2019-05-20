using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageRanged : State
{
    private float timerAttack;

    private RangedMove ranged;

    private float startRotation;

    private float currentRotation;

    public float rotationSpeed;

    public float shotAccuracy;



    public override void Enter()
    {

        ranged = GetComponent<RangedMove>();

        ranged.enemy_navmesh.isStopped = true;

        startRotation = transform.rotation.y;

        timerAttack = 0;



    }

    public override void Execute()
    {

        timerAttack += Time.deltaTime;

        currentRotation += Time.deltaTime + rotationSpeed;

        transform.localEulerAngles = new Vector3(0, startRotation + currentRotation, 0);

        if (timerAttack >= shotAccuracy)
        {
            Vector3 positionProjectile = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
            Instantiate(ranged.projectile, positionProjectile, transform.rotation);
            timerAttack = 0;
        }

        if (currentRotation >= 359.0f)
        {
            this.gameObject.SetActive(false);
        }
    }

    public override void Exit()
    {
       

    }
}
