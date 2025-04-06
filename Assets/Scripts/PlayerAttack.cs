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
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private float _attackRadius;
    private IEnumerator _attackCoroutine;
    private LayerMask _attackableBounceLayer;


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
            Debug.Log(_attackCoroutine);
            _playerRigidbody.AddForce(Vector2.up * _upwardForcePower, ForceMode2D.Impulse);
        }
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyGameObject))
        {
            enemyGameObject.TakeDamage(_attackDamage);
        }
    }
}