using System.Collections;
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

    public bool activated;

    public Vector3 startPosition;
    public Particles_Behavior trailParticles;

    void Start()
    {
        player = PlayerController.instance;

        timer = 0;
        myTransform = transform;

        startPosition = transform.position;

        activated = false;

    }

    void Update()
    {
        if (activated)
        {
            if (trailParticles.active != true) trailParticles.SetParticlesOnScene(transform.position);
            timer += Time.deltaTime;

            float Move = ProjectileSpeed * Time.deltaTime;
            myTransform.Translate(Vector3.forward * Move);

            if (timer >= deathTime)
            {
                timer = 0;
                activated = false;
            }
        }
       

        else
        {
            transform.position = startPosition;
            if (trailParticles.active != false) trailParticles.HideParticlesOutScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player.GetDamage(dmg);
            timer = 0;
            ParticlesFeedback_Control.instance.SetProjectileImpactCompositeOnScene(transform.position, transform.forward);
            activated = false;
        }

        else if(other.gameObject.layer != 12 && other.gameObject.layer != 11)
        {
            timer = 0;
            ParticlesFeedback_Control.instance.SetProjectileImpactCompositeOnScene(transform.position, transform.forward);
            activated = false;
        }

    }
}
