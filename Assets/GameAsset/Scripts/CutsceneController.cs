using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public CanvasGroup[] cutsceneImages;
    public float displayTime = 3f;
    public float fadeDuration = 1f;

    private int currentIndex = 0;
    private bool isTransitioning = false;
    private Coroutine currentCoroutine;
    public bool isFinalCutScene;

    void Start()
    {
        foreach (var img in cutsceneImages)
        {
            img.alpha = 0;
            img.gameObject.SetActive(false);
        }

        if (cutsceneImages.Length > 0)
        {
            cutsceneImages[0].gameObject.SetActive(true);
            currentCoroutine = StartCoroutine(ShowImageWithDelay(cutsceneImages[0]));
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTransitioning)
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            ShowNextImage();
        }
    }

    IEnumerator ShowImageWithDelay(CanvasGroup image)
    {
        isTransitioning = true;
        yield return StartCoroutine(FadeIn(image));
        isTransitioning = false;

        yield return new WaitForSeconds(displayTime);

        if (!isTransitioning)
        {
            ShowNextImage();
        }
    }

    void ShowNextImage()
    {
        if (currentIndex < cutsceneImages.Length - 1)
        {
            StartCoroutine(TransitionToNextImage());
        }
        else if(!isFinalCutScene)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            SaveSystem.DeleteSave();
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator TransitionToNextImage()
    {
        isTransitioning = true;

        CanvasGroup current = cutsceneImages[currentIndex];
        CanvasGroup next = cutsceneImages[++currentIndex];

        yield return StartCoroutine(FadeOut(current));
        current.gameObject.SetActive(false);

        next.gameObject.SetActive(true);
        currentCoroutine = StartCoroutine(ShowImageWithDelay(next));

        isTransitioning = false;
    }

    IEnumerator FadeIn(CanvasGroup cg)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            cg.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        cg.alpha = 1f;
    }

    IEnumerator FadeOut(CanvasGroup cg)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            cg.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        cg.alpha = 0f;
    }
}
