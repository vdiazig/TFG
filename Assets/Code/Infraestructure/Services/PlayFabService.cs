using UnityEngine;
using System.Collections.Generic;
using System;

using PlayFab;
using PlayFab.ClientModels;

using Entities.Class;
using UseCases.Services;

namespace Infraestructure.Services
{
    public class PlayFabService : IPlayFabService
    {
        public PlayFabService()
        {
            // Inicializar PlayFab si es necesario
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = "A9405"; // TitleId de PlayFab
            }
        }


        //______ Acciones sobre la sesión de usuario
        public bool IsUserLoggedIn()
        {
            return PlayFabClientAPI.IsClientLoggedIn();
        }

        public void GetUserProfile(System.Action<UserProfile> onSuccess, System.Action<string> onFailure) 
        {
            var request = new GetAccountInfoRequest();

            PlayFabClientAPI.GetAccountInfo(request, result =>
            {
                var profile = new UserProfile
                {
                    DisplayName = result.AccountInfo.TitleInfo.DisplayName,
                    Email = result.AccountInfo.PrivateInfo.Email,
                    PlayFabId = result.AccountInfo.PlayFabId,
                };

                onSuccess(profile);
            },
            error =>
            {
                Debug.LogError("Error getting user profile: " + error.GenerateErrorReport());
                onFailure(error.GenerateErrorReport());
            });
        }

        public void ForgetSession(){
            PlayFabClientAPI.ForgetAllCredentials();
            Debug.Log("Forgotten session on PlayFab");
        }


        //______ Inicio de sesión y registro de usuario
        public void LoginWithEmail(string emailInputSesion, string passwordInputSesion, System.Action<UserProfile> onSuccess, System.Action<string> onFailure)
        {
            var request = new LoginWithEmailAddressRequest
            {
                Email = emailInputSesion,
                Password = passwordInputSesion
            };

            PlayFabClientAPI.LoginWithEmailAddress(request, result =>
            {
                // Después del login, obtenemos el perfil del usuario
                GetUserProfile(onSuccess, onFailure);
            },
            error =>
            {
                onFailure(error.GenerateErrorReport());
            });
        }

        public void LoginWithUsername(string username, string passwordInputSesion, System.Action<UserProfile> onSuccess, System.Action<string> onFailure)
        {
            var request = new LoginWithPlayFabRequest
            {
                Username = username,
                Password = passwordInputSesion
            };

            PlayFabClientAPI.LoginWithPlayFab(request, result =>
            {
                GetUserProfile(onSuccess, onFailure);
            },
            error =>
            {
                onFailure(error.GenerateErrorReport());
            });
        }

        public void RegisterWithEmail(string username, string emailInputSesion, string passwordInputSesion, System.Action onSuccess, System.Action<string> onFailure)
        {
            var registerRequest = new RegisterPlayFabUserRequest
            {
                Username = username,
                Email = emailInputSesion,
                Password = passwordInputSesion,
                RequireBothUsernameAndEmail = true
            };

            PlayFabClientAPI.RegisterPlayFabUser(registerRequest, 
                result => UpdateDisplayName(username, onSuccess, onFailure), 
                error => onFailure(error.GenerateErrorReport()));
        }

        private void UpdateDisplayName(string displayName, System.Action onSuccess, System.Action<string> onFailure)
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = displayName
            };

            PlayFabClientAPI.UpdateUserTitleDisplayName(request, 
                result => onSuccess(), 
                error => onFailure(error.GenerateErrorReport()));
        }



        public void RecoverPassword(string email, System.Action onSuccess, System.Action<string> onFailure)
        {
            var request = new SendAccountRecoveryEmailRequest
            {
                Email = email,
                TitleId = PlayFabSettings.staticSettings.TitleId
            };

            PlayFabClientAPI.SendAccountRecoveryEmail(request, result => onSuccess(),
                error => onFailure(error.GenerateErrorReport()));
        }

        //______ DATOS
        // Guarda los datos del jugador en PlayFab
        public void SavePlayerData(Dictionary<string, string> data, Action onSuccess, Action<string> onFailure)
        {
            var request = new UpdateUserDataRequest
            {
                Data = data
            };

            PlayFabClientAPI.UpdateUserData(request,
                result => onSuccess(),
                error => onFailure(error.ErrorMessage));
        }
        
        // Carga los datos del jugador desde PlayFab
        public void LoadPlayerData(Action<Dictionary<string, string>> onSuccess, Action<string> onFailure)
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
            {
                var data = new Dictionary<string, string>();
                if (result.Data != null)
                {
                    foreach (var entry in result.Data)
                    {
                        data[entry.Key] = entry.Value.Value;
                    }
                }
                onSuccess(data);
            }, 
            error => onFailure(error.GenerateErrorReport()));
        }

    }
}