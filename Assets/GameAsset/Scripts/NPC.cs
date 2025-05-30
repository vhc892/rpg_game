using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    private bool isPlayerInRange = false;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            // Đăng ký sự kiện Interact khi người chơi vào vùng
            playerControls.Combat.Interact.performed += OnInteractPerformed;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            // Gỡ sự kiện khi người chơi rời vùng
            playerControls.Combat.Interact.performed -= OnInteractPerformed;

            // Tắt panel nếu đang hiện
            if (UIManager.Instance != null)
                UIManager.Instance.upgradePanel.SetActive(false);
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (!isPlayerInRange) return;

        if (UIManager.Instance != null && UIManager.Instance.upgradePanel != null)
        {
            UIManager.Instance.upgradePanel.SetActive(true);
        }
    }
}
