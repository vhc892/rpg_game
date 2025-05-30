using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Quest Chain")]
public class QuestChainData : ScriptableObject
{
    public QuestData[] questSequence;
}
