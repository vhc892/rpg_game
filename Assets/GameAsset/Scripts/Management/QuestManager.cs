using UnityEngine;
using Helper;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [Header("Config")]
    public QuestData startingQuest;

    private QuestData activeQuest;
    private int currentCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartQuest(startingQuest);
    }

    public void StartQuest(QuestData quest)
    {
        activeQuest = quest;
        currentCount = 0;

        GameEvents.RaiseQuestStarted(quest);
    }

    public void RegisterEnemyKilled(EnemyType killedType)
    {
        if (activeQuest == null || killedType != activeQuest.targetType) return;

        currentCount++;
        GameEvents.OnQuestStarted?.Invoke(activeQuest);

        QuestUI.Instance.UpdateProgress(currentCount, activeQuest.requiredCount);

        if (currentCount >= activeQuest.requiredCount)
        {
            GameEvents.RaiseQuestCompleted(activeQuest);
        }
    }
    public bool IsQuestComplete(string questID)
    {
        return activeQuest != null && activeQuest.questID == questID && currentCount >= activeQuest.requiredCount;
    }

}
