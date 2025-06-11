using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    private Shooter shooter;
    private EnemyHealth enemyHealth;
    private Slider bossHealthSlider;

    [Header("%hp to enter phase 2")]
    [SerializeField] private float phase2Threshold = 0.5f;

    private bool isPhase2 = false;
    private bool isInitialized = false;

    private void OnEnable()
    {
        StartCoroutine(InitAfterDelay());
    }
    private void OnDestroy()
    {
        if (bossHealthSlider != null)
        {
            bossHealthSlider.gameObject.SetActive(false);
        }
    }


    private IEnumerator InitAfterDelay()
    {
        yield return null;

        if (shooter == null) shooter = GetComponent<Shooter>();
        if (enemyHealth == null) enemyHealth = GetComponent<EnemyHealth>();

        if (UIManager.Instance != null && UIManager.Instance.bossHealthSlider != null)
        {
            bossHealthSlider = UIManager.Instance.bossHealthSlider;
            bossHealthSlider.maxValue = enemyHealth.GetMaxHealth();
            bossHealthSlider.value = enemyHealth.GetCurrentHealth();
            bossHealthSlider.gameObject.SetActive(true);
        }
        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized) return;
        if (bossHealthSlider != null)
        {
            bossHealthSlider.value = enemyHealth.GetCurrentHealth();
        }

        if (!isPhase2 && enemyHealth.GetMaxHealth() > 0 && enemyHealth.GetCurrentHealth() <= enemyHealth.GetMaxHealth() * phase2Threshold)
        {
            EnterPhase2();
        }

        if (enemyHealth.GetCurrentHealth() <= 0)
        {
            OnBossDeath();
        }
    }

    private void EnterPhase2()
    {
        isPhase2 = true;

        shooter.SetStagger(true);
        shooter.SetOscillate(true);
        shooter.SetBurst(3, 5, 60f, 8);

        Debug.Log("Boss đã vào Phase 2!");
    }
    private void OnBossDeath()
    {
        if (bossHealthSlider != null)
        {
            bossHealthSlider.gameObject.SetActive(false);
        }
        enabled = false;
        StartCoroutine(WaitLoadCutScene());
    }
    private IEnumerator WaitLoadCutScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(3);
    }
}
