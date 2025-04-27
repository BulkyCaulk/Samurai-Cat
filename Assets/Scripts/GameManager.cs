using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Tooltip("Drag in the scene’s default spawn-point (empty Transform) here")]
    [SerializeField] private Transform startPoint;

    private string _respawnScene;
    private Vector3 _respawnPos;

    // keep only one lit checkpoint at a time
    private HashSet<string> _litCheckpoints = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _respawnScene = SceneManager.GetActiveScene().name;
            _respawnPos   = startPoint != null
                           ? startPoint.position
                           : Vector3.zero;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == _respawnScene)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                player.transform.position = _respawnPos;
        }
    }

    // Clear out any previously-lit checkpoint, then mark this one.
    public void ActivateCheckpoint(string checkPointId, Vector3 worldPos, string sceneName)
    {
        _litCheckpoints.Clear();
        _litCheckpoints.Add($"{sceneName}:{checkPointId}");

        _respawnScene = sceneName;
        _respawnPos   = worldPos;
    }

    public bool IsCheckpointLit(string checkPointId, string sceneName)
    {
        return _litCheckpoints.Contains($"{sceneName}:{checkPointId}");
    }

    public void ReloadToCheckpoint()
    {
        SceneManager.LoadScene(_respawnScene);
    }
}
