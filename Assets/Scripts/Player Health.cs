using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _playerHealth;
    [SerializeField] private float _invincibilityTime;
    private Rigidbody2D _playerRigidBody;
    private PlayerMovement _playerMovement;
    private Knockback _knockback;
    private bool _isInvincible;
    private SpriteRenderer _playerSprite;
    private Material _defaultMat;
    private Material _injuredMat;

    void Start()
    {
        _isInvincible = false;
        _playerRigidBody = GetComponent<Rigidbody2D>();  
        _playerMovement = GetComponent<PlayerMovement>();
        _playerSprite = GetComponent<SpriteRenderer>();
        _knockback = GetComponent<Knockback>();
        _defaultMat = _playerSprite.material;
        _injuredMat = Resources.Load("mWhite", typeof(Material)) as Material;
    }


    void Update()
    {
        //quit game 
        if(Input.GetKey("9") && Input.GetKey("1"))
        {
            Application.Quit();
        }
    }


    public void PlayerTakeDamage(Vector2 _knockbackDirection)
    {
        if(!_isInvincible)
        {
            _playerHealth--;
            _isInvincible = true;
            _knockback.CallKnockBackAction(-_knockbackDirection.normalized, Vector2.down + Vector2.left, Input.GetAxisRaw("Horizontal"));
            StartCoroutine(TemporaryInvincibility());
        }
        if(_playerHealth <= 0)
        {
            GameManager.Instance.ReloadToCheckpoint();
        }
    }


    private IEnumerator TemporaryInvincibility()
    {

        yield return DamageFlasher();
        _isInvincible = false;
    }


    private IEnumerator DamageFlasher()
    {
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(_invincibilityTime);
            _playerSprite.material = _injuredMat;
            Invoke("ResetMaterial", _invincibilityTime - .23f);
        }
    }

    private void ResetMaterial()
    {
        _playerSprite.material = _defaultMat;
    }
}
