using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator : MonoBehaviour
{
    public static Locator Instance {get ; private set ; }
    [SerializeField] private PlayerAttack _playerAttackScript;
    [SerializeField] private AnimatorController _playerAnimatorController;

    public PlayerAttack PlayerAttackScript {get ; private set ; }
    public AnimatorController AnimatorControllerScript {get ; private set ; }
    // public UI UIScript { get ; private set ; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this; 

        PlayerAttackScript = _playerAttackScript;
        AnimatorControllerScript = _playerAnimatorController;
    }

    private void Start()
    {
        if(PlayerAttackScript == null)
        {
            Debug.Log("Locator is missing a reference to the PlayerAttack script");
        }
        if(AnimatorControllerScript == null)
        {
            Debug.Log("Locator is missing a reference to the AnimatorController script");
        }
    }
}
