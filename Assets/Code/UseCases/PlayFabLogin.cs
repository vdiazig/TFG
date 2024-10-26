using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class PlayFabLogin : MonoBehaviour
{
    private IPlayFabService playFabService;

    [Header("Login Inputs")]
    public TMP_InputField loginInputField;
    public TMP_InputField passwordInputSesion;
    
    [Header("Register Inputs")]
    public TMP_InputField nameInputRegister;
    public TMP_InputField emailInputSesionRegister;
    public TMP_InputField passwordInputRegister;
    public TMP_InputField passwordConfirmInputRegister;

    private void Start()
    {
        playFabService = new PlayFabService();
    }

    //_____ LOGIN DE USUARIO 
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
        NotificationManager.Instance.ShowNotification("Inicio de sesión exitoso", NotificationType.Success, 3);
        Debug.Log("Login successful!");
    }

    // Callback en caso de fallo de inicio de sesión
    private void OnLoginFailure(string errorReport)
    {
        NotificationManager.Instance.ShowNotification("Usuario o contraseña incorrectos", NotificationType.Warning);
        Debug.LogWarning("Login failed!");
        Debug.LogError(errorReport);
    }


    //_____ REGISTRO DE USUARIO 
    // Método para registrar un nuevo usuario
    public void RegisterWithEmail()
    {
        if (passwordInputRegister.text != passwordConfirmInputRegister.text)
        {
            NotificationManager.Instance.ShowNotification("Las contraseñas no coinciden.", NotificationType.Warning);
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
        NotificationManager.Instance.ShowNotification("Registro exitoso", NotificationType.Success, 4);
        Debug.Log("Registration successful!");
    }

    // Callback en caso de fallo en el registro
    private void OnRegisterFailure(string errorReport)
    {
        NotificationManager.Instance.ShowNotification("Error al registrar: " + errorReport, NotificationType.Error);
        Debug.LogWarning("Registration failed!");
        Debug.LogError(errorReport);
    }


    


    //_____ RECUPERACIÓN DE CONTRASEÑA
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
            NotificationManager.Instance.ShowNotification("Por favor, introduce un correo electrónico válido.", NotificationType.Warning);
        }
    }

    // Callback en caso de recuperación exitosa de contraseña
    private void OnRecoverPasswordSuccess()
    {
        NotificationManager.Instance.ShowNotification("Se ha enviado un correo de recuperación.", NotificationType.Success);
        Debug.Log("Password recovery email sent successfully!");
    }

    // Callback en caso de fallo la recuperación de contraseña
    private void OnRecoverPasswordFailure(string errorReport)
    {
        NotificationManager.Instance.ShowNotification("Error al enviar el correo de recuperación.", NotificationType.Warning);
        Debug.LogWarning("Password recovery failed!");
        Debug.LogError(errorReport);
    }



    //_____ OTROS MÉTODOS 
    // Método para validar si el texto es un email
    private bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

}
