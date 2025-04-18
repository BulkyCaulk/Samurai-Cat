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
    }

    private void DownWardAttackHandler()
    {
        _playerAnimator.SetTrigger("downAttack");
    }
}
