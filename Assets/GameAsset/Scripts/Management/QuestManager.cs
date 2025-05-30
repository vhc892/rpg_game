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
        StartNextQuest();
    }

    private void StartNextQuest()
    {
        if (questChain == null || questChain.questSequence.Length == 0 || currentQuestIndex >= questChain.questSequence.Length)
        {
            Debug.Log("All quest completed");
            activeQuest = null;
            return;
        }

        activeQuest = questChain.questSequence[currentQuestIndex];
        currentCount = 0;

        GameEvents.RaiseQuestStarted(activeQuest);
    }
    public void RegisterEnemyKilled(Helper.EnemyType killedType)
    {
        if (activeQuest == null || killedType != activeQuest.targetType) return;

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

        //complete quest
        if (!completedQuests.Contains(activeQuest.questID))
            completedQuests.Add(activeQuest.questID);

        EconomyManager.Instance.AddGold(activeQuest.goldReward);

        currentQuestIndex++;
        StartNextQuest();
    }

    public bool IsQuestComplete(string questID)
    {
        return completedQuests.Contains(questID);
    }
    public QuestData GetActiveQuest() => activeQuest;
}
