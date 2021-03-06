﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidBush_Behavior : MonoBehaviour
{
    public GameObject bush_Pivot;
    public float distanceInteraction;
    public float smoothMovement;
    public float blendScale;
    public Vector3 hidePos;
    public GameObject particlesPivot;
    public ParticleSystem cutParticles;

    private Vector3 desiredBlendVector;
    private Vector3 blendVector;
    private Vector3 initUpVector;
    private bool playerInteraction;
    private bool isCutted;
    //private bool justBeingCutted;
    private bool active;

    void Start()
    {
        initUpVector = bush_Pivot.transform.up;
        playerInteraction = false;
        hidePos = Vector3.down * 1000;
        isCutted = false;
        //justBeingCutted = true;
        active = true;
        HideParticles();
    }

    void Update()
    {
        if (active)
        {
            if (!isCutted)
            {
                if (GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerController.instance.playerRoot.transform.position, bush_Pivot.transform.position) < distanceInteraction)
                {
                    playerInteraction = true;
                }
                else
                {
                    playerInteraction = false;
                }
                if (playerInteraction)
                {
                    blendVector = GenericSensUtilities.instance.Transform2DTo3DMovement(GenericSensUtilities.instance.Transform3DTo2DMovement(GenericSensUtilities.instance.GetDirectionFromTo_N(PlayerController.instance.gameObject.transform.position, bush_Pivot.transform.position)));
                    desiredBlendVector = Vector3.up + blendVector * ((1 - ((GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerController.instance.playerRoot.transform.position, bush_Pivot.transform.position) / distanceInteraction))) * blendScale);
                    bush_Pivot.transform.up = Vector3.Lerp(bush_Pivot.transform.up, desiredBlendVector, smoothMovement * Time.deltaTime);
                }
                if (!playerInteraction && bush_Pivot.transform.up != initUpVector)
                {
                    bush_Pivot.transform.up = Vector3.Lerp(bush_Pivot.transform.up, initUpVector, (smoothMovement * 2) * Time.deltaTime);
                }
            }
            /*if (isCutted && justBeingCutted)
            {
                Debug.Log("Bush Cutted");
                if (cutParticles.particleCount == 0)
                {
                    Debug.Log("Bush particlesCOuint == 0");
                    HideParticles();
                    ResetParticles();
                    justBeingCutted = false;
                    active = false;
                }
            }*/
        }
    }
    public void CutBush()
    {
        Vector3 bushPos = bush_Pivot.transform.position;
        isCutted = true;
        SetParticles(bushPos);
        HideBush();
        PlayParticles();
    }
    private void SetParticles(Vector3 _pos)
    {
        particlesPivot.transform.position = _pos;
    }
    private void HideParticles()
    {
        particlesPivot.transform.position = hidePos;
    }
    private void HideBush()
    {
        this.transform.position = hidePos;
    }
    private void SetBush(Vector3 _pos)
    {
        active = true;
        isCutted = false;
        //justBeingCutted = true;
        this.transform.position = _pos;
    }
    private void PlayParticles()
    {
        cutParticles.Play();
    }
    private void ResetParticles()
    {
        cutParticles.Stop();
    }
    public void OnTriggerEnter(Collider other)
    {
        //PlayerController p = other.gameObject.GetComponent<PlayerController>();
        if (other.tag == "PlayerWeapon")
        {
            CutBush();
        }
    }
}
