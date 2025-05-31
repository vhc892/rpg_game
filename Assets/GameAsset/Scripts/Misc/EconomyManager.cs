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
    public void BuyIncreaseMaxHealth()
    {
        if (currentGold >= 8)
        {
            currentGold -= 8;
            PlayerHealth.Instance.IncreaseMaxHealth(5);
            UpdateGold();
        }
        else
        {
            UIManager.Instance.ShowNotEnoughGold();
        }
    }

    public void BuyHeal()
    {
        if (currentGold >= 5)
        {
            currentGold -= 5;
            PlayerHealth.Instance.Heal(5);
            UpdateGold();
        }
        else
        {
            UIManager.Instance.ShowNotEnoughGold();
        }
    }


    public int GetGold()
    {
        return currentGold;
    }

    public void SetGold(int amount)
    {
        currentGold = amount;
        UpdateGold();
    }


}
