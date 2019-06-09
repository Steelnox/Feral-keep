using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles_Behavior : MonoBehaviour
{
    public ParticleSystem particles;
    public bool active;
    public Vector3 hidePos;

    void Start()
    {
        active = false;
    }

    void Update()
    {
        if (active)
        {
            if (particles.isStopped)
            {
                active = false;
                HideParticlesOutScene();
            }
        }
    }
    private void PlayParticleSystem()
    {
        particles.Play();
        active = true;
    }
    private void SetParticlesPosition(Vector3 _pos)
    {
        transform.position = _pos;
    }
    public void HideParticlesOutScene()
    {
        SetParticlesPosition(hidePos);
        particles.Stop();
    }
    public void SetParticlesOnScene(Vector3 location)
    {
        SetParticlesPosition(location);
        PlayParticleSystem();
    }
    public void StopParticles()
    {
        particles.Stop();
    }
}
