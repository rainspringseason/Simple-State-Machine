using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private string currentAnimation;

    #region Unity Methods
    private void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }
    #endregion

    #region Animations
    private void PlayAnimation(string animationName, bool useTransition = false, float transitionDuration = 0.1f)
    {
        if (animator == null) return;
        if (currentAnimation == animationName) return;

        currentAnimation = animationName;
        if (useTransition)
        {
            animator.CrossFade(animationName, transitionDuration);
        }
        else
        {
            animator.Play(animationName, -1, 0f);
        }
    }
    #endregion

    #region State Animations
    public void PlayIdle(Direction direction)
    {
        string animName = GetAnimationName("Idle", direction);
        PlayAnimation(animName);
    }

    public void PlayWalk(Direction direction)
    {
        string animName = GetAnimationName("Walk", direction);
        PlayAnimation(animName, true, 0.1f);
    }

    public void PlayAttack(Direction direction)
    {
        string animName = GetAnimationName("Attack", direction);
        PlayAnimation(animName);
    }

    public void PlayFirstAbility(Direction direction)
    {
        string animName = GetAnimationName("AbilityFirst", direction);
        PlayAnimation(animName);
    }

    public void PlaySecondAbility(Direction direction)
    {
        string animName = GetAnimationName("AbilitySecond", direction);
        PlayAnimation(animName);
    }
    #endregion

    #region Utility Methods
    public string GetAnimationName(string prefix, Direction direction)
    {
        string directionStr = DirectionHelper.DirectionToString(direction);
        return $"{prefix}_{directionStr}";
    }

    public float GetAnimationLength(string animationName)
    {
        if (animator == null) return 1f;

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        return 1f;
    }

    public bool IsAnimationPlaying(string animationName)
    {
        if (animator == null) return false;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName);
    }

    public float GetCurrentAnimationNormalizedTime()
    {
        if (animator == null) return 0f;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime;
    }
    #endregion
}
