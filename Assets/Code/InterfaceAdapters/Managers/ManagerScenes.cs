using UnityEngine;
using UnityEngine.SceneManagement;

using Infraestructure.Services;
using InterfaceAdapters.Interfaces;
using InterfaceAdapters.Presentation.HUD;
using Unity.VisualScripting;

namespace InterfaceAdapters.Managers
{
    public class ManagerScenes : MonoBehaviour
    {
        public static ManagerScenes Instance { get; private set; } 
        [SerializeField] private ManagerUser managerUser;
        //[SerializeField] public GameObject PlayerPrefab { get; private set; }

        [Header("HUD")]
        [SerializeField][Tooltip("Only for control")] private bool activateHUD;
        [SerializeField] private GameObject canvasHUD;
        [SerializeField] private GameObject gamePause;
        [SerializeField] private GameObject prefabHUD;
        [SerializeField] private GameObject contentHUD;

        private ISceneManager _sceneManager;
        private INotification _notification;

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
        void Start(){
            _notification = NotificationManager.Instance;
        }

        private void OnDestroy()
        {
            // Desuscribir para evitar problemas si el objeto es destruido
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        public void LoadScene(string sceneName, bool needHUD, System.Action onSceneLoaded = null)
        {
            activateHUD = needHUD;
            _sceneManager.LoadScene(sceneName, onSceneLoaded);
        }

        public void UnloadScene(string sceneName, bool needHUD, System.Action onSceneUnloaded = null)
        {
            _sceneManager.UnloadScene(sceneName, onSceneUnloaded);
        }

        public void LoadSceneAdditively(string sceneName, bool needHUD, System.Action onSceneLoaded = null)
        {
            activateHUD = needHUD;
            _sceneManager.LoadSceneAdditively(sceneName, onSceneLoaded);
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"Scene {scene.name} loaded in {mode} mode.");

            // Llama a HUDState si el HUD es necesario
            if(canvasHUD != null)
            {
                Destroy(canvasHUD);
            }

            if (activateHUD)
            {
                canvasHUD = Instantiate(prefabHUD, contentHUD.transform);
                gamePause = canvasHUD.GetComponent<HUDController>().MenuGamePause;
                managerUser.SetupHUD(canvasHUD.GetComponent<HUDController>(), canvasHUD.GetComponent<HUDController>().ContentSelectAttack);
            }

        }

       
        public void LoadTitleScreen()
        {
            _notification.NotificationScreen(
                "Volver a la pantalla de título", 
                null,
                "¿Estas seguro? Si sales ahora se perderán los cambios no guardados",
                () =>
                {
                    // Acción a ejecutar al confirmar
                    ResetCanvas();
                    LoadScene("Init", false, () =>
                    {
                        Debug.Log("Title screen loaded.");
                    });
                }
            );
        }
            
       
        // Pausa el tiempo de juego
        public void TogglePauseGame()
        {
            if (Time.timeScale == 1f)
            {
                // Pausar el juego
                Time.timeScale = 0f;
            }
            else
            {
                // Reanudar el juego
                Time.timeScale = 1f;
            }
        }

        private void ResetCanvas()
        {
            gamePause.SetActive(false);
            canvasHUD.SetActive(false);
            TogglePauseGame();
        }
        

    }
}
