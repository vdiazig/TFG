using UnityEngine;

public class ManagerScenes : MonoBehaviour
{
    public static ManagerScenes Instance { get; private set; }  // Propiedad Singleton

    private ISceneManager _sceneManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        _sceneManager = new SceneManagerAdapter();  // Inyección de dependencia
    }

    public void LoadScene(string sceneName, System.Action onSceneLoaded = null)
    {
        _sceneManager.LoadScene(sceneName, onSceneLoaded);
    }

    public void UnloadScene(string sceneName, System.Action onSceneUnloaded = null)
    {
        _sceneManager.UnloadScene(sceneName, onSceneUnloaded);
    }

    public void LoadSceneAdditively(string sceneName, System.Action onSceneLoaded = null)
    {
        _sceneManager.LoadSceneAdditively(sceneName, onSceneLoaded);
    }
}
