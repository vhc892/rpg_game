using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening;

public class SurvivalWaveManager : Singleton<SurvivalWaveManager>
{
    [System.Serializable]
    public class Wave
    {
        [LabelText("Enemy Prefab")] public GameObject enemyPrefab;
        [LabelText("Enemy Count")] public int enemyCount;
        [LabelText("Delay Between Each Spawn (sec)"), MinValue(0f)]
        public float spawnInterval = 0.2f;
    }

    [Title("Wave Settings")]
    [SerializeField] private List<Wave> waves = new List<Wave>();

    [Title("Global Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Title("Wave Timing")]
    [SerializeField] private float delayBetweenWaves = 2f;

    [Title("UI")]
    [SerializeField] private TextMeshProUGUI waveCountdownText;
    [SerializeField] private TextMeshProUGUI waveCompleteText;
    [SerializeField] private RectTransform goldUI;
    [SerializeField] private RectTransform[] coinRewardUI;
    [SerializeField] private float coinFlyDuration = 0.75f;
    [SerializeField] private float waveCompleteTextDuration = 1.5f;
    [SerializeField] private TextMeshProUGUI highestWaveText;
    public GameObject playerDeathUI;

    private int currentWaveIndex = 0;
    private int enemiesKilled = 0;
    private int totalEnemiesThisWave = 0;
    private bool waitingForContinue = false;

    private void OnEnable()
    {
        GameEvents.OnEnemyKilledSurvival += OnEnemyKilled;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilledSurvival -= OnEnemyKilled;
    }

    private void Start()
    {
        UpdateHighestWaveUI();
        StartCoroutine(StartNextWave());
    }

    private void OnEnemyKilled()
    {
        enemiesKilled++;
        if (enemiesKilled >= totalEnemiesThisWave && !waitingForContinue)
        {
            waitingForContinue = true;
            ShowWaveCompleteUI();
        }
    }
    private void ShowWaveCompleteUI()
    {
        if (waveCompleteText != null)
        {
            waveCompleteText.gameObject.SetActive(true);
            waveCompleteText.DOFade(1f, 0.25f).From(0f);
            waveCompleteText.transform.DOScale(Vector3.one, 0.3f).From(Vector3.zero).SetEase(Ease.OutBack);
        }

        StartCoroutine(PlayCoinRewardEffect());
    }
    private IEnumerator PlayCoinRewardEffect()
    {
        List<Vector2> originalAnchors = new List<Vector2>();

        for (int i = 0; i < coinRewardUI.Length; i++)
        {
            RectTransform coin = coinRewardUI[i];
            if (coin == null) continue;

            Vector2 originalPos = coin.anchoredPosition;
            coin.gameObject.SetActive(true);
            coin.localScale = Vector3.one;

            coin.DOMove(goldUI.position, coinFlyDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    coin.gameObject.SetActive(false);
                    coin.anchoredPosition = originalPos;
                    coin.localScale = Vector3.one;
                    EconomyManager.Instance.AddGold(1);
                });

            coin.DOScale(0.5f, coinFlyDuration).SetEase(Ease.InSine);

            yield return new WaitForSeconds(0.25f);//delay between coin
        }

        yield return new WaitForSeconds(coinFlyDuration + 0.2f);

        // hide waveCompleteText
        if (waveCompleteText != null)
        {
            waveCompleteText.DOFade(0f, 0.3f);
            waveCompleteText.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                waveCompleteText.gameObject.SetActive(false);
            });
        }
        UIManager.Instance.upgradePanel.gameObject.SetActive(true);
    }

    public void OnContinueButtonPressed()
    {
        UIManager.Instance.upgradePanel.SetActive(false);
        waitingForContinue = false;
        StartCoroutine(StartNextWave());
    }
    private IEnumerator StartNextWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("All waves completed!");
            yield break;
        }
        //hightest wave
        int savedHighest = PlayerPrefs.GetInt("HighestWave", 0);
        if (currentWaveIndex + 1 > savedHighest)
        {
            PlayerPrefs.SetInt("HighestWave", currentWaveIndex + 1);
            PlayerPrefs.Save();
            UpdateHighestWaveUI();
        }
        // Countdown UI
        if (waveCountdownText != null)
        {
            waveCountdownText.gameObject.SetActive(true);

            for (int i = 5; i > 0; i--)
            {
                waveCountdownText.text = $"Wave {currentWaveIndex + 1} starting in {i}...";
                yield return new WaitForSeconds(1f);
            }

            waveCountdownText.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(delayBetweenWaves);

        enemiesKilled = 0;
        Wave wave = waves[currentWaveIndex];
        totalEnemiesThisWave = wave.enemyCount;

        for (int i = 0; i < wave.enemyCount; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Instantiate(wave.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        Debug.Log($"Wave {currentWaveIndex + 1} started.");
        currentWaveIndex++;
    }
    private void UpdateHighestWaveUI()
    {
        if (highestWaveText != null)
        {
            int highest = PlayerPrefs.GetInt("HighestWave", 0);
            highestWaveText.text = $"Highest Wave: {highest}";
        }
    }
}
