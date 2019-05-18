using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesCompositeSystem : MonoBehaviour
{
    public Particles_Behavior[] particles;
    //void Start()
    //{

    //}

    void Update()
    {
        if (!IsCompositePlaying()) transform.position = GameManager.instance.hidePos;
    }
    public void PlayComposition(Vector3 position)
    {
        transform.position = position;
        for(int i = 0; i < particles.Length; i++)
        {
            particles[i].SetParticlesOnScene(position);
        }
    }
    public bool IsCompositePlaying()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i].active) return true;
        }
        return false;
    }
    public void FaceComposite(Vector3 direction)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].transform.forward = direction;
        }
    }
}
