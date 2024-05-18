using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EconomyManager : Singleton<EconomyManager>, IDataPersistence
{
    private TMP_Text goldText;
    private int currentGold;

    const string COIN_AMOUNT_TEXT = "GoldAmountText";

    public void LoadData(GameData data)
    {
        this.currentGold = data.currentGold;
    }
    public void SaveData(ref GameData data )
    {
        data.currentGold = this.currentGold;
    }
    public void UpdateCurrentGold()
    {
        currentGold += 1;
        if(goldText == null)
        {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }
        goldText.text = currentGold.ToString("D3");
    }
}
