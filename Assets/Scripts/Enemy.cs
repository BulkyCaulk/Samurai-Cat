using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private Rigidbody2D _enemyRigidbody;
    [SerializeField] private Animator _enemyAnimator;
    private float _health;
    public float Enemy_Health { get { return _health ;} }
    // Start is called before the first frame update
    void Start()
    {
        _health = _maxHealth;   
    }

    public void TakeDamage(float damage)
    {
        //reduce enemy health by x amount of damage
        _health -= damage;
        if(_health <= 0)
        {
            //play death animation
            //_enemyAnimator.SetBool("isDead", true);
            //destroy game object
            Destroy(gameObject);
        }
    }

}
