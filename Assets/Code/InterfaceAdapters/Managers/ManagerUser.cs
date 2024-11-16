using UnityEngine;
using System;

public class ManagerUser : MonoBehaviour
{
    private IPlayFabService _playFabService;

    [Header("User Session Info")]
    [SerializeField] private bool _userSesion; // Indica si hay sesión activa
    [SerializeField] private string _displayName;
    [SerializeField] private string _email;
    [SerializeField] private string _playFabId;

    private void Start()
    {
        _playFabService = new PlayFabService(); 
    }

    //___ Sesión de usuario
    public bool IsUserLoggedIn()
    {
        _userSesion = _playFabService.IsUserLoggedIn();
        return _userSesion;
    }

    public void GetUserProfile(Action<UserProfile> onSuccess, Action<string> onFailure)
    {
        _playFabService.GetUserProfile(onSuccess, onFailure);
    }

    public void LogoutUser()
    {
        _playFabService.ForgetSession();
        ClearSessionData();
    }

    public void SetUserProfile(UserProfile profile)
    {
        _userSesion = true;
        _displayName = profile.DisplayName;
        _email = profile.Email;
        _playFabId = profile.PlayFabId;
    }

    private void ClearSessionData()
    {
        _userSesion = false;
        _displayName = string.Empty;
        _email = string.Empty;
        _playFabId = string.Empty;
    }



}
