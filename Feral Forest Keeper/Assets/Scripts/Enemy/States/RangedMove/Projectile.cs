﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private Transform myTransform;
    private PlayerController player;
    public float ProjectileSpeed;
    public int dmg;

    private float timer;
    public float deathTime;


    void Start()
    {
        player = PlayerController.instance;

        timer = 0;
        myTransform = transform;

        this.gameObject.SetActive(false);


    }

    void Update()
    {
        timer += Time.deltaTime;

        float Move = ProjectileSpeed * Time.deltaTime;
        myTransform.Translate(Vector3.forward * Move);

        if(timer >= deathTime)
        {
            timer = 0;
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player.GetDamage(dmg);
            timer = 0;

            this.gameObject.SetActive(false);

        }

        else if(other.tag == "MovableRock")
        {
            timer = 0;

            this.gameObject.SetActive(false);
        }

    }
}
