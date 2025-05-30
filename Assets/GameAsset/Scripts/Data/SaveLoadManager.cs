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
            completedQuestIDs = QuestManager.Instance.GetCompletedQuestIDs()
        };

        SaveSystem.Save(data);
        Debug.Log("Saved!!!");
    }

    public void LoadGame()
    {
        GameData data = SaveSystem.Load();
        if (data == null)
        {
            Debug.LogWarning("No file save.");
            return;
        }

        PlayerController.Instance.SetPosition(data.playerPosition);
        EconomyManager.Instance.SetGold(data.currentGold);
        LevelManager.Instance.SetCurrentLevelIndex(data.currentLevelIndex);
        QuestManager.Instance.LoadQuestState(data.activeQuestID, data.activeQuestProgress);
        QuestManager.Instance.LoadCompletedQuestList(data.completedQuestIDs);
    }

    public bool SaveExists()
    {
        return SaveSystem.Exists();
    }
}
