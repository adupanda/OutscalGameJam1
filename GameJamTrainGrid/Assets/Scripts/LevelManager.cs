using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance {  get { return instance; } }

    public string[] Levels;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach(var level in Levels)
        {
            SetLevelStatus(level, LevelStatus.Locked);
        }
        if (GetLevelStatus(Levels[0]) == LevelStatus.Locked)
        {
            SetLevelStatus(Levels[0], LevelStatus.Unlocked);
        }
        if(GetLevelStatus("Lobby") == LevelStatus.Locked)
        {
            SetLevelStatus("Lobby", LevelStatus.Unlocked);
        }
        if (GetLevelStatus("HowToPlay") == LevelStatus.Locked)
        {
            SetLevelStatus("HowToPlay", LevelStatus.Unlocked);
        }
    }

    public void MarkLevelComplete()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SetLevelStatus(currentScene.name, LevelStatus.Completed);

        int currentSceneIndex = Array.FindIndex(Levels, level => level == currentScene.name);
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex<Levels.Length)
        {
            SetLevelStatus(Levels[nextSceneIndex],LevelStatus.Unlocked);
        }
    }
    public LevelStatus GetLevelStatus(string levelName)
    {
        LevelStatus levelStatus = (LevelStatus)PlayerPrefs.GetInt(levelName,0);
        return levelStatus;
    }

    public void SetLevelStatus(string level,LevelStatus levelStatus)
    {
        PlayerPrefs.SetInt(level,(int)levelStatus);
    }
}
