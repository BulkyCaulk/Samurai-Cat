using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileShooter1 : MonoBehaviour
{

    [SerializeField] private float _nextShotTimer; 
    [SerializeField] private SpawnProjectile _spawnProjectile;
    
    private GameObject _player;
    private Vector3 _playerLastPosition;
    private Vector3 _playerConstantPosition;
    private Vector3 _barrelDirection;
    private Vector3 _spawnPosition;
    private float _timer;


    void Start()
    {
        _timer = _nextShotTimer;
        _spawnPosition = this.gameObject.transform.GetChild(0).gameObject.transform.position;
        _player = GameObject.Find("Player");  
        _barrelDirection = this.transform.up;
    }


    void Update()
    {
        _timer -= Time.deltaTime;

        _playerConstantPosition = _player.transform.position - this.transform.position;

        
        if(_timer < 0)
        {
            _timer = _nextShotTimer;
            StartCoroutine(StartShooting());
        }  

        TrackPlayerPosition();
    }


    void Shoot()
    {
        // local variables for projectile and player to calculate 
        GameObject _projectile = _spawnProjectile.SpawnProjectileObject(_spawnPosition);
        
        Vector3 _projectileShooterPosition = _spawnPosition;
        Vector3 _lastKnownPlayerPosition = _player.transform.position;

        if(_projectile.TryGetComponent<SpawnProjectile>(out SpawnProjectile projectileScript))
       {
            projectileScript.ProjectileGameObject = _projectile;
            projectileScript.ProjectileDirection = Vector2.left;

            _playerLastPosition = Vector3.Normalize(_lastKnownPlayerPosition - _projectileShooterPosition);
            _projectile.transform.rotation = Quaternion.FromToRotation(Vector3.left, _playerLastPosition);
       }
        
    }   


    Quaternion ProjectileClampedRotation(Vector3 playerPosition)
    {   
        Quaternion targetRotation = Quaternion.FromToRotation(_barrelDirection, playerPosition);
        
        Vector3 euler = targetRotation.eulerAngles;
    
        euler.x = Mathf.Clamp(euler.x, 0, 0);
        euler.y = Mathf.Clamp(euler.y, 0, 0);
        
        
        targetRotation = Quaternion.Euler(euler);
        
        return targetRotation;
    }


    IEnumerator StartShooting()
    {
        Shoot(); 
        yield return new WaitForSeconds(_nextShotTimer);
    }


    void TrackPlayerPosition()
    {
        if(_player == null)
        {
            return;
        }

        this.transform.rotation = ProjectileClampedRotation(_playerConstantPosition);
    }
}
