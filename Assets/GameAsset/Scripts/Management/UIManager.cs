using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject upgradePanel;
    public GameObject questBlockerPanel;
    public GameObject settingPanel;
    public GameObject notEnoughGoldPanel;

    private void Start()
    {
        TurnOffPanel();
    }
    public void TurnOnSettingPanel()
    {
        settingPanel.SetActive(true);
    }
    public void TurnOffPanel()
    {
        settingPanel.SetActive(false);
        upgradePanel.SetActive(false);
        notEnoughGoldPanel.SetActive(false);

    }
    public void ShowNotEnoughGold()
    {
        StopAllCoroutines();
        StartCoroutine(NotEnoughGoldRoutine());
    }

    private IEnumerator NotEnoughGoldRoutine()
    {
        notEnoughGoldPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        notEnoughGoldPanel.SetActive(false);
    }

    public void SaveButton()
    {
        SaveLoadManager.Instance.SaveGame();
    }
}
