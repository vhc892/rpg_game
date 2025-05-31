using System.Collections;
using UnityEngine;

public class AreaExit : MonoBehaviour
{
    [Header("Level Transition")]
    [SerializeField] private int levelIndexToLoad;
    [SerializeField] private string entranceID;

    [Header("Quest Requirement")]
    [SerializeField] private string requiredQuestID;

    private float waitToLoadTime = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<PlayerController>()) return;

        // check quest complete
        if (!string.IsNullOrEmpty(requiredQuestID) &&
    !       QuestManager.Instance.IsQuestComplete(requiredQuestID))
        {
            var questData = QuestManager.Instance.GetQuestByID(requiredQuestID);

            if (questData != null && UIManager.Instance.questBlockerText != null)
            {
                UIManager.Instance.questBlockerText.text = $"{questData.questTitle} to unlock this area";
            }

            UIManager.Instance.questBlockerPanel.SetActive(true);
            return;
        }

        LevelManager.Instance.SetNextEntranceID(entranceID);
        UIFade.Instance.FadeToBlack();
        StartCoroutine(LoadLevelRoutine());
    }

    private IEnumerator LoadLevelRoutine()
    {
        yield return new WaitForSeconds(waitToLoadTime);
        LevelManager.Instance.LoadLevel(levelIndexToLoad);
    }
}
