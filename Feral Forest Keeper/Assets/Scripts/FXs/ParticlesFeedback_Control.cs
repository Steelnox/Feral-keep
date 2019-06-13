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

    public Particles_Behavior[] bushParticlesPOOL;
    public ParticlesCompositeSystem[] hitEnemyParticlesPOOL;
    public ParticlesCompositeSystem[] itemsScrollUpSparksParticlesPOOL;
    public ParticlesCompositeSystem[] hitStaticBushParticlesCompositePOOL;
    public ParticlesCompositeSystem[] projectileImpactCompositePOOL;

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
    private ParticlesCompositeSystem FindFirstParticleCompositeNoActiveOnList(ParticlesCompositeSystem[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            if (!list[i].IsCompositePlaying())
            {
                return list[i];
            }
        }
        return null;
    }
    public void SetBushParticlesOnScene(Vector3 _pos)
    {
        FindFirstParticleNoActiveOnList(bushParticlesPOOL).SetParticlesOnScene(_pos);
    }
    public void SetHitEnemyParticlesOnScene(Vector3 _pos)
    {
        FindFirstParticleCompositeNoActiveOnList(hitEnemyParticlesPOOL).PlayComposition(_pos);
    }
    public void SetItemsScrollUpSparksCompositeOnScene(Vector3 _pos)
    {
        FindFirstParticleCompositeNoActiveOnList(itemsScrollUpSparksParticlesPOOL).PlayComposition(_pos);
    }
    public void SetHitStaticBushParticlesOnScene(Vector3 _pos)
    {
        FindFirstParticleCompositeNoActiveOnList(hitStaticBushParticlesCompositePOOL).PlayComposition(_pos);
    }
    public void SetProjectileImpactCompositeOnScene(Vector3 _pos, Vector3 _dir)
    {
        ParticlesCompositeSystem projImp = GetNOActiveCompoisteOnList(projectileImpactCompositePOOL);
        if (projImp != null)
        {
            projImp.FaceComposite(_dir);
            projImp.PlayComposition(_pos);
        }
    }
    public void HideComposite(ParticlesCompositeSystem composite)
    {
        composite.HideComposition();
    }
    public ParticlesCompositeSystem GetNOActiveCompoisteOnList(ParticlesCompositeSystem[] list)
    {
        return FindFirstParticleCompositeNoActiveOnList(list);
    }
}
