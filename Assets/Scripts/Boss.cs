using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private List<GameObject> _bossPlatformsSpawner;
    [SerializeField] private GameObject _bossPlatformPattern;
    [SerializeField] private float _timerTillNextPlatform;
    [SerializeField] private float _bossHealth;
    [SerializeField] private GameObject _fireBallSpawner;
    [SerializeField] private float _platformTimeSpan;
    [SerializeField] private SpawnProjectile _spawnProjectile;
    [SerializeField] private float _fireRate;
    [SerializeField] private BoxCollider2D _bossCollider;
    private float _timer;
    private IEnumerator _platformCoroutine;
    private IEnumerator _shootAttackCoroutine;
    private int _randomSpawnLocation;
    private List<GameObject> _platformsSpawned;
    private List<GameObject> _platformsToRemove;
    private Vector3 _playersLastPosition;
    private Vector3 _pushBackArea;

    
    // Start is called before the first frame update
    void Awake()
    {
        _timer = 0f;
        _platformsSpawned = new List<GameObject>();
        _platformsToRemove = new List<GameObject>();
    }
    void Start()
    {
        _pushBackArea = new Vector3(0, 1.5f, 0) + this.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        _fireRate -= Time.deltaTime;

        if(_timer >= _timerTillNextPlatform) {
            _timer = 0;
            _bossCollider.enabled = true;
            _platformCoroutine = SpawnPlatforms();
            StartCoroutine(_platformCoroutine);
        }

        Debug.Log(_bossHealth);

        
        // needs to be implemented still
        if(_bossHealth <= 2 && _fireRate <= 0)
        {
            _fireRate = 1.2f;
            _shootAttackCoroutine = ShootAttack();
            StartCoroutine(_shootAttackCoroutine);
        }
        

        if(_platformsSpawned != null)
        {
            for(int i = 0; i < _platformsSpawned.Count; i++)
            {
                if(_platformsSpawned[i].TryGetComponent<BossPlatforms>(out BossPlatforms _spawnedPlatform))
                {
                        if(_spawnedPlatform.SpawnTimeSpan <= 0)
                        {
                            _platformsToRemove.Add(_platformsSpawned[i]);
                            _platformsSpawned.Remove(_platformsSpawned[i]);
                            Destroy(_platformsToRemove[i]);
                            _platformsToRemove = new List<GameObject>();
                        }
                }
            }
        }
    }

    private void EnableBossPlatforms()
    {
        _randomSpawnLocation = Random.Range(0,2);
        _bossPlatformsSpawner[_randomSpawnLocation].SetActive(true);
        GameObject platform = Instantiate(_bossPlatformPattern, _bossPlatformsSpawner[_randomSpawnLocation].transform.position, Quaternion.identity);
        if(platform.TryGetComponent<BossPlatforms>(out BossPlatforms component))
        {
            component.SpawnTimeSpan = _platformTimeSpan;
        }
        _platformsSpawned.Add(platform);
    }
    private void DisableBossPlatforms()
    {
        _bossPlatformsSpawner[_randomSpawnLocation].SetActive(false);
    }

    private IEnumerator SpawnPlatforms()
    {
        EnableBossPlatforms();
        DisableBossPlatforms();
        yield return new WaitForSeconds(_timerTillNextPlatform);
    }

    private IEnumerator ShootAttack()
    {
        SpitFireBall();
        yield return new WaitForSeconds(2);
    }



    private void SpitFireBall()
    {
       GameObject _projectile =  _spawnProjectile.SpawnProjectileObject();
       GameObject player = GameObject.Find("Player");
       

       // set the direction and player position for the projectile
       if(_projectile.TryGetComponent<SpawnProjectile>(out SpawnProjectile projectileScript))
       {
            _playersLastPosition = Vector3.Normalize(player.transform.position - projectileScript.ProjectileSpawnPosition.transform.position);
            projectileScript.ProjectileDirection = _playersLastPosition;
            projectileScript.ProjectileGameObject = _projectile;
       }
       // increase the projectile speed 
       if(_bossHealth == 1)
        {
            projectileScript.ProjectileSpeed = 10;
            _fireRate = .4f;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        _pushBackArea = new Vector3(0, 1.5f, 0) + this.transform.position;
        Gizmos.DrawWireSphere(_pushBackArea, 3.5f);
    }
    void PushPlayerAway()
    {
        Collider2D objectInVicinity = Physics2D.OverlapCircle(_pushBackArea, 3.5f);
        Debug.Log(objectInVicinity.name);
        if(objectInVicinity.TryGetComponent<Rigidbody2D>(out Rigidbody2D playerRB) && objectInVicinity.name == "Player")
        {
            Vector2 direction = Vector3.Normalize(_pushBackArea - objectInVicinity.transform.position);
            playerRB.velocity = direction * 30;
            Debug.Log($"{playerRB.name} is being pushed?");
        }
    }

    public void TakeDamage()
    {
        //reduce enemy health by x amount of damage
        _bossHealth--;
        PushPlayerAway();
        _bossCollider.enabled = false;
        if(_bossHealth <= 0)
        {
            //play death animation
            //_enemyAnimator.SetBool("isDead", true);
            //destroy game object
            Destroy(gameObject);
        }
        
    }
}
