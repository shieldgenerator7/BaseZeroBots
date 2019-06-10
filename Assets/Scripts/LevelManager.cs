using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int startLevelIndex = 0;
    public List<string> levels;

    int currentLevelIndex = 0;
    public int LevelIndex
    {
        get { return currentLevelIndex; }
        set
        {
            if (value != currentLevelIndex)
            {
                value = (int)Mathf.Repeat(value, levels.Count);
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
        //Initialize
        LoadLevel(startLevelIndex);
        SceneManager.sceneLoaded += sceneLoaded;
        SceneManager.sceneUnloaded += sceneUnloaded;
    }

#if UNITY_EDITOR
    public void fillLevelsArray()
    {
        //Fill levels array from build settings
        //2019-05-30: copied from http://answers.unity.com/answers/1540619/view.html
        levels = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled && !scene.path.Contains("PlayScene"))
            {
                //String manipulation
                string sceneName = scene.path;
                string[] split = sceneName.Split('/','.');
                sceneName = split[split.Length - 2];
                //Add the scene name to the list
                levels.Add(sceneName);
            }
        }
    }
#endif

    public void LoadLevel(int levelIndex = 0)
    {
        Scene currentLevel = Level;
        if (currentLevel.isLoaded)
        {
            SceneManager.UnloadSceneAsync(Level);
        }
        SceneManager.LoadScene(levels[levelIndex], LoadSceneMode.Additive);
        currentLevelIndex = levelIndex;
        FindObjectOfType<TurnManager>().resetTime();
        FindObjectOfType<TurnManager>().resetTurnDelay();
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
