using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

using Entities.Types;
using Entities.Class;
using Infraestructure.Services;
using InterfaceAdapters.Interfaces;
using InterfaceAdapters.Managers;

namespace InterfaceAdapters.Presentation.TitleSceen
{
    public class PlayFabLogin : MonoBehaviour
    {
        private IPlayFabService _playFabService;
        private INotification _notification;  
        [SerializeField] private ManagerUser managerUser;

        [Header("Navegation")]
        [SerializeField] public GameObject menuLogin;
        [SerializeField] public GameObject menuTitle;


        [Header("Login Inputs")]
        [SerializeField] public GameObject loginObject;
        private bool _isLogin = true;
        [SerializeField] public TMP_InputField loginInputField;
        [SerializeField] public TMP_InputField passwordInputSesion;

        
        [Header("Register Inputs")]
        [SerializeField] public GameObject registerObject;
        [SerializeField] public TMP_InputField nameInputRegister;
        [SerializeField] public TMP_InputField emailInputSesionRegister;
        [SerializeField] public TMP_InputField passwordInputRegister;
        [SerializeField] public TMP_InputField passwordConfirmInputRegister;

        private void Start()
        {
            _playFabService = new PlayFabService(); 
            _notification = NotificationManager.Instance; 

            CleanInputsRegister();
            CleanInputsSesion();
        }

        // Navegación entre menús
        public void toTitleMenu()
        {
            menuLogin.SetActive(false);
            menuTitle.SetActive(true);
        }

        // Método para iniciar sesión con email o username y contraseña
        public void LoginWithEmailOrUsername()
        {
            string loginInput = loginInputField.text;

            if (IsValidEmail(loginInput))
            {
                _playFabService.LoginWithEmail(loginInput, passwordInputSesion.text, OnLoginSuccess, OnLoginFailure);
            }
            else
            {
                _playFabService.LoginWithUsername(loginInput, passwordInputSesion.text, OnLoginSuccess, OnLoginFailure);
            }
        }


        // Callback en caso de inicio de sesión exitoso
        private void OnLoginSuccess(UserProfile profile)
        {
            managerUser.SetUserProfile(profile); 
            _notification.NotificationUp("Inicio de sesión exitoso", NotificationType.Success);
            Debug.Log("Login successful!");
            toTitleMenu();
            CleanInputsSesion();
        }


        // Callback en caso de fallo de inicio de sesión
        private void OnLoginFailure(string errorReport)
        {
            _notification.NotificationUp("Usuario o contraseña incorrectos", NotificationType.Warning);
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
                _notification.NotificationUp("Las contraseñas no coinciden.", NotificationType.Warning);
                return;
            }

            _playFabService.RegisterWithEmail(
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
            _notification.NotificationUp("Registro exitoso", NotificationType.Success);
            Debug.Log("Registration successful!");
            CleanInputsRegister();
            ChangeWindowSesion();
        }

        // Callback en caso de fallo en el registro
        private void OnRegisterFailure(string errorReport)
        {
            _notification.NotificationUp("Error al registrar", NotificationType.Error);
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
                _playFabService.RecoverPassword(email, OnRecoverPasswordSuccess, OnRecoverPasswordFailure);
            }
            else
            {
                _notification.NotificationUp("Por favor, introduce un correo electrónico válido.", NotificationType.Warning);
            }
        }

        // Callback en caso de recuperación exitosa de contraseña
        private void OnRecoverPasswordSuccess()
        {
            _notification.NotificationUp("Se ha enviado un correo de recuperación.", NotificationType.Success);
            Debug.Log("Password recovery email sent successfully!");
        }

        // Callback en caso de fallo en la recuperación de contraseña
        private void OnRecoverPasswordFailure(string errorReport)
        {
            _notification.NotificationUp("Error al enviar el correo de recuperación.", NotificationType.Warning);
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
            _isLogin = !_isLogin;

            loginObject.SetActive(_isLogin);
            registerObject.SetActive(!_isLogin);
        }
    }
}
