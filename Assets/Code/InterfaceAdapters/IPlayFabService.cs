public interface IPlayFabService
{
    //______ Interfaz comprobación de sesion
    bool IsUserLoggedIn(); 
    void GetUserProfile(System.Action<UserProfile> onSuccess, System.Action<string> onFailure);
    void ForgetSession(); 

    //______ Interfaz para login y registro
    void LoginWithEmail(string emailInputSesion, string passwordInputSesion, System.Action<UserProfile> onSuccess, System.Action<string> onFailure);
    void LoginWithUsername(string username, string passwordInputSesion, System.Action<UserProfile> onSuccess, System.Action<string> onFailure);
    void RegisterWithEmail(string username, string emailInputSesion, string passwordInputSesion, System.Action onSuccess, System.Action<string> onFailure);
    void RecoverPassword(string email, System.Action onSuccess, System.Action<string> onFailure);
    
}



