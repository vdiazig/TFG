using UnityEngine;

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
        Debug.Log("NEW SCENE");
        _notification.NotificationClean();
        //UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    // Mostrar panel de sesion y login
    private void ShowLoginPanel()
    {
        titleObject.SetActive(false);
        loginObject.SetActive(true);
    }
}
