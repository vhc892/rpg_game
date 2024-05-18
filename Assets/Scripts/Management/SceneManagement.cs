using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : Singleton<SceneManagement>
{
    public string SceneTransitionName {  get; private set; }
    private float waitToLoadTime = 1f;

    public void SetTransitionName(string sceneTransitionName)
    {
        this.SceneTransitionName = sceneTransitionName;
    }
    public void Play()
    {
        
        SceneManager.LoadScene(1);
    }
    
    public void Menu()
    {

    }
}
