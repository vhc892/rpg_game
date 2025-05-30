using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class QuestUI : MonoBehaviour
{
    public static QuestUI Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private RectTransform goldUI;          
    [SerializeField] private RectTransform goldTargetUI;

    private QuestData currentQuest;
    private bool isQuestComplete = false;
    private bool rewardCollected = false;
    private Vector3 goldOriginalPos;

    private void Awake()
    {
        Instance = this;
        goldUI.gameObject.SetActive(false);
        goldOriginalPos = goldUI.anchoredPosition;
    }

    private void OnEnable()
    {
        GameEvents.OnQuestStarted += ShowQuest;
        GameEvents.OnQuestCompleted += OnQuestCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnQuestStarted -= ShowQuest;
        GameEvents.OnQuestCompleted -= OnQuestCompleted;
    }

    private void ShowQuest(QuestData quest)
    {
        currentQuest = quest;
        isQuestComplete = false;
        rewardCollected = false;

        questText.text = $"{quest.questTitle}: 0 / {quest.requiredCount}";
        questText.color = Color.white;

        Button button = questText.GetComponent<Button>();
        if (button == null)
        {
            button = questText.gameObject.AddComponent<Button>();
        }

        button.onClick.RemoveAllListeners();
        button.interactable = false;
    }


    public void UpdateProgress(int current, int total)
    {
        if (currentQuest != null && !isQuestComplete)
        {
            questText.text = $"{currentQuest.questTitle}: {current} / {total}";
        }
    }


    private void OnQuestCompleted(QuestData quest)
    {
        if (quest == currentQuest)
        {
            isQuestComplete = true;
            questText.text = $"{quest.questTitle} — Hoàn thành!";
            questText.color = Color.yellow;

            Button button = questText.GetComponent<Button>();
            if (button != null)
            {
                button.interactable = true;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => CollectReward(quest));
            }
        }
    }


    private void CollectReward(QuestData quest)
    {
        if (rewardCollected) return;
        rewardCollected = true;

        goldUI.gameObject.SetActive(true);
        goldUI.anchoredPosition = goldOriginalPos;

        Vector3 jumpUpPos = goldOriginalPos + new Vector3(0, 50f, 0);

        Sequence seq = DOTween.Sequence();
        seq.Append(goldUI.DOAnchorPos(jumpUpPos, 0.3f).SetEase(Ease.OutQuad));
        seq.AppendInterval(0.2f);
        seq.Append(goldUI.DOMove(goldTargetUI.position, 0.5f).SetEase(Ease.InQuad));
        seq.OnComplete(() =>
        {
            goldUI.gameObject.SetActive(false);
            QuestManager.Instance.CollectQuestReward();
        });
    }

}
