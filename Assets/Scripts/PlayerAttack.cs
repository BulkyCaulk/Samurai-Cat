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

    [SerializeField] private float _bounceHeight = 2.5f;
    private IEnumerator _attackCoroutine;
    private LayerMask _attackableBounceLayer;
    // Event for when animation for downward attack should start
    public delegate void OnDownwardAttack();
    public event OnDownwardAttack onDownwardAttack;

    void Start()
    {
        _attackableBounceLayer = LayerMask.GetMask("BounceAttackable");
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.S))
        {
            _attackCoroutine = DownwardAttack();
            if(Input.GetKeyDown(_attackButton))
            {
                StartCoroutine(_attackCoroutine);
            }
        }
        else if(Input.GetKeyDown(_attackButton))
        {
            _attackCoroutine = BasicAttack();
            StartCoroutine(_attackCoroutine);
        }

        
    }

    private IEnumerator BasicAttack()
    {
        _basicAttackArea.SetActive(true);
        Collider2D objectHit = Physics2D.OverlapCircle(_basicAttackArea.transform.position,_attackRadius, _attackableBounceLayer);
        if(objectHit != null)
            CheckHit(objectHit);
        yield return new WaitForSeconds(_attackWaitTime);
        _basicAttackArea.SetActive(false);
    }

    private IEnumerator DownwardAttack()
    {
        _downwardAttackArea.SetActive(true);
        onDownwardAttack?.Invoke();
        Collider2D objectHit = Physics2D.OverlapCircle(_downwardAttackArea.transform.position,_attackRadius, _attackableBounceLayer);
        if(objectHit != null)
            CheckDownHit(objectHit);
        yield return new WaitForSeconds(_attackWaitTime);
        _downwardAttackArea.SetActive(false);
    }


    private void CheckHit(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyGameObject))
        {
            enemyGameObject.TakeDamage(_attackDamage);
        }
    }
    private void CheckDownHit(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<BounceAttackable>(out BounceAttackable bounceableObject))
        {
            float gravity = Physics2D.gravity.y * _playerRigidbody.gravityScale;
            float bounceVelocity = Mathf.Sqrt(2f * -gravity * _bounceHeight);

            Vector2 verticle = _playerRigidbody.velocity;
            verticle.y = 0f;

            verticle.y = bounceVelocity;
            _playerRigidbody.velocity = verticle;

        }
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyGameObject))
        {
            enemyGameObject.TakeDamage(_attackDamage);
        }
    }
}