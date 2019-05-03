using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    #region Singleton

    public static PlayerAnimationController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    #endregion

    public Animator animator;

    void Start()
    {
        
    }

    void Update()
    {
        animator.SetFloat("Velocity", GenericSensUtilities.instance.Transform3DTo2DMovement(PlayerController.instance.movement).magnitude);
    }
    public void SetTargetLockAnim(bool blocked)
    {
        animator.SetBool("TargetLocked", blocked);
    }
    public void AttackAnim()
    {
        animator.SetTrigger("Attack");
    }
    public bool IsAnimationPlaying(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
    public void SetLeafWeaponAnim(bool b)
    {
        animator.SetBool("WeaponLeaf", b);
    }

}
