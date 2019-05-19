﻿using System.Collections;
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

    void Start()
    {
        
    }
    void Update()
    {
        if(liveUpFeedbackParticlesComposite.IsCompositePlaying())
        {
            liveUpFeedbackParticlesComposite.transform.position = PlayerController.instance.transform.position + Vector3.up * 0.5f;
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
}
