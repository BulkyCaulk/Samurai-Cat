using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackTime;
    public float hitDirectionForce;
    public float constForce;
    public float inputForce;
    public AnimationCurve knockbackForceCurve;
    public AnimationCurve constantForceCurve;

    private Rigidbody2D _playerRb;

    private Coroutine _knockbackCoroutine;

    public bool IsBeingKnockedBack { get ; private set ; }

    void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
    }

    public IEnumerator KnockbackAction(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        IsBeingKnockedBack = true;

        Vector2 _hitForce ;
        Vector2 _constantForce;
        Vector2 _knockbackForce;
        Vector2 _combinedForce;
        float _time = 0f;


        float _elapsedTime = 0f;
        while(_elapsedTime < knockbackTime)
        {
            _elapsedTime += Time.fixedDeltaTime;
            _time += Time.fixedDeltaTime;

            _constantForce = constantForceDirection * constForce * constantForceCurve.Evaluate(_time);
            _hitForce = hitDirection * hitDirectionForce * knockbackForceCurve.Evaluate(_time);
            _knockbackForce = _hitForce + _constantForce;

            if(inputDirection != 0 )
            {
                _combinedForce = _knockbackForce + new Vector2(inputDirection * inputForce , 0);
            }
            else 
            {
                _combinedForce = _knockbackForce;
            }

            _playerRb.velocity = _combinedForce;

            yield return new WaitForFixedUpdate();
        }

        IsBeingKnockedBack = false;
    }


    public void CallKnockBackAction(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        _knockbackCoroutine = StartCoroutine(KnockbackAction(hitDirection, constantForceDirection, inputDirection));
    }
}
