using UnityEngine;

using Entities.Types;
using InterfaceAdapters.Managers;
using InterfaceAdapters.Interfaces;
using UseCases;

namespace InterfaceAdapters.Presentation.TitleSceen
{
    public class TitleScreenController : MonoBehaviour
    {
        private ExitGameUseCase _exitGameUseCase;
        private INotification _notification;  
        
        [SerializeField] public ManagerUser _managerUser;

        [SerializeField] public GameObject loginObject;
        [SerializeField] public GameObject titleObject;
        [SerializeField] private AudioClip buttonClip;
        [SerializeField] private AudioSource audioButtons;


        private void Start()
        {
            _managerUser = FindObjectOfType<ManagerUser>();
            _exitGameUseCase = new ExitGameUseCase(_managerUser);
            _notification = NotificationManager.Instance; 
            audioButtons = gameObject.AddComponent<AudioSource>();
            audioButtons.playOnAwake = false;
            audioButtons.clip = buttonClip;

        }


        // Salir del juego
        public void OnExitButtonPressed()
        {
            _exitGameUseCase.Execute();
        }

        // Comenzar a jugar
        public void OnPlayButtonPressed()
        {
            if (_managerUser.IsUserLoggedIn())
            {
                Debug.Log("Loading game....");

                // Llamar a LoadItems para cargar los datos del usuario antes de cargar la escena
                _managerUser.LoadData(
                    // En caso de éxito
                    () => 
                    {
                        Debug.Log("Player data loaded successfully.");
                        LoadGameScene();
                    },

                    // En caso de error
                    error =>
                    {
                        var notification = NotificationManager.Instance;
                        notification.NotificationUp($"Error loading player data: {error}", NotificationType.Error);
                        Debug.LogError($"Error loading player data: {error}");
                    }
                );
            }
            else
            {
                Debug.Log("Not session");
                ShowLoginPanel();
            }
        }

        // Cargar escena de juego
        private void LoadGameScene()
        {
            _notification.NotificationClean();
            ManagerScenes.Instance.LoadScene("Base", false);
        }

        // Mostrar panel de sesion y login
        private void ShowLoginPanel()
        {
            titleObject.SetActive(false);
            loginObject.SetActive(true);
        }

        // Sonido de botones
        public void PlayButton()
        {
            audioButtons.Play();
        }

    }
}
