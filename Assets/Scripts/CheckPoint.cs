using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    [Tooltip("Unique ID for this checkpoint")]
    [SerializeField] private string checkPointId;
    [SerializeField] private GameObject interactPrompt;

    private bool _inRange;
    private bool _activated;

    void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
        RefreshVisuals();
    }

    void Update()
    {
        if (_inRange && Input.GetKeyDown(KeyCode.E))
        {
            // mark this as the only lit checkpoint
            GameManager.Instance.ActivateCheckpoint(
                checkPointId,
                transform.position,
                SceneManager.GetActiveScene().name
            );

            // force all checkpoints to refresh their colors right now
            foreach (var checkpt in FindObjectsOfType<CheckPoint>())
                checkpt.RefreshVisuals();

            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = true;
            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = false;
            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }

    /// update the _activated flag and color accordingly.
    public void RefreshVisuals()
    {
        string scene = SceneManager.GetActiveScene().name;
        _activated = GameManager.Instance.IsCheckpointLit(checkPointId, scene);
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        var sr = GetComponent<SpriteRenderer>();
        sr.color = _activated ? Color.white : Color.gray;
    }
}
