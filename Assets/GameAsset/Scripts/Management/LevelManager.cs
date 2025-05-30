using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public GameObject[] levelPrefabs;
    private GameObject currentLevelInstance;
    private int currentLevelIndex = 0;

    private string nextEntranceID = "";

    private void Start()
    {
        if (SaveLoadManager.Instance.isNewGame)
        {
            Debug.Log("New game");
        }
        else
        {
            Debug.Log("Continue");
            SaveLoadManager.Instance.LoadGame();
        }
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levelPrefabs.Length)
        {
            Debug.LogWarning("Level index out of range!");
            return;
        }
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }
        currentLevelInstance = Instantiate(levelPrefabs[levelIndex]);
        currentLevelIndex = levelIndex;

        Debug.Log($"Loaded level {levelIndex}");
    }

    public void LoadNextLevel()
    {
        int nextIndex = currentLevelIndex + 1;
        if (nextIndex < levelPrefabs.Length)
        {
            LoadLevel(nextIndex);
        }
        else
        {
            Debug.Log("No more levels.");
        }
    }

    public void LoadPreviousLevel()
    {
        int prevIndex = currentLevelIndex - 1;
        if (prevIndex >= 0)
        {
            LoadLevel(prevIndex);
        }
        else
        {
            Debug.Log("This is the first level.");
        }
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }
    public void SetCurrentLevelIndex(int index)
    {
        currentLevelIndex = index;
    }
    //public GameObject GetCurrentLevelInstance()
    //{
    //    return currentLevelInstance;
    //}

    public void SetNextEntranceID(string id)
    {
        nextEntranceID = id;
    }

    public string GetEntranceID()
    {
        return nextEntranceID;
    }
}
