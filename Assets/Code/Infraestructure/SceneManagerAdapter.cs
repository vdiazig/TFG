using UnityEngine.SceneManagement;
using System;

public class SceneManagerAdapter : ISceneManager
{
    public void LoadScene(string sceneName, Action onSceneLoaded = null)
    {
        SceneManager.LoadScene(sceneName);
        onSceneLoaded?.Invoke();
    }

    public void UnloadScene(string sceneName, Action onSceneUnloaded = null)
    {
        SceneManager.UnloadSceneAsync(sceneName).completed += _ => onSceneUnloaded?.Invoke();
    }

    public void LoadSceneAdditively(string sceneName, Action onSceneLoaded = null)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += _ => onSceneLoaded?.Invoke();
    }
}
