using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public static QuestUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI questText;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnQuestStarted += ShowQuest;
        GameEvents.OnQuestCompleted += HideQuest;
    }

    private void OnDisable()
    {
        GameEvents.OnQuestStarted -= ShowQuest;
        GameEvents.OnQuestCompleted -= HideQuest;
    }

    private void ShowQuest(QuestData quest)
    {
        questText.text = $"{quest.questTitle}: 0 / {quest.requiredCount}";
    }

    public void UpdateProgress(int current, int total)
    {
        questText.text = $"{QuestManager.Instance.startingQuest.questTitle}: {current} / {total}";
    }

    private void HideQuest(QuestData quest)
    {
        questText.text = $"{quest.questTitle} — Hoàn thành!";
    }
}
