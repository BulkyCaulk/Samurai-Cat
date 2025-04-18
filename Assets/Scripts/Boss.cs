using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private List<GameObject> _bossPlatformsSpawner;
    [SerializeField] private GameObject _bossPlatformPattern;
    [SerializeField] private float _timerTillNextPlatform;
    [SerializeField] private Enemy _bossHealth;
    [SerializeField] private GameObject _fireBallSpawner;
    [SerializeField] private float _platformTimeSpan;
    private float _timer;
    private IEnumerator _platformCoroutine;
    private IEnumerator _shootAttackCoroutine;
    private int _randomSpawnLocation;
    private List<GameObject> _platformsSpawned;
    private List<GameObject> _platformsToRemove;
    
    // Start is called before the first frame update
    void Awake()
    {
        _timer = 0f;
        _platformsSpawned = new List<GameObject>();
        _platformsToRemove = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if(_timer >= _timerTillNextPlatform) {
            _timer = 0;
            _platformCoroutine = SpawnPlatforms();
            StartCoroutine(_platformCoroutine);
        }

        Debug.Log(_bossHealth.Enemy_Health);

        /*
        // needs to be implemented still
        if(_bossHealth.Enemy_Health <= 2)
        {
            _shootAttackCoroutine = ShootAttack();
            StartCoroutine(_shootAttackCoroutine);
        }
        */

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
        yield return new WaitForSeconds(0);
    }
}
