using System.Collections.Generic;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Tooltip("Drag in the scene’s default spawn-point (empty Transform) here")]
    [SerializeField] private Transform startPoint;

    [Header("Pause Menu UI")]
    [Tooltip("PauseMenu Canvas")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private AudioClip Tutorial;
    [SerializeField] private AudioClip Dungeon;
    [SerializeField] private AudioClip Mushroom;
    private AudioSource audioSource;
    private bool isPaused = false;
    private string _respawnScene;
    private Vector3 _respawnPos;
    // keep only one lit checkpoint at a time
    private HashSet<string> _litCheckpoints = new HashSet<string>();
    public bool UnlockedDash { get; private set; } = false;
    public bool UnlockedDoubleJump { get; private set; } = false;
    private Dictionary<string, bool> AudiosPlaying = new Dictionary<string, bool>
    {
        { "Tutorial", false},
        {"Dungeon", false },
        {"Mushroom", false }
    };
    public void UnlockDash()
        => UnlockedDash = true;
    public void UnlockDoubleJump()
        => UnlockedDoubleJump = true;

    public void OnResumeButton() => Resume();
    public void OnBackToMenu() => ReturnToMenu();
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _respawnScene = SceneManager.GetActiveScene().name;
            _respawnPos = startPoint != null
                           ? startPoint.position
                           : Vector3.zero;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "Starting_Menu")
            {
                return;
            }
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Starting_Menu");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Name: {scene.name}");
        if (scene.name == _respawnScene)
            StartCoroutine(DoRespawn());
        if (scene.name == "Tutorial1")
        {
            if (audioSource.isPlaying && audioSource.clip == Tutorial)
            {
                //do nothing
            }
            else
            {
                audioSource.clip = Tutorial;
                audioSource.Play();
            }
        }
        else if (scene.name == "VerticleSlice" || scene.name == "Mushroom2")
        {
            if (audioSource.isPlaying && audioSource.clip == Mushroom)
            {
                // do nothing
            }
            else
            {
                audioSource.clip = Mushroom;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying && audioSource.clip == Dungeon)
            {
                //do nothing
            }
            else
            {
                audioSource.clip = Dungeon;
                audioSource.Play();
            }
        }
        
    }

    // Clear out any previously-lit checkpoint, then mark this one.
    public void ActivateCheckpoint(string checkPointId, Vector3 worldPos, string sceneName)
    {
        _litCheckpoints.Clear();
        _litCheckpoints.Add($"{sceneName}:{checkPointId}");
        Debug.Log($"{sceneName}, {checkPointId}");
        _respawnScene = sceneName;
        _respawnPos = worldPos;

        Debug.Log($"Saved: {_respawnScene}, {_respawnPos}");
    }

    public bool IsCheckpointLit(string checkPointId, string sceneName)
    {
        return _litCheckpoints.Contains($"{sceneName}:{checkPointId}");
    }

    public void ReloadToCheckpoint()
    {
        SceneManager.LoadScene(_respawnScene);
    }

    private System.Collections.IEnumerator DoRespawn()
    {
        yield return null;


        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = _respawnPos;
        }
        else Debug.Log("Respawn failed at checkpoint");
    }
}
