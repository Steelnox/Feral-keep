using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTrail_Control : MonoBehaviour
{
    #region Singleton

    public static DashTrail_Control instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    #endregion

    public TrailRenderer trail;

    private bool startCount;
    private float count;

    void Start()
    {
        EnableTrail(false);
        HideTrail();
        count = 0;
    }
    void Update()
    {
        if (startCount)
        {
            count += Time.deltaTime;
            if (count > PlayerController.instance.dashCooldownTime * 0.8f)
            {
                EnableTrail(false);
                startCount = false;
                count = 0;
            }
        }
    }
    public void EnableTrail(bool b)
    {
        trail.enabled = b;
    }
    public void SetTrailIOnScene(Vector3 _pos, Vector3 _dir)
    {
        transform.position = _pos;
        transform.forward = -_dir;
        startCount = true;
        EnableTrail(true);
    }
    public void HideTrail()
    {
        transform.position = GameManager.instance.hidePos;
    }
}
