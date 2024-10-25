public interface IPlayFabService
{
    void LoginWithEmail(string emailInputSesion, string passwordInputSesion, System.Action onSuccess, System.Action<string> onFailure);
    void LoginWithUsername(string username, string passwordInputSesion, System.Action onSuccess, System.Action<string> onFailure);
    void RegisterWithEmail(string username, string emailInputSesion, string passwordInputSesion, System.Action onSuccess, System.Action<string> onFailure);
    void RecoverPassword(string email, System.Action onSuccess, System.Action<string> onFailure);
}



