using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject upgradePanel;
    public GameObject questBlockerPanel;
    public GameObject settingPanel;
    public GameObject notEnoughGoldPanel;
    public GameObject infoPanel;
    public Slider bossHealthSlider;

    public TMP_Text questBlockerText;

    private void Start()
    {
#if !UNITY_EDITOR
            Application.targetFrameRate = 60;
#endif
        TurnOffPanel();
    }
    public void TurnOnSettingPanel()
    {
        settingPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void TurnOffSettingPanel()
    {
        settingPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void TurnOnInfoPanel()
    {
        infoPanel.SetActive(true);
    }

    public void TurnOffPanel()
    {
        settingPanel.SetActive(false);
        upgradePanel.SetActive(false);
        notEnoughGoldPanel.SetActive(false);
        questBlockerPanel.SetActive(false);
        bossHealthSlider.gameObject.SetActive(false);
        infoPanel.SetActive(false);
        Debug.Log("turn off panel");
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
    public void HomeAndSaveButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        SaveButton();
    }
    public void HomeButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void RestartSurvivalMode()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(4);
    }
    public void SkipToBossBtn()
    {
        LevelManager.Instance.LoadLevel(9);
        PlayerController.Instance.transform.position = Vector3.zero;
    }
    public void BossDeath()
    {
        StartCoroutine(WaitLoadCutScene());
    }
    private IEnumerator WaitLoadCutScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(3);
    }
}
