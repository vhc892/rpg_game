using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material whiteFlashMat;
    [SerializeField] private float restoreDefaultMatTime = 0.2f;

    private Material defaultMat;
    private SpriteRenderer spriteRenderer;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
    }
    public float GetRestoreMatTime()
    {
        return restoreDefaultMatTime;
    }
    public IEnumerator FlashRoutine()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashSequence());
        yield return flashCoroutine;
    }
    private IEnumerator FlashSequence()
    {
        spriteRenderer.material = whiteFlashMat;
        yield return new WaitForSeconds(restoreDefaultMatTime);
        spriteRenderer.material = defaultMat;
    }

    public void ResetFlash()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }
        if (spriteRenderer != null && defaultMat != null)
        {
            spriteRenderer.material = defaultMat;
        }
    }
}
