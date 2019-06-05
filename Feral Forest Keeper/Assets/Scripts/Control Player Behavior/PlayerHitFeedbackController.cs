using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHitFeedbackController : MonoBehaviour
{
    #region Singleton

    public static PlayerHitFeedbackController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    #endregion

    public PostProcessVolume globalVolume;
    public float feedbackLength;
    [SerializeField]
    private float vignetteFactor;
    [SerializeField]
    private Vignette vignette;
    [SerializeField]
    private bool fallHit;
    // Start is called before the first frame update
    void Start()
    {
        fallHit = false;
        if (globalVolume != null)
        {
            globalVolume.profile.TryGetSettings(out vignette);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (vignette != null)
        {
            vignette.intensity.value = vignetteFactor;
        }
        
        if (fallHit)
        {
            vignetteFactor = GameManager.instance.GetActualRespawnCooldown() / (GameManager.instance.respawnCoolDown * 2);
            if (GameManager.instance.GetActualRespawnCooldown() < 0.1f)
            {
                fallHit = false;
                vignetteFactor = 0;
            }
        }
        else
        {
            if (vignetteFactor > 0)
            {
                if (vignetteFactor <= 0.1) vignetteFactor = 0;
                vignetteFactor -= Time.deltaTime / feedbackLength;
            }
        }
    }
    public void Hit()
    {
        vignetteFactor = 0.5f;
    }
    public void FallHit()
    {
        fallHit = true;
    }
}
