using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] Animator _playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        Locator.Instance.PlayerAttackScript.onDownwardAttackAnimation += DownWardAttackHandler;
        Locator.Instance.PlayerAttackScript.onDownwardAttackAnimationDuration += GetCurrentAnimationDurationHandler;
        Locator.Instance.PlayerMove.OnPlayerRun += PlayerRunHandler;
        Locator.Instance.PlayerMove.OnPlayerNotRun += PlayerNotRunHandler;
        Locator.Instance.PlayerMove.OnPlayerSprint += PlayerSprintHandler;
        Locator.Instance.PlayerMove.OnPlayerNotSprint += PlayerNotSprintHandler;
    }

    private void DownWardAttackHandler()
    {
        _playerAnimator.SetTrigger("downAttack");
    }
    private void PlayerRunHandler()
    {
        _playerAnimator.SetBool("isRunning", true);
    }
    private void PlayerNotRunHandler()
    {
        _playerAnimator.SetBool("isRunning", false);
    }

    private void PlayerSprintHandler()
    {
        _playerAnimator.SetBool("isSprinting", true);
    }
    private void PlayerNotSprintHandler()
    {
        _playerAnimator.SetBool("isSprinting", false);
    }

    private float GetCurrentAnimationDurationHandler(string animationName)
    {
        float duration = 0;
        AnimationClip[] allAnimations = _playerAnimator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip animation in allAnimations)
        {
            if(animation.name == animationName)
            {
                Debug.Log($"{animation.name} has length of: {animation.length}");
                duration = animation.length;
                break;
            }
            
        }
        
        if(duration == 0)
        {
            Debug.Log($"ERROR\\\\\\\\{animationName} is not a real animation\\\\\\\\\\\\");
        }

        return duration;
    }
}
