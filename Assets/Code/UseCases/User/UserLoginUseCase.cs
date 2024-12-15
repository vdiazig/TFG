using System;

using Entities.Class;
using UseCases.Services;


namespace UseCases.User
{
    public class UserLoginUseCase
    {
        private readonly IPlayFabService _playFabService;

        public UserLoginUseCase(IPlayFabService playFabService)
        {
            _playFabService = playFabService;
        }
        
        
        //______ UseCase comprobación de usuario
        public bool IsUserLoggedIn()
        {
            return _playFabService.IsUserLoggedIn();
        }

        public void GetUserProfile(Action<UserProfile> onSuccess, Action<string> onFailure)
        {
            _playFabService.GetUserProfile(onSuccess, onFailure);
        }

        public void ForgetSession()
        {
            _playFabService.ForgetSession();
        }



     //______ UseCase Login y registro
        public void LoginWithEmail(string email, string password, Action<UserProfile> onSuccess, Action<string> onFailure)
        {
            _playFabService.LoginWithEmail(email, password, onSuccess, onFailure);
        }

        public void LoginWithUsername(string username, string password, Action<UserProfile> onSuccess, Action<string> onFailure)
        {
            _playFabService.LoginWithUsername(username, password, onSuccess, onFailure);
        }

        public void RegisterWithEmail(string username, string emailInputSesion, string passwordInputSesion, System.Action onSuccess, System.Action<string> onFailure)
        {
         _playFabService.RegisterWithEmail(username, emailInputSesion, passwordInputSesion, onSuccess, onFailure);
        }

        public void RecoverPassword(string email, System.Action onSuccess, System.Action<string> onFailure)
        {
            _playFabService.RecoverPassword(email, onSuccess, onFailure);
        }

    }
}
