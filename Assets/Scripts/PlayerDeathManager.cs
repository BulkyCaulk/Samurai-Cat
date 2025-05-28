using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathParticle;
    private ParticleSystem particleInstance;
    public void PlayDeathEffect()
    {
        Vector3 currentPosition = transform.position;
        particleInstance = Instantiate(deathParticle, currentPosition, Quaternion.identity);
    }
}
