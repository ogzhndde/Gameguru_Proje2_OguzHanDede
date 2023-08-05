using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerManager pManager;
    private Animator anim;

    void Awake()
    {
        pManager = GetComponent<PlayerManager>();
        anim = pManager.structAnimation.anim;

        InvokeRepeating(nameof(AnimationState), 0f, 0.2f);
    }

    private void AnimationState()
    {
        bool isIdle = false;
        bool isRunning = false;
        bool isDancing = false;
        bool isFalling = false;

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

        SetAnimationStates(isIdle, isRunning, isDancing, isFalling);
    }

    private void SetAnimationStates(bool isIdle, bool isRunning, bool isDancing, bool isFalling)
    {
        anim.SetBool("_isIdle", isIdle);
        anim.SetBool("_isRunning", isRunning);
        anim.SetBool("_isDancing", isDancing);
        anim.SetBool("_isFalling", isFalling);
    }
}