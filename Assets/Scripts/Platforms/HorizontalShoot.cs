using System.Collections;
using UnityEngine;

public class HorizontalShoot : MonoBehaviour
{
    [Header("Projectile Prefab (must have DestroyShoot on it)")]
    [SerializeField] private GameObject projectilePrefab;

    [Header("Spawn configuration")]
    [Tooltip("Positions where each shot will appear")]
    [SerializeField] private Transform[] spawnPoints;
    [Tooltip("Time (in seconds) after wave start when each point fires — must be in ascending order")]
    [SerializeField] private float[] spawnDelays;
    [Tooltip("Horizontal speed for each spawned projectile")]
    [SerializeField] private float[] spawnSpeeds;

    [Header("Wave timing")]
    [Tooltip("Seconds to wait after the last point fires before starting the next wave")]
    [SerializeField] private float waveRepeatDelay = 2f;

    void Start()
    {
        if (spawnPoints.Length != spawnDelays.Length || spawnPoints.Length != spawnSpeeds.Length)
        {
            Debug.LogError("HorizontalShoot: spawnPoints, spawnDelays, and spawnSpeeds lengths must match!", this);
            enabled = false;
            return;
        }

        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (true)
        {
            float prevDelay = 0f;

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                // wait the difference between this point's time and the previous
                float delta = Mathf.Max(0f, spawnDelays[i] - prevDelay);
                yield return new WaitForSeconds(delta);

                // spawn
                if (spawnPoints[i] != null && projectilePrefab != null)
                {
                    GameObject proj = Instantiate(projectilePrefab, spawnPoints[i].position, Quaternion.identity);
                    var shooter = proj.GetComponent<DestroyShoot>();
                    if (shooter != null)
                        shooter.Init(spawnSpeeds[i]);
                }

                prevDelay = spawnDelays[i];
            }

            // wait before the next wave
            yield return new WaitForSeconds(waveRepeatDelay);
        }
    }
}
