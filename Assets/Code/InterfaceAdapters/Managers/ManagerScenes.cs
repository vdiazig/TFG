using UnityEngine;
using UnityEngine.SceneManagement;

using Infraestructure.Services;
using InterfaceAdapters.Interfaces;

namespace InterfaceAdapters.Managers
{
    public class ManagerScenes : MonoBehaviour
    {
        public static ManagerScenes Instance { get; private set; } 
        [SerializeField] private ManagerUser managerUser;
        [SerializeField] private GameObject canvasHUD;
        [SerializeField][Tooltip("Only for control")] private bool activateHUD;
        [SerializeField] public GameObject PlayerPrefab { get; private set; }

        private ISceneManager _sceneManager;
        private bool pendingHUDState = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            _sceneManager = new SceneManagerService(); // Inyección de dependencia

            // Suscribir al evento SceneManager.sceneLoaded
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void OnDestroy()
        {
            // Desuscribir para evitar problemas si el objeto es destruido
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        public void LoadScene(string sceneName, bool needHUD, System.Action onSceneLoaded = null)
        {
            pendingHUDState = needHUD; // Guardar el estado del HUD
            _sceneManager.LoadScene(sceneName, onSceneLoaded);
        }

        public void UnloadScene(string sceneName, bool needHUD, System.Action onSceneUnloaded = null)
        {
            _sceneManager.UnloadScene(sceneName, onSceneUnloaded);
            HUDState(needHUD);
        }

        public void LoadSceneAdditively(string sceneName, bool needHUD, System.Action onSceneLoaded = null)
        {
            pendingHUDState = needHUD; // Guardar el estado del HUD
            _sceneManager.LoadSceneAdditively(sceneName, onSceneLoaded);
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"Scene {scene.name} loaded in {mode} mode.");

            // Llama a HUDState si el HUD es necesario
            if (pendingHUDState)
            {
                HUDState(true);
                pendingHUDState = false; // Resetear el estado pendiente
            }
        }

        // Activa el HUD 
        private void HUDState(bool needHUD)
        {
            activateHUD = needHUD;
            canvasHUD.SetActive(activateHUD);

            if (needHUD)
            {
                managerUser.SetupHUD();
            }
        }
    }
}
