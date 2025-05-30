using UnityEngine;
using Helper;
[CreateAssetMenu(menuName = "Quests/Kill Quest")]
public class QuestData : ScriptableObject
{
    public string questID;
    public string questTitle;
    public EnemyType targetType;
    public int requiredCount;
    public int goldReward;
}
