using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;


public class PlayFabLogin : MonoBehaviour
{
    private IPlayFabService playFabService;

    [Header("Feedback message user")]
    public TMP_Text feedbackText;

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

     // Método para iniciar sesión con email o username y contraseña
    public void LoginWithEmailOrUsername()
    {
        string loginInput = loginInputField.text;

        // Determinamos si es un email o un nombre de usuario
        if (IsValidEmail(loginInput))
        {
            playFabService.LoginWithEmail(loginInput, passwordInputSesion.text, OnLoginSuccess, OnLoginFailure);
        }
        else
        {
            playFabService.LoginWithUsername(loginInput, passwordInputSesion.text, OnLoginSuccess, OnLoginFailure);
        }
    }

    // Método para validar si el texto es un email
    private bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    // Método para registrar un nuevo usuario
    public void RegisterWithEmail()
    {
        if (passwordInputRegister.text != passwordConfirmInputRegister.text)
        {
            feedbackText.text = "Las contraseñas no coinciden.";
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


    // Callback en caso de inicio de sesión exitoso
    private void OnLoginSuccess()
    {
        feedbackText.text = "Inicio de sesión exitoso";
        Debug.Log("Login successful!");
    }

    // Callback en caso de fallo de inicio de sesión
    private void OnLoginFailure(string errorReport)
    {
        feedbackText.text = "Usuario o contraseña incorrectos";
        Debug.LogWarning("Login failed!");
        Debug.LogError(errorReport);
    }

    // Callback en caso de registro exitoso
    private void OnRegisterSuccess()
    {
        feedbackText.text = "Registro exitoso";
        Debug.Log("Registration successful!");
    }

    // Callback en caso de fallo en el registro
    private void OnRegisterFailure(string errorReport)
    {
        feedbackText.text = "Error al registrar: " + errorReport;
        Debug.LogWarning("Registration failed!");
        Debug.LogError(errorReport);
    }

    
    // Callback en caso de recuperación exitosa
    private void OnRecoverPasswordSuccess()
    {
        feedbackText.text = "Se ha enviado un correo de recuperación.";
        Debug.Log("Password recovery email sent successfully!");
    }

    // Callback en caso de fallo la recuperación de contraseña
    private void OnRecoverPasswordFailure(string errorReport)
    {
        feedbackText.text = "Error al enviar el correo de recuperación.";
        Debug.LogWarning("Password recovery failed!");
        Debug.LogError(errorReport);
    }

    // Método para recuperar contraseña
    public void RecoverPassword()
    {
        string email = loginInputField.text;

        if (IsValidEmail(email))
        {
            playFabService.RecoverPassword(email, OnRecoverPasswordSuccess, OnRecoverPasswordFailure);
        }
        else
        {
            feedbackText.text = "Por favor, introduce un correo electrónico válido.";
        }
    }



}


