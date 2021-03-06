﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageRanged : State
{
    private RangedMove ranged;

    private float startRotation;

    private float currentRotation;

    public float rotationSpeed;

    public float shotAccuracy;

    private Projectile projectile;

    [FMODUnity.EventRef]
    public string dieEvent;

    public override void Enter()
    {

        ranged = GetComponent<RangedMove>();

        ranged.enemy_navmesh.isStopped = true;

        startRotation = transform.rotation.y;

        ranged.enemy_animator.SetBool("Attack", true);

    }

    public override void Execute()
    {

        currentRotation += Time.deltaTime + rotationSpeed;

        transform.localEulerAngles = new Vector3(0, startRotation + currentRotation, 0);

        if (ranged.shoot)
        {
            Vector3 positionProjectile = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
            projectile = ranged.gamemanagerScript.GetProjectileNotActive();
            projectile.transform.position = positionProjectile;
            projectile.transform.rotation = transform.rotation;
            ranged.shoot = false;
        }

        if (currentRotation >= 359.0f)
        {
            ranged.ChangeState(ranged.chase);


        }
    }

    public override void Exit()
    {
        FMODUnity.RuntimeManager.PlayOneShot(dieEvent, transform.position);

        currentRotation = 0;
        ranged.currentHealth = 0;
        ranged.enemy_animator.SetBool("Attack", false);

    }
}
