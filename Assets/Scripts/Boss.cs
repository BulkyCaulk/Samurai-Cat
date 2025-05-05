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
    [SerializeField] private GameObject blockedWall1;
    [SerializeField] private GameObject blockedWall2;
    [SerializeField] private float _platformTimeSpan;
    [SerializeField] private SpawnProjectile _spawnProjectile;
    [SerializeField] private float _fireRate;
    [SerializeField] private BoxCollider2D _bossCollider;
    [SerializeField] private PlayerMovement playerMovement;
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
        // timers for spawning platforms and spawning projectiles
        _timer += Time.deltaTime;
        _fireRate -= Time.deltaTime;

        if(_timer >= _timerTillNextPlatform) {
            _timer = 0;
            _bossCollider.enabled = true;
            _platformCoroutine = SpawnPlatforms();
            StartCoroutine(_platformCoroutine);
        }


        
        // Start projectile shooting when boss is damaged
        if(_bossHealth <= 2 && _fireRate <= 0)
        {
            // reset timer and start shooting projectiles every 1.2 seconds
            _fireRate = 1.2f;
            _shootAttackCoroutine = ShootAttack();
            StartCoroutine(_shootAttackCoroutine);
        }
        
        
        // For destroying platforms and removing data from lists
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
                            // old data is released as its no longer useful
                            _platformsToRemove.Clear();
                        }
                }
            }
        }
    }


    private void EnableBossPlatforms()
    {
        // get a random int for choosing from the two spawn locations 
        _randomSpawnLocation = Random.Range(0,2);
        // turn on that spawner gameobject and spawn the platform
        _bossPlatformsSpawner[_randomSpawnLocation].SetActive(true);
        GameObject platform = Instantiate(_bossPlatformPattern, _bossPlatformsSpawner[_randomSpawnLocation].transform.position, Quaternion.identity);
        // 
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
        // spawn projectile object 
       GameObject _projectile =  _spawnProjectile.SpawnProjectileObject(_fireBallSpawner.transform.position);
       // find the player gameobject in unity 
       GameObject player = GameObject.Find("Player");
       

       // set the direction and player position for the projectile
       if(_projectile.TryGetComponent<SpawnProjectile>(out SpawnProjectile projectileScript))
       {
            _playersLastPosition = Vector3.Normalize(player.transform.position - _fireBallSpawner.transform.position);
            _projectile.transform.rotation = Quaternion.FromToRotation(Vector3.left, _playersLastPosition);
            projectileScript.ProjectileDirection = Vector2.left;
            projectileScript.ProjectileGameObject = _projectile;
       }
       // increase the projectile speed when boss is close to death - hard coded
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
        // check what is in the area and hold its collider data
        Collider2D objectInVicinity = Physics2D.OverlapCircle(_pushBackArea, 3.5f);
        // check if the collider has a rigidbody and that the objects name is of Player
        if(objectInVicinity.TryGetComponent<Rigidbody2D>(out Rigidbody2D playerRB) && objectInVicinity.name == "Player")
        {
            // calculate the direction of the player from the _pushBackArea
            Vector2 direction = Vector3.Normalize(objectInVicinity.transform.position - _pushBackArea);
            // throw the player in the opposite direction by scalar multiple - hard coded to 30
            playerRB.velocity = direction * 30;
        }
    }


    public void TakeDamage()
    {
        //reduce boss health
        _bossHealth--;
        // pushs the player away from boss
        PushPlayerAway();
        // disable hitbox when boss is damaged
        _bossCollider.enabled = false;
        if(_bossHealth <= 0)
        {
            Destroy(gameObject);
            GameManager.Instance.UnlockDash();
            playerMovement.RefreshAbilities();
            //blockedWall1.SetActive(false);
            blockedWall2.SetActive(false);
        }   
    }


}
