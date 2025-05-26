using UnityEngine;

public class ElevatorTrigger2D : MonoBehaviour
{
    [Tooltip("Reference to the parent elevator controller")]
    [SerializeField] private ElevatorPlatform elevator;
    [Tooltip("UI prompt")]
    [SerializeField] private GameObject interactionPromptUI;

    private bool _playerInRange = false;

    private void Update()
    {
        // Show prompt automatically once elevator stops, if player is still in range
        if (_playerInRange && !elevator.IsMoving && !interactionPromptUI.activeSelf)
            interactionPromptUI.SetActive(true);

        // On E-press, hide prompt and tell elevator to move
        if (_playerInRange && !elevator.IsMoving && Input.GetKeyDown(KeyCode.E))
        {
            interactionPromptUI.SetActive(false);
            elevator.ToggleElevator();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
            if (!elevator.IsMoving)
                interactionPromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
            interactionPromptUI.SetActive(false);
        }
    }
}
