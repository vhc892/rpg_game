using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : Singleton<SceneManagement>
{
    public string SceneTransitionName {  get; private set; }
    private float waitToLoadTime = 1f;
    //private DataPersistenceManager data;

    public void SetTransitionName(string sceneTransitionName)
    {
        this.SceneTransitionName = sceneTransitionName;
    }
    public void Continue()
    {
        SceneManager.LoadScene(1);
    }
    public void Newgame()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.NewGame();
            //SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogError("DataPersistenceManager instance is null.");
        }
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
