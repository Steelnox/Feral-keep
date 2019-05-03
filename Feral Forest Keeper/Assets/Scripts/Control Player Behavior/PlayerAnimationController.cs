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

    /*void Start()
    {
        
    }*/

    void Update()
    {
        animator.SetFloat("Velocity", GenericSensUtilities.instance.Transform3DTo2DMovement(PlayerController.instance.movement).magnitude);
        animator.SetFloat("X_Input", PlayerController.instance.X_Input);
        animator.SetFloat("Y_Input", PlayerController.instance.Z_Input);
        animator.SetFloat("PushDirection_X", PlayerController.instance.pushDirection.x);
        animator.SetFloat("PushDirection_Y", PlayerController.instance.pushDirection.z);
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
    public void SetLeafWeaponAnim(bool active)
    {
        animator.SetBool("WeaponLeaf", active);
    }
    public void SetPushinAnim(bool active)
    {
        animator.SetBool("Pushing", active);
    }
    public string GetActualAnimationPlayingName()
    {
        AnimatorClipInfo[] animInfo = animator.GetCurrentAnimatorClipInfo(0);
        return animInfo[0].clip.name;
    }
}
