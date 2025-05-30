using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [Header("Quest Chain Setup")]
    public QuestChainData questChain;

    private int currentQuestIndex = 0;
    private QuestData activeQuest;
    private int currentCount = 0;
    private List<string> completedQuests = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (SaveLoadManager.Instance != null && SaveLoadManager.Instance.isNewGame)
        {
            StartNextQuest();
        }
    }

    public void RegisterEnemyKilled(Helper.EnemyType killedType)
    {
        if (activeQuest == null || killedType != activeQuest.targetType)
            return;

        currentCount++;
        QuestUI.Instance.UpdateProgress(currentCount, activeQuest.requiredCount);

        if (currentCount >= activeQuest.requiredCount)
        {
            GameEvents.RaiseQuestCompleted(activeQuest);
        }
    }

    public void CollectQuestReward()
    {
        if (activeQuest == null) return;

        if (!completedQuests.Contains(activeQuest.questID))
            completedQuests.Add(activeQuest.questID);

        EconomyManager.Instance.AddGold(activeQuest.goldReward);

        currentQuestIndex++;
        StartNextQuest();
    }

    private void StartNextQuest()
    {
        if (questChain == null || questChain.questSequence.Length == 0 || currentQuestIndex >= questChain.questSequence.Length)
        {
            Debug.Log("✅ All quests completed.");
            activeQuest = null;
            return;
        }

        activeQuest = questChain.questSequence[currentQuestIndex];
        currentCount = 0;

        GameEvents.RaiseQuestStarted(activeQuest);
        QuestUI.Instance.UpdateProgress(currentCount, activeQuest.requiredCount);
    }

    // === Getters for Save ===
    public string GetActiveQuestID()
    {
        return activeQuest != null ? activeQuest.questID : null;
    }

    public int GetCurrentProgress()
    {
        return currentCount;
    }

    public List<string> GetCompletedQuestIDs()
    {
        return completedQuests;
    }

    public int GetCurrentQuestIndex()
    {
        return currentQuestIndex;
    }

    // === Load from Save ===
    public void SetQuestStateFromSave(int index, string questID, int progress, List<string> completedList)
    {
        currentQuestIndex = Mathf.Clamp(index, 0, questChain.questSequence.Length - 1);
        LoadCompletedQuestList(completedList);
        LoadQuestState(questID, progress);
    }

    private void LoadCompletedQuestList(List<string> completedList)
    {
        completedQuests = completedList ?? new List<string>();
    }

    private void LoadQuestState(string questID, int progress)
    {
        foreach (var quest in questChain.questSequence)
        {
            if (quest.questID == questID)
            {
                activeQuest = quest;
                currentCount = progress;
                GameEvents.RaiseQuestStarted(activeQuest);
                QuestUI.Instance.UpdateProgress(currentCount, activeQuest.requiredCount);
                return;
            }
        }

        Debug.LogWarning("❌ Quest ID không khớp trong chuỗi.");
    }

    public bool IsQuestComplete(string questID)
    {
        return completedQuests.Contains(questID);
    }

    public QuestData GetActiveQuest()
    {
        return activeQuest;
    }
}
