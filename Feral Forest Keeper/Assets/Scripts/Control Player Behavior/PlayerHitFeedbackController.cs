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
    public AnimationCurve fadeInFallDeathCurveTime;
    public AnimationCurve fadeOutFallDeathCurveTime;
    [SerializeField]
    private float vignetteFactor;
    [SerializeField]
    private Vignette vignette;
    [SerializeField]
    private bool fallHit;
    [SerializeField]
    private ColorGrading colorGrading;
    [SerializeField]
    private float evaluateTime;
    [SerializeField]
    private float initialPostExposureValue;
    float curveValue;

    void Start()
    {
        fallHit = false;
        if (globalVolume != null)
        {
            globalVolume.profile.TryGetSettings(out vignette);
            globalVolume.profile.TryGetSettings(out colorGrading);
        }
        if (colorGrading != null)
        {
            initialPostExposureValue = colorGrading.postExposure.value;
        }
        curveValue = 0;
    }
    void Update()
    {
        if (vignette != null)
        {
            vignette.intensity.value = vignetteFactor;
        }
        
        if (fallHit)
        {
            if (GameManager.instance.GetActualRespawnCooldown() == GameManager.instance.respawnCoolDown)
            {
                curveValue += Time.deltaTime;
                float time = curveValue / 1.0f;
                if (curveValue < 1.0f)
                {
                    colorGrading.postExposure.value = fadeOutFallDeathCurveTime.Evaluate(time);
                }
                else
                {
                    colorGrading.postExposure.value = initialPostExposureValue;
                    fallHit = false;
                    evaluateTime = 0;
                    curveValue = 0;
                }
            }
            else
            if (colorGrading != null && GameManager.instance.GetActualRespawnCooldown() < GameManager.instance.respawnCoolDown)
            {
                evaluateTime += Time.deltaTime;
                float time = evaluateTime / GameManager.instance.respawnCoolDown;
                colorGrading.postExposure.value = fadeInFallDeathCurveTime.Evaluate(time);
            }
        }
        else
        {
            if (vignetteFactor > 0.1f)
            {
                vignetteFactor -= Time.deltaTime / feedbackLength;
            }
            else
            {
                if (vignetteFactor != 0) vignetteFactor = 0;
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
        fadeInFallDeathCurveTime.postWrapMode = WrapMode.ClampForever;
    }
}
