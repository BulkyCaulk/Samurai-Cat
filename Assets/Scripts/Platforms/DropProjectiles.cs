using System.Collections;
using UnityEngine;

public class StaggeredSpawner2D : MonoBehaviour
{
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private Transform[] spawnLocations;

    [Header("Timing")]
    [Tooltip("Delay before the very first spike appears")]
    [SerializeField] private float initialDelay    = 1f;
    [Tooltip("Time between each successive spawn location")]
    [SerializeField] private float interSpawnDelay = 0.5f;
    [Tooltip("Time to wait after the last has spawned before repeating the whole sequence")]
    [SerializeField] private float waveRepeatDelay = 2f;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        // initial pause
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            for (int i = 0; i < spawnLocations.Length; i++)
            {
                Transform location = spawnLocations[i];
                if (location != null)
                {
                    Instantiate(spikePrefab, location.position, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning($"SpawnLocations[{i}] is null!", this);
                }

                // wait before the next one
                yield return new WaitForSeconds(interSpawnDelay);
            }

            // wait before starting the whole wave again
            yield return new WaitForSeconds(waveRepeatDelay);
        }
    }
}
