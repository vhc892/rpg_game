using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class SurvivalWaveManager : MonoBehaviour
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
        StartCoroutine(StartNextWave());
    }

    private void OnEnemyKilled()
    {
        enemiesKilled++;
        if (enemiesKilled >= totalEnemiesThisWave && !waitingForContinue)
        {
            waitingForContinue = true;
            UIManager.Instance.upgradePanel.SetActive(true);
        }
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
}
