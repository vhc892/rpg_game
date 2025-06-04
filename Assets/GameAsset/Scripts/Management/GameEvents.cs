using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<QuestData> OnQuestStarted;
    public static Action<QuestData> OnQuestCompleted;
    public static Action OnEnemyKilledSurvival;
    public static Action OnLevelLoaded;

    public static void RaiseQuestStarted(QuestData quest)
    {
        OnQuestStarted?.Invoke(quest);
    }

    public static void RaiseQuestCompleted(QuestData quest)
    {
        OnQuestCompleted?.Invoke(quest);
    }
    public static void RaiseEnemyKilledSurvival()
    {
        OnEnemyKilledSurvival?.Invoke();
    }
    public static void RaiseLevelLoaded()
    {
        OnLevelLoaded?.Invoke();
    }
}
