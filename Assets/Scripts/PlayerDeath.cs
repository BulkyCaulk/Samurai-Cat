using System.Collections;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [Tooltip("Delay before respawning at checkpoint")]
    [SerializeField] private AudioClip deathAudioClip;
    private AudioSource deathAudioSource;
    public float waitTime = 0.7f;
    void Start()
    {
        deathAudioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(HandleDeath(other));
    }

    public void HealthIsZero(Collider2D playerCollider, AudioClip audioClip)
    {
        deathAudioClip = audioClip;
        StartCoroutine(HandleDeath(playerCollider));
    }

    private IEnumerator HandleDeath(Collider2D playerCollider)
    {
        // Hide the player
        yield return new WaitForSecondsRealtime(0.04f);
        deathAudioSource.clip = deathAudioClip;
        deathAudioSource.Play();
        playerCollider.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(waitTime);
        GameManager.Instance.ReloadToCheckpoint();
    }
}
