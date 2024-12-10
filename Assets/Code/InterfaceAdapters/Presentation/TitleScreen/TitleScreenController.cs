using UnityEngine;

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


        private void Start()
        {
            _exitGameUseCase = new ExitGameUseCase(_managerUser);
            _notification = NotificationManager.Instance; 

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
                LoadGameScene();
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
            ManagerScenes.Instance.LoadScene("test2", true, () => 
            {
                Debug.Log("Game scene exp1-01 loaded.");
            });
        }

        // Mostrar panel de sesion y login
        private void ShowLoginPanel()
        {
            titleObject.SetActive(false);
            loginObject.SetActive(true);
        }
    }
}
