using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour
{
    Animator anim;

    //Animation Clips
    public AnimationClip[] _moveAnimations;
    public AnimationClip[] _hitAnimations;
    public AnimationClip[] _idleAnimations;
    public AnimationClip[] _deathAnimations;
    public AnimationClip[] _abilityAnimations;

    public enum animName
    {
        idle,
        move,
        death,
        hit,
        ability
    }

    #region "Initialisation"

    private void Awake()
    {
        VariablesSetter();
    }
    // Start is called before the first frame update
    void Start()
    {
        RandomiseAnimationStart();   
    }

    //This method will make sure that the zombies are not all moving in synchro
    private void RandomiseAnimationStart()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0); 
        anim.Play(state.fullPathHash, -1, UnityEngine.Random.Range(0f, 1f));
    }

    private void VariablesSetter()
    {
        anim = GetComponent<Animator>();
    }

    #endregion

    public void PlayAnimationClip(AnimationClip[] clipsArray, int index)
    {
        string animationName = clipsArray[index].name;     //Gets the name of the animation we have randomly picked

        Debug.Log(animationName);
        anim.Play(animationName);                               //Playes the animation we have picked

        //Debug.Log("The name of the currently played animation is: "+ animationName);
    }

    #region "Animation Player Methods"
    public void PlayRandomAnimationClip(AnimationClip[] clipsArray)
    {
        var randomClip = UnityEngine.Random.Range(0, clipsArray.Length);    //Test a random number so we can randomise the idle animation

        string animationName = clipsArray[randomClip].name;     //Gets the name of the animation we have randomly picked

        Debug.Log(animationName);
        anim.Play(animationName);                               //Playes the animation we have picked

        //Debug.Log("The name of the currently played animation is: "+ animationName);
    }

    public void PlayAnimation(animName anim, int index)
    {
        switch (anim)
        {
            case animName.idle:
                PlayAnimationClip(_idleAnimations, index);
                break;
            case animName.move:
                PlayAnimationClip(_moveAnimations, index);
                //PlayRandomAnimationClip(_idleAnimations);
                break;
            case animName.death:
                PlayAnimationClip(_deathAnimations, index);
                //PlayRandomAnimationClip(_idleAnimations);
                break;
            case animName.hit:
                PlayAnimationClip(_hitAnimations, index);
                //PlayRandomAnimationClip(_idleAnimations);
                break;
            case animName.ability:
                PlayAnimationClip(_abilityAnimations, index);
                //PlayRandomAnimationClip(_idleAnimations);
                break;
            default:
                return;
        }
    }

    public void PlayRandomAnimation(animName anim)
    {
        switch (anim)
        {
            case animName.idle:
                PlayRandomAnimationClip(_idleAnimations);
                break;
            case animName.move:
                PlayRandomAnimationClip(_moveAnimations);
                //PlayRandomAnimationClip(_idleAnimations);
                break;
            case animName.death:
                PlayRandomAnimationClip(_deathAnimations);
                //PlayRandomAnimationClip(_idleAnimations);
                break;
            case animName.hit:
                PlayRandomAnimationClip(_hitAnimations);
                //PlayRandomAnimationClip(_idleAnimations);
                break;
            case animName.ability:
                PlayRandomAnimationClip(_abilityAnimations);
                //PlayRandomAnimationClip(_idleAnimations);
                break;
            default:
                return;
        }
    }

    #endregion

    public bool IsPlaying()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) // needs changing
        {
            return false;
        }
        else
        {
            return anim.GetCurrentAnimatorStateInfo(0).length >
                   anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
    }

}
