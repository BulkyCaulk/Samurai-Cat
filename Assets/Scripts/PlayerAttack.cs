using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float _attackDamage;
    [SerializeField] private GameObject _basicAttackArea;
    [SerializeField] private GameObject _downwardAttackArea;
    [SerializeField] private KeyCode _attackButton;
    [SerializeField] private Rigidbody2D _playerRigidbody;
    [SerializeField] private float _upwardForcePower;
    [SerializeField] private float _attackWaitTime;
    [SerializeField] private float _attackRadius;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private float _bounceHeight = 2.5f;
    [SerializeField] private AudioSource _playSFX;
    [SerializeField] private BoxCollider2D _playerBoxCollider;
    [SerializeField] private BoxCollider2D _playerDownAttackCollider;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private GameObject attackVisual;
    private ParticleSystem particleInstance;
    private GameObject attackVisualInstance;

    private Knockback _knockback;
    private IEnumerator _attackCoroutine;
    private LayerMask _attackableBounceLayer;
    // Event for when animation for downward attack should start
    public delegate void OnDownwardAttack();
    public delegate float OnDownwardAttackFloat(string animationName);
    public event OnDownwardAttack onDownwardAttackAnimation;
    public event OnDownwardAttack onDownwardAttackHit;
    public event OnDownwardAttackFloat onDownwardAttackAnimationDuration;
    public Collider2D objectHit = null;
    private float _animationDuration;
    private bool _canAttack = true;
    private float _pogoDuration;


    void Start()
    {
        _knockback = GetComponent<Knockback>();
        _attackableBounceLayer = LayerMask.GetMask("BounceAttackable");
        _pogoDuration = 1f;
    }
    // Update is called once per frame
    void Update()
    {
        if (_knockback.IsBeingKnockedBack)
        {
            return;
        }
        // should put attack timer here too? 
        if (Input.GetKeyDown(_attackButton) && _canAttack == true)
        {
            _attackCoroutine = DownwardAttack();
            StartCoroutine(_attackCoroutine);
        }
        
    }


    private IEnumerator BasicAttack()
    {
        _basicAttackArea.SetActive(true);
        Collider2D objectHit = Physics2D.OverlapCircle(_basicAttackArea.transform.position, _attackRadius, _attackableBounceLayer);
        if (objectHit != null)
            CheckHit(objectHit);
        yield return new WaitForSeconds(_attackWaitTime);
        _basicAttackArea.SetActive(false);
    }

    private IEnumerator DownwardAttack()
    {
        //_downwardAttackArea.SetActive(true);
        // _playerBoxCollider.enabled = false;
        _canAttack = false;
        onDownwardAttackAnimation?.Invoke();
        _animationDuration = onDownwardAttackAnimationDuration.Invoke("DownwardAttack");
        SpawnHitVisuals();

        _basicAttackArea.SetActive(true);
        float timer = 0;
        while (timer < _pogoDuration)
        {
            objectHit = Physics2D.OverlapCircle(_downwardAttackArea.transform.position, _attackRadius, _attackableBounceLayer);
            if (objectHit != null)
            {
                CheckDownHit(objectHit);
                break;
            }
            timer += Time.deltaTime;
        }

        _basicAttackArea.SetActive(false);
        onDownwardAttackHit?.Invoke();
        yield return new WaitForSeconds(_animationDuration);
        _canAttack = true;
        // should not wait for attack wait time to be done, instead wait till animation is done playing then turn back on
        // gonna move it up above or better calculate animation time and place it in wait for seconds 
        //_downwardAttackArea.SetActive(false);
        // _playerBoxCollider.enabled = true;
    }


    private void CheckHit(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyGameObject))
        {
            enemyGameObject.TakeDamage(_attackDamage);
        }

    }
    private void CheckDownHit(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<BounceAttackable>(out BounceAttackable bounceableObject))
        {
            float gravity = Physics2D.gravity.y * _playerRigidbody.gravityScale;
            float bounceVelocity = Mathf.Sqrt(2f * -gravity * _bounceHeight);

            Vector2 verticle = _playerRigidbody.velocity;
            verticle.y = 0f;

            verticle.y = bounceVelocity;
            _playerRigidbody.velocity = verticle;
            SpawnHitParticles();
            _playSFX.clip = _playerData._downWardAttack;
            _playSFX.Play();
        }
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyGameObject))
        {
            enemyGameObject.TakeDamage(_attackDamage);
        }
        if (collision.gameObject.TryGetComponent<Boss>(out Boss bossGameObject))
        {
            bossGameObject.TakeDamage();
        }
    }
    // Spawns the particle effect below the player
    private void SpawnHitParticles()
    {
        Vector3 currentTransform = transform.position;
        Quaternion spawnRotation = particle.transform.rotation;
        currentTransform.y = (float)(currentTransform.y - 1.4);
        particleInstance = Instantiate(particle, currentTransform, spawnRotation);
    }

    //
    private void SpawnHitVisuals()
    {
        Vector3 currentTransform = transform.position;
        currentTransform.y = (float)(currentTransform.y - 0.95);
        attackVisualInstance = Instantiate(attackVisual, currentTransform, Quaternion.identity);
        IEnumerator DestroyVisual()
        {
            yield return new WaitForSeconds((float)0.1);
            Destroy(attackVisualInstance);
        }
        StartCoroutine(DestroyVisual());
    }
}