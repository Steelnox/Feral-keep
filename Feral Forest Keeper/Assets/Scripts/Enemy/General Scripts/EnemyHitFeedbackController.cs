using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitFeedbackController : MonoBehaviour
{
    public SkinnedMeshRenderer myMeshRenderer;
    public Enemy me;
    public float feedbackLenght;
    [SerializeField]
    private float hitFactor;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myMeshRenderer.material.SetFloat("_HitTintFactor", hitFactor);
        if (hitFactor > 0)
        {
            hitFactor -= Time.deltaTime / feedbackLenght;
            if (hitFactor <= 0.1) hitFactor = 0;
        }
    }
    public void Hit()
    {
        hitFactor = 1;
    }
}
