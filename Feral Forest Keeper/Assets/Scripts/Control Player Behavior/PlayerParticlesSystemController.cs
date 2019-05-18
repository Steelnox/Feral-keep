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

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void SetDashParticlesOnScene(Vector3 position)
    {
        dashParticlesComposite.PlayComposition(position);
        dashParticlesComposite.FaceComposite(PlayerController.instance.characterModel.transform.forward);
    }
}
