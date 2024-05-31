using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int currentGold;
    public Vector3 playerPositon;
    public GameData()
    {
        //this.currentGold = 0;
        playerPositon = Vector3.zero;
    }
}
