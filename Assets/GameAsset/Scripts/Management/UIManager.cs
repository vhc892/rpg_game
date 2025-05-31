using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : Singleton<UIManager>
{
    public GameObject upgradePanel;
    public GameObject questBlockerPanel;
    public GameObject settingPanel;
    public GameObject notEnoughGoldPanel;

    public TMP_Text questBlockerText;

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
        questBlockerPanel.SetActive(false);
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
    public void HomeButton()
    {
        SceneManager.LoadScene(0);
        SaveButton();
    }
}
