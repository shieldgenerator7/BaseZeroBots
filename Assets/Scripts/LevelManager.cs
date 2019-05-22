using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<string> levels;

    int currentLevelIndex = 0;
    public int LevelIndex
    {
        get { return currentLevelIndex; }
        set
        {
            if (value >= 0 && value != currentLevelIndex)
            {
                LoadLevel(value);
                currentLevelIndex = value;
            }
        }
    }
    public Scene Level
    {
        get { return SceneManager.GetSceneByName(levels[LevelIndex]); }
        set
        {
            int newLevelIndex = levels.IndexOf(value.name);
            if (newLevelIndex >= 0)
            {
                LevelIndex = newLevelIndex;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadLevel(0);
        SceneManager.sceneLoaded += sceneLoaded;
        SceneManager.sceneUnloaded += sceneUnloaded;
    }

    public void LoadLevel(int levelIndex = 0)
    {
        Scene currentLevel = Level;
        if (currentLevel.isLoaded)
        {
            SceneManager.UnloadSceneAsync(Level);
        }
        SceneManager.LoadScene(levels[levelIndex], LoadSceneMode.Additive);
        currentLevelIndex = levelIndex;
    }

    private void sceneLoaded(Scene s, LoadSceneMode lsm)
    {
        if (levels.Contains(s.name))
        {
            onSceneLoaded?.Invoke();
        }
    }
    private void sceneUnloaded(Scene s)
    {
        if (levels.Contains(s.name))
        {
            onSceneUnloaded?.Invoke();
        }
    }

    public delegate void OnSceneLoadStateChanged();
    public OnSceneLoadStateChanged onSceneLoaded;
    public OnSceneLoadStateChanged onSceneUnloaded;
}
