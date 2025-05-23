using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    [SerializeField] private List<Transform> _bossSpawnPositions;
    [SerializeField] private List<Transform> _bossSlerpPositions;
    [SerializeField] private List<Transform> _bossSpitPositions;
    [SerializeField] private GameObject _spitCannon;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private BoxCollider2D _platformDisabler;
    private float _currentHealth;
    private float _reducedHealth;
    private float _bossLerpDuration;
    private int _spawnIndex;
    private float _timer;
    private bool _isSlerpin;
    private bool _isCorountineRunning;
    private GameObject _platformDisabled;
    private IEnumerator _currentAttack;


    void Start()
    {
        _currentHealth = this.GetComponent<Enemy>().Enemy_Health;
        //_reducedHealth = this.GetComponent<Enemy>().Enemy_Health;
        _bossLerpDuration = 1f;
        _timer = 0;
        _isSlerpin = false;
        _isCorountineRunning = false;
    }


    void Update()
    {
        _timer += Time.deltaTime;

        // current health can't be set on first frame
        if (_currentHealth == 0)
        {
            _currentHealth = this.GetComponent<Enemy>().Enemy_Health;
        }
        _reducedHealth = this.GetComponent<Enemy>().Enemy_Health;


        if (_reducedHealth < _currentHealth)
        {
            _spitCannon.SetActive(false);
            StopCoroutine(_currentAttack);
            _currentHealth = _reducedHealth;
            // set currentAttack to lerp down coroutine
            _currentAttack = LerpDownward();
            StartCoroutine(_currentAttack);
            _bossLerpDuration += .25f;
        }
        else
        {
            // set currentAttack to lerp coroutine
            _currentAttack = LerpUpward();
        }


        if (_timer > 10f && !_isSlerpin && !_isCorountineRunning)
        {
            StartCoroutine(_currentAttack);
        }
    }


    IEnumerator LerpUpward()
    {
        _isCorountineRunning = true;
        this.transform.position = GetSpawnPosition();
        Debug.Log($"Spawning at position: {_spawnIndex}");
        StartCoroutine(SignalPlayer());

        yield return new WaitUntil(() => particle.isStopped);

        float timeElapsed = 0;
        while (timeElapsed < _bossLerpDuration)
        {
            this.transform.position = Vector3.Lerp(_bossSpawnPositions[_spawnIndex].position, _bossSpitPositions[_spawnIndex].position, timeElapsed / _bossLerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        this.transform.position = _bossSpitPositions[_spawnIndex].position;

        _spitCannon.SetActive(true);
    }


    IEnumerator LerpDownward()
    {
        _isSlerpin = true;
        _spitCannon.SetActive(false);
        float timeElapsed = 0;
        _timer = 0;

        while (timeElapsed < _bossLerpDuration)
        {
            this.transform.position = Vector3.Lerp(_bossSpitPositions[_spawnIndex].position, _bossSpawnPositions[_spawnIndex].position, timeElapsed / _bossLerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    
        this.transform.position = _bossSpawnPositions[_spawnIndex].position;

        yield return new WaitUntil(() => this.transform.position == _bossSpawnPositions[_spawnIndex].position);
        yield return new WaitForSeconds(.5f);

        _platformDisabled.SetActive(true);

        _currentAttack = BossSlerpin();
        StartCoroutine(_currentAttack);
    }


    IEnumerator BossSlerpin()
    {
        _spitCannon.SetActive(true);
        float timeElapsed = 0;

        // slerp
        while (timeElapsed < _bossLerpDuration)
        {
            this.transform.position = Vector3.Slerp(_bossSlerpPositions[1].position, _bossSlerpPositions[0].position, timeElapsed / _bossLerpDuration) * -1;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        this.transform.position = _bossSlerpPositions[1].position;

        yield return new WaitUntil(() => this.transform.position == _bossSlerpPositions[1].position);

        // reset encounter
        _spitCannon.SetActive(false);
        _isSlerpin = false;
        _timer = 0;
        _isCorountineRunning = false;
    }

    IEnumerator SignalPlayer()
    {
        // set particle position to where the boss 
        //particle.transform.position = _bossSpawnPositions[_spawnIndex].position;
        particle.Play();
        yield return new WaitUntil(() => particle.isStopped);

        //stop playing particle
        particle.Stop();

    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlatformDisableable")
        {
            _platformDisabled = collision.gameObject;
            _platformDisabled.SetActive(false);
        }
    }


    private Vector3 GetSpawnPosition()
    {
        _spawnIndex = Random.Range(0, 3);
        return _bossSpawnPositions[_spawnIndex].position;
    }
}
