using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticlesSystemController : MonoBehaviour
{
    #region Singleton

    public static PlayerParticlesSystemController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    #endregion

    public ParticlesCompositeSystem dashParticlesComposite;
    public ParticlesCompositeSystem hitEnemiesParticlesComposite;
    public ParticlesCompositeSystem liveUpFeedbackParticlesComposite;
    public ParticlesCompositeSystem dashDuastTrailComposite;
    public Particles_Behavior walk_Dust_Trail_R;
    public Particles_Behavior walk_Dust_Trail_L;


    void Start()
    {
        
    }
    void Update()
    {
        if(liveUpFeedbackParticlesComposite.IsCompositePlaying())
        {
            liveUpFeedbackParticlesComposite.transform.position = PlayerController.instance.transform.position + Vector3.up * 0.5f;
            //liveUpFeedbackParticlesComposite.transform.rotation = PlayerController.instance.characterModel.transform.rotation;
        }
    }
    public void SetDashParticlesOnScene(Vector3 position)
    {
        dashParticlesComposite.PlayComposition(position);
        dashParticlesComposite.FaceComposite(PlayerController.instance.characterModel.transform.forward);
    }
    public void SetHitEnemiesParticlesOnScene(Vector3 position)
    {
        hitEnemiesParticlesComposite.PlayComposition(position);
    }
    public void SetLiveUpFeedbackParticlesOnScene(Vector3 position)
    {
        liveUpFeedbackParticlesComposite.PlayComposition(position);
    }
    public void SetDashDustTrailParticlesOnScene(Vector3 position)
    {
        dashDuastTrailComposite.PlayComposition(position);
    }
    public ParticlesCompositeSystem GetDashTrailComposite()
    {
        return dashDuastTrailComposite;
    }
    public void SetWalkDustTrail_R_ParticlesOnScene(Vector3 position)
    {
        walk_Dust_Trail_R.SetParticlesOnScene(position);
    }
    public void SetWalkDustTrail_L_ParticlesOnScene(Vector3 position)
    {
        walk_Dust_Trail_L.SetParticlesOnScene(position);
    }
}
