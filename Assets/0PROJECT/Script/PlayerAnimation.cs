using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerManager pManager;
    private Animator anim;

    void Awake()
    {
        pManager = GetComponent<PlayerManager>();
        anim = pManager.structAnimation.anim;

        //CHECK ANIMATION STATE IN EVERY 0.2 SECONDS
        InvokeRepeating(nameof(AnimationState), 0f, 0.2f);
    }

    private void AnimationState()
    {
        //MAKE ALL STATE AS FALSE
        bool isIdle = false;
        bool isRunning = false;
        bool isDancing = false;
        bool isFalling = false;

        //MAKE STATES TO TRUE ACCORDING TO PLAYER STATE
        switch (pManager.playerStateEnum)
        {
            case PlayerState.IdlePhase:
                isIdle = true;
                break;
            case PlayerState.RunPhase:
                isRunning = true;
                break;
            case PlayerState.FinishPhase:
                isDancing = true;
                break;
            case PlayerState.FallingPhase:
                isFalling = true;
                break;
        }

        //SET STATES
        SetAnimationStates(isIdle, isRunning, isDancing, isFalling);
    }

    private void SetAnimationStates(bool isIdle, bool isRunning, bool isDancing, bool isFalling)
    {
        //SET STATES
        anim.SetBool("_isIdle", isIdle);
        anim.SetBool("_isRunning", isRunning);
        anim.SetBool("_isDancing", isDancing);
        anim.SetBool("_isFalling", isFalling);
    }
}