using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesFeedback_Control : MonoBehaviour
{
    #region Singleton

    public static ParticlesFeedback_Control instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    #endregion

    public Particles_Behavior[] bushParticles;

    //void Start()
    //{
        
    //}

    //void Update()
    //{
        
    //}
    private Particles_Behavior FindFirstParticleNoActiveOnList(Particles_Behavior[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            if (!list[i].active)
            {
                return list[i];
            }
        }
        return null;
    }
    public void SetBushParticlesOnScene(Vector3 _pos)
    {
        FindFirstParticleNoActiveOnList(bushParticles).SetParticlesOnScene(_pos);
    }
}
