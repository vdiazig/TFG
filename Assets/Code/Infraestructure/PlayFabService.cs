using PlayFab;
using PlayFab.ClientModels;


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

    public void LoginWithEmail(string emailInputSesion, string passwordInputSesion, System.Action onSuccess, System.Action<string> onFailure)
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInputSesion,
            Password = passwordInputSesion
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, result => onSuccess(),
            error => onFailure(error.GenerateErrorReport()));
    }

    public void LoginWithUsername(string username, string passwordInputSesion, System.Action onSuccess, System.Action<string> onFailure)
    {
        var request = new LoginWithPlayFabRequest
        {
            Username = username,
            Password = passwordInputSesion
        };

        PlayFabClientAPI.LoginWithPlayFab(request, result => onSuccess(),
            error => onFailure(error.GenerateErrorReport()));
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

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, result => onSuccess(),
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

}
