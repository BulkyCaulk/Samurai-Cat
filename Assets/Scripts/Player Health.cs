using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _playerHealth;
    [SerializeField] private float _invincibilityTime;
    [SerializeField] private AudioClip deathAudioClip;
    private Rigidbody2D _playerRigidBody;
    private PlayerMovement _playerMovement;
    private Collider2D _playerCollider;
    private Knockback _knockback;
    private bool _isInvincible;
    private SpriteRenderer _playerSprite;
    private Material _defaultMat;
    private Material _injuredMat;
    private PlayerDeath playerDeath;
    private AudioSource _audioSource;

    void Start()
    {
        _isInvincible = false;
        _playerRigidBody = GetComponent<Rigidbody2D>();  
        _playerMovement = GetComponent<PlayerMovement>();
        _playerSprite = GetComponent<SpriteRenderer>();
        _knockback = GetComponent<Knockback>();
        _audioSource = GetComponent<AudioSource>();
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
        _audioSource.clip = deathAudioClip;
        _audioSource.Play();
        if (!_isInvincible)
        {
            _playerHealth--;
            _isInvincible = true;
            _knockback.CallKnockBackAction(-_knockbackDirection.normalized, _knockbackDirection.normalized, Input.GetAxisRaw("Horizontal"));
            StartCoroutine(TemporaryInvincibility());
        }
        if(_playerHealth <= 0)
        {
            IEnumerator Respawn()
            {
                _audioSource.clip = deathAudioClip;
                _audioSource.Play();
                yield return new WaitForSeconds(0.7f);
                GameManager.Instance.ReloadToCheckpoint();
            }
            StartCoroutine(Respawn());
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
