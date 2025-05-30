using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject dialoguePanel;
    public GameObject questBlockerPanel;
    public GameObject settingPanel;

    private void Start()
    {
        TurnOffSettingPanel();
    }
    public void TurnOnSettingPanel()
    {
        settingPanel.SetActive(true);
    }
    public void TurnOffSettingPanel()
    {
        settingPanel.SetActive(false);
    }
    public void SaveButton()
    {
        SaveLoadManager.Instance.SaveGame();
    }
}
