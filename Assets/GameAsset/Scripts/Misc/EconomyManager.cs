using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EconomyManager : Singleton<EconomyManager>
{
    public TMP_Text goldText;
    private int currentGold;

    const string COIN_AMOUNT_TEXT = "GoldAmountText";

    private void Start()
    {
        UpdateGold();
    }
   
    //pick up
    public void UpdateCurrentGold()
    {
        currentGold += 1;
        //if(goldText == null)
        //{
        //    goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        //}
        goldText.text = currentGold.ToString("D3");
    }
    public void UpdateGold()
    {
        goldText.text = currentGold.ToString("D3");
    }
    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateGold();
    }
    public void Buy()
    {
        if (currentGold > 2) {
            currentGold = currentGold - 2;
            UpdateGold();
         }
    }

}
