using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : Singleton<SceneManagement>
{
    public string SceneTransitionName {  get; private set; }

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
      
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
