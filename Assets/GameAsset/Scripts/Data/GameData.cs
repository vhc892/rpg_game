using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public int currentGold;
    public int currentLevelIndex;

    public string activeQuestID;
    public int activeQuestProgress;

    public List<string> completedQuestIDs = new List<string>();
}
