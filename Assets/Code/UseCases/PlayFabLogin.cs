using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class PlayFabLogin : MonoBehaviour
{
    private IPlayFabService playFabService;
    private INotification notification;  

    [Header("Login Inputs")]
    [SerializeField] public GameObject loginObject;
    private bool isLogin = true;
    [SerializeField] public TMP_InputField loginInputField;
    [SerializeField] public TMP_InputField passwordInputSesion;
    
    [Header("Register Inputs")]
    [SerializeField] public GameObject registerObject;
    [SerializeField] public TMP_InputField nameInputRegister;
    [SerializeField] public TMP_InputField emailInputSesionRegister;
    [SerializeField] public TMP_InputField passwordInputRegister;
    [SerializeField] public TMP_InputField passwordConfirmInputRegister;

    // Constructor para la inyección de dependencias
    public PlayFabLogin(IPlayFabService playFabService, INotification notification)
    {
        this.playFabService = playFabService;
        this.notification = notification;
    }

    private void Start()
    {
        // Inicia los servicios solo si no se han proporcionado en el constructor (para soporte en inspector)
        playFabService = new PlayFabService();
        notification = NotificationManager.Instance;

        CleanInputsRegister();
        CleanInputsSesion();
    }

    // Método para iniciar sesión con email o username y contraseña
    public void LoginWithEmailOrUsername()
    {
        string loginInput = loginInputField.text;

        if (IsValidEmail(loginInput))
        {
            playFabService.LoginWithEmail(loginInput, passwordInputSesion.text, OnLoginSuccess, OnLoginFailure);
        }
        else
        {
            playFabService.LoginWithUsername(loginInput, passwordInputSesion.text, OnLoginSuccess, OnLoginFailure);
        }
    }

    // Callback en caso de inicio de sesión exitoso
    private void OnLoginSuccess()
    {
        notification.NotificationUp("Inicio de sesión exitoso", NotificationType.Success, 3);
        Debug.Log("Login successful!");
        CleanInputsSesion();
    }

    // Callback en caso de fallo de inicio de sesión
    private void OnLoginFailure(string errorReport)
    {
        notification.NotificationUp("Usuario o contraseña incorrectos", NotificationType.Warning);
        Debug.LogWarning("Login failed!");
        Debug.LogError(errorReport);
    }

    // Limpia los inputs de inicio de sesión
    private void CleanInputsSesion(){
        loginInputField.text = "";
        passwordInputSesion.text = "";
    }

    // Método para registrar un nuevo usuario
    public void RegisterWithEmail()
    {
        if (passwordInputRegister.text != passwordConfirmInputRegister.text)
        {
            notification.NotificationUp("Las contraseñas no coinciden.", NotificationType.Warning);
            return;
        }

        playFabService.RegisterWithEmail(
            nameInputRegister.text, 
            emailInputSesionRegister.text, 
            passwordInputRegister.text, 
            OnRegisterSuccess, 
            OnRegisterFailure
        );
    }

    // Callback en caso de registro exitoso
    private void OnRegisterSuccess()
    {
        notification.NotificationUp("Registro exitoso", NotificationType.Success, 4);
        Debug.Log("Registration successful!");
        CleanInputsRegister();
        ChangeWindowSesion();
    }

    // Callback en caso de fallo en el registro
    private void OnRegisterFailure(string errorReport)
    {
        notification.NotificationUp("Error al registrar", NotificationType.Error);
        Debug.LogWarning("Registration failed!");
        Debug.LogError(errorReport);
    }

    // Limpia los inputs de registro
    private void CleanInputsRegister(){
        nameInputRegister.text = "";
        emailInputSesionRegister.text = "";
        passwordInputRegister.text = "";
        passwordConfirmInputRegister.text = "";
    }

    // Funcion para recuperar la contraseña
    public void RecoverPassword()
    {
        string email = loginInputField.text;

        if (IsValidEmail(email))
        {
            playFabService.RecoverPassword(email, OnRecoverPasswordSuccess, OnRecoverPasswordFailure);
        }
        else
        {
            notification.NotificationUp("Por favor, introduce un correo electrónico válido.", NotificationType.Warning);
        }
    }

    // Callback en caso de recuperación exitosa de contraseña
    private void OnRecoverPasswordSuccess()
    {
        notification.NotificationUp("Se ha enviado un correo de recuperación.", NotificationType.Success);
        Debug.Log("Password recovery email sent successfully!");
    }

    // Callback en caso de fallo en la recuperación de contraseña
    private void OnRecoverPasswordFailure(string errorReport)
    {
        notification.NotificationUp("Error al enviar el correo de recuperación.", NotificationType.Warning);
        Debug.LogWarning("Password recovery failed!");
        Debug.LogError(errorReport);
    }

    // Método para validar si el texto es un email
    private bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    // Cambia entre ventana de login y registro
    public void ChangeWindowSesion()
    {
        isLogin = !isLogin;

        loginObject.SetActive(isLogin);
        registerObject.SetActive(!isLogin);
    }
}
