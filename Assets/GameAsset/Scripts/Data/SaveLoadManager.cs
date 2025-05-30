using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    public bool isNewGame = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //private void OnApplicationQuit()
    //{
    //    SaveGame();
    //}

    public void SaveGame()
    {
        GameData data = new GameData
        {
            playerPosition = PlayerController.Instance.GetPosition(),
            currentGold = EconomyManager.Instance.GetGold(),
            currentLevelIndex = LevelManager.Instance.GetCurrentLevelIndex(),
            activeQuestID = QuestManager.Instance.GetActiveQuestID(),
            activeQuestProgress = QuestManager.Instance.GetCurrentProgress(),
            completedQuestIDs = QuestManager.Instance.GetCompletedQuestIDs(),
            playerHealth = PlayerHealth.Instance.GetCurrentHealth(),
            currentQuestIndex = QuestManager.Instance.GetCurrentQuestIndex(),
            maxHealth = PlayerHealth.Instance.GetMaxHealth()

        };

        SaveSystem.Save(data);
        Debug.Log("Saved!!!");
    }

    public void LoadGame()
    {
        GameData data = SaveSystem.Load();
        if (data == null)
        {
            Debug.LogWarning("❌ No saved data found.");
            return;
        }

        PlayerController.Instance.SetPosition(data.playerPosition);
        EconomyManager.Instance.SetGold(data.currentGold);
        LevelManager.Instance.SetCurrentLevelIndex(data.currentLevelIndex);
        PlayerHealth.Instance.SetMaxHealth(data.maxHealth);
        PlayerHealth.Instance.SetHealth(data.playerHealth);

        QuestManager.Instance.SetQuestStateFromSave(
            data.currentQuestIndex,
            data.activeQuestID,
            data.activeQuestProgress,
            data.completedQuestIDs
        );
    }

    public bool SaveExists()
    {
        return SaveSystem.Exists();
    }
}
