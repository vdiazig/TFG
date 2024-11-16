public interface ISceneManager
{
    void LoadScene(string sceneName, System.Action onSceneLoaded = null);
    void UnloadScene(string sceneName, System.Action onSceneUnloaded = null);
    void LoadSceneAdditively(string sceneName, System.Action onSceneLoaded = null);
}
