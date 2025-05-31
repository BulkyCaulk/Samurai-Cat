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
    private float _bossSlerpDuration;
    private int _spawnIndex;
    private float _timer;
    private bool _isSlerpin;
    private bool _isCorountineRunning;
    private GameObject _platformDisabled;
    private IEnumerator _currentAttack;
    private ProjectileShooter1 spitter;
    private float _centerOffset;
    Vector3 centerPivot;
    Vector3 start;
    Vector3 end;

    void Start()
    {
        // generic values
        _currentHealth = this.GetComponent<Enemy>().Enemy_Health;
        _bossLerpDuration = 1f;
        _bossSlerpDuration = 6f;
        _timer = 0;
        _centerOffset = .3f;
        _isSlerpin = false;
        _isCorountineRunning = false;

        // slerp values
        centerPivot = (_bossSlerpPositions[1].position + _bossSlerpPositions[0].position) * .5f ;
        centerPivot -= new Vector3(0, -_centerOffset);
        start = _bossSlerpPositions[1].position - centerPivot;
        end = _bossSlerpPositions[0].position - centerPivot;

        // spit cannon reference
        spitter = _spitCannon.GetComponent<ProjectileShooter1>();
        spitter.ShotTimer = 2f;
        spitter.Projectile.ProjectileSpeed = 20f;
    }


    void Update()
    {
        _timer += Time.deltaTime;
        _spitCannon.transform.position = this.transform.position;

        // current health can't be set on first frame
        if (_currentHealth == 0)
        {
            _currentHealth = this.GetComponent<Enemy>().Enemy_Health;
        }
        _reducedHealth = this.GetComponent<Enemy>().Enemy_Health;


        if (_reducedHealth < _currentHealth)
        {
            _spitCannon.SetActive(false);
            //StopCoroutine(_currentAttack);
            _currentHealth = _reducedHealth;
            // set currentAttack to lerp down coroutine
            _currentAttack = LerpDownward();
            StartCoroutine(_currentAttack);
            _bossLerpDuration -= .15f;
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


        if (this.transform.position.x - spitter.ConstantPlayerPosition.x < 0)
        {
            this.transform.localScale = new Vector3(-3, this.transform.localScale.y, this.transform.localScale.z);
        }
        else
        {
            this.transform.localScale = new Vector3(3, this.transform.localScale.y, this.transform.localScale.z);
        }
    }


    IEnumerator LerpUpward()
    {
        _isCorountineRunning = true;
        spitter.ShotTimer = 2f;
        this.transform.position = GetSpawnPosition();
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
        spitter.ShotTimer = .2f;

        // slerp
        while (timeElapsed < _bossSlerpDuration)
        {
            this.transform.position = (Vector3.Slerp(start, end, timeElapsed / _bossSlerpDuration) * -1) + centerPivot;
            _spitCannon.transform.position = this.transform.position;
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
