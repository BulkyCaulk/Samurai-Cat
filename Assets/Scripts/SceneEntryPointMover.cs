using UnityEngine;

public class SceneEntryPointMover : MonoBehaviour
{
    void Start()
    {
        var info = FindObjectOfType<TransitionInfo>();
        if (info == null) return;

        // teleport player to the named entry-point
        var entry  = GameObject.Find(info.entryPointName);
        var player = GameObject.FindGameObjectWithTag("Player");
        if (entry != null && player != null)
            player.transform.position = entry.transform.position;
        else
            Debug.LogWarning($"Couldn’t find '{info.entryPointName}' or Player in scene.");

        Destroy(info.gameObject);
    }
}
