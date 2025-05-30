using UnityEngine;

public class TimedPanel : MonoBehaviour
{
    [SerializeField] private float displayTime = 2f;

    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(Hide), displayTime);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
