using UnityEngine;
using UnityEngine.SceneManagement;
using Helper;

public class MainMenuUI : MonoBehaviour
{
    public GameObject continueButton;

    private void Start()
    {
        continueButton.SetActive(SaveLoadManager.Instance.SaveExists());
    }

    public void OnNewGame()
    {
        SaveSystem.DeleteSave();
        SaveLoadManager.Instance.isNewGame = true;
        GameModeManager.Instance.SetGameMode(GameMode.MainGame);
        SceneManager.LoadScene(1);
    }
    public void OnSurvivalMode()
    {
        GameModeManager.Instance.SetGameMode(GameMode.Survival);
        SceneManager.LoadScene(4);
    }
    public void OnContinue()
    {
        if (SaveLoadManager.Instance.SaveExists())
        {
            SaveLoadManager.Instance.isNewGame = false;
            GameModeManager.Instance.SetGameMode(GameMode.MainGame);
            SceneManager.LoadScene(2);
        }
        else
        {
            Debug.Log("No save file to continue.");
        }
    }
}
