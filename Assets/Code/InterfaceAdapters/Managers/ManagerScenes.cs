using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


using Entities.Class;
using Infraestructure.Services;
using InterfaceAdapters.Interfaces;
using InterfaceAdapters.Presentation.HUD;

namespace InterfaceAdapters.Managers
{
    public class ManagerScenes : MonoBehaviour
    {
        public static ManagerScenes Instance { get; private set; } 
        [SerializeField] private ManagerUser managerUser;

        [Header("Areas data")]
        [SerializeField] private List<ExplorationData> explorationData = new List<ExplorationData>();


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
            _sceneManager = new SceneManagerService(); 
        }
        void Start(){
            _notification = NotificationManager.Instance;
        }


        // -------- GESTIÓN DE ESCENAS -----
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
            
       
        // -------- PAUSA DE JUEGO -----
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
        


    // -------- DATOS DE EXPLORACIÓN -----
        // Agregar nueva área de exploración
        public void AddExplorationArea(string id, bool isLocked = true, bool isCompleted = false)
        {
            var existingArea = explorationData.Find(data => data.Id == id);
            if (existingArea == null)
            {
                var newArea = new ExplorationData(id, isLocked, isCompleted);
                explorationData.Add(newArea);
                Debug.Log($"Exploration area added: {id} (Locked: {isLocked}, Completed: {isCompleted})");
            }
            else
            {
                Debug.LogWarning($"Exploration area with id {id} already exists.");
            }
        }

        // Alternar estado de bloqueo del área
        public void ToggleAreaLock(string id)
        {
            var area = explorationData.Find(data => data.Id == id);
            if (area != null)
            {
                area.ChangeLock();
                Debug.Log($"Toggled lock for area: {id}");
            }
            else
            {
                Debug.LogError($"Area with id {id} not found.");
            }
        }

        // Alternar estado de completado del área
        public void ToggleAreaComplete(string id)
        {
            var area = explorationData.Find(data => data.Id == id);
            if (area != null)
            {
                area.ChangeComplete();
                Debug.Log($"Toggled completion for area: {id}");
            }
            else
            {
                Debug.LogError($"Area with id {id} not found.");
            }
        }

        // Obtener todas las áreas de exploración
        public List<ExplorationData> GetAllExplorationAreas()
        {
            return new List<ExplorationData>(explorationData); // Devuelve una copia de la lista
        }

        // Actualiza las áreas de exploración desde una lista externa
        public void UpdateExplorationData(List<ExplorationData> newExplorationData)
        {
            explorationData.Clear();
            explorationData.AddRange(newExplorationData);
        }



    }
}
