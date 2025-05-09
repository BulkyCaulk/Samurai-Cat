using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private GameObject _spawnPosition;
    private float _timeSpan;
    private GameObject _projectile;
    private Vector2 _projectileDirection;
    public GameObject ProjectileSpawnPosition {get { return _spawnPosition ;} set {_spawnPosition = value ; } }
    public Vector2 ProjectileDirection { set {_projectileDirection = value; } }
    public GameObject ProjectileGameObject { set {_projectile = value; } }
    public float TimeSpan { set { _timeSpan = value ; } }
    public float ProjectileSpeed { set { _projectileSpeed = value ;} }

    void Start()
    {
        _timeSpan = 5;
    }
    public GameObject SpawnProjectileObject(Vector3 spawnPosition)
    {
        _projectile = Instantiate(_projectilePrefab, spawnPosition, Quaternion.identity);
        return _projectile;
    }

    void Update()
    {
        _timeSpan -= Time.deltaTime;
        if(_projectile != null)
        {
            Debug.Log($"{_projectile}");
            _projectile.transform.Translate(_projectileDirection * _projectileSpeed * Time.deltaTime);

        }
        if(_timeSpan <= 0)
        {
            _timeSpan = 3;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Hit {collision.name}");
        if(collision.name == "Player")
        {
            Vector2 kbDirection = Vector2.left;
            float directionXValue = collision.transform.position.x - this.transform.position.x;

            if(directionXValue > 0)
            {
                kbDirection = Vector2.right;
            }
            
            Locator.Instance.Player.PlayerTakeDamage(kbDirection);
            Destroy(gameObject);        
        }
        
    }
}
