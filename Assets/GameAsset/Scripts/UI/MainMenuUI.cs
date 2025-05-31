using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene(1);
    }

    public void OnContinue()
    {
        if (SaveLoadManager.Instance.SaveExists())
        {
            SaveLoadManager.Instance.isNewGame = false;
            SceneManager.LoadScene(2);
        }
        else
        {
            Debug.Log("No save file to continue.");
        }
    }
}
