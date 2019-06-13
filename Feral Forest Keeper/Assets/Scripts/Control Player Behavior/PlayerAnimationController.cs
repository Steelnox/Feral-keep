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
    public FinishAnimationController finishAnimationController;

    void Start()
    {
        int runId = Animator.StringToHash("Run");
        int jumpId = Animator.StringToHash("Jump");
    }

    void Update()
    {
        animator.SetFloat("Velocity", GenericSensUtilities.instance.Transform3DTo2DMovement(PlayerController.instance.movement).magnitude);
        animator.SetFloat("X_Input", PlayerController.instance.X_Input);
        animator.SetFloat("Y_Input", PlayerController.instance.Z_Input);
        animator.SetFloat("PushDirection_X", PlayerController.instance.pushDirection.x);
        animator.SetFloat("PushDirection_Y", PlayerController.instance.pushDirection.z);
        SetPushinAnim(PlayerController.instance.pushing);
        animator.SetBool("Falling", PlayerController.instance.falling);
        animator.SetBool("FallingToDeath", PlayerController.instance.fallingToDeath);
        animator.SetBool("ShowWeapon", PlayerController.instance.showingWeapon);
        SetDeathByFall(PlayerController.instance.deathByFall);
    }
    public void SetTargetLockAnim(bool blocked)
    {
        if (animator.GetBool("TargetLocked") != blocked)
        {
            animator.SetBool("TargetLocked", blocked);
        }
    }
    public void AttackAnim()
    {
        animator.SetTrigger("Attack");
    }
    public void DashAnim()
    {
        animator.SetTrigger("Dash");
    }
    public void SetDashing(bool b)
    {
        animator.SetBool("Dashing", b);
    }
    public bool IsAnimationPlaying(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
    public void SetWeaponAnim(bool active)
    {
        if (animator.GetBool("Weapon") != active)
        {
            animator.SetBool("Weapon", active);
        }
    }
    public void SetPushinAnim(bool active)
    {
        if(animator.GetBool("Pushing") != active)
        {
            animator.SetBool("Pushing", active);
        }
    }
    public void SetGettingHitAnim(bool active)
    {
        if (animator.GetBool("GettingHit") != active)
        {
            animator.SetBool("GettingHit", active);
        }
    }
    public void SetDeathByFall(bool active)
    {
        if (animator.GetBool("DeathByFall") != active)
        {
            animator.SetBool("DeathByFall", active);
        }
    }
    public bool GetGettingHitAnimState()
    {
        return animator.GetBool("GettingHit");
    }
    public string GetActualAnimationPlayingName()
    {
        AnimatorClipInfo[] animInfo = animator.GetCurrentAnimatorClipInfo(0);
        return animInfo[0].clip.name;
    }
}
