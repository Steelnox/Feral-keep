using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponColliderChecker : MonoBehaviour
{
    #region Singleton

    public static PlayerWeaponColliderChecker instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    #endregion
    [SerializeField]
    private bool enemyContact;

    //void Start()
    //{

    //}

    //void Update()
    //{

    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.tag == "Enemy")
            {
                enemyContact = true;
                ParticlesFeedback_Control.instance.SetHitEnemyParticlesOnScene(other.gameObject.transform.position + Vector3.up * 0.5f);
            }
            if (other.tag == "StaticBush")
            {
                if (PlayerManager.instance.leafSwordSlot == null)
                {
                    ParticlesFeedback_Control.instance.SetHitStaticBushParticlesOnScene(other.ClosestPoint(transform.position));
                }
                Debug.Log("Hitting StaticBush");
            }
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other != null)
        {
            if (other.tag == "Enemy")
            {
                enemyContact = true;
            }
            else
            {
                enemyContact = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            if (other.tag == "Enemy")
            {
                enemyContact = false;
            }
        }
    }
    public bool GetEnemyContact()
    {
        return enemyContact;
    }
}
