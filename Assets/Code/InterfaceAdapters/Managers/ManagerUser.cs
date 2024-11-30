using UnityEngine;
using System;

public class ManagerUser : MonoBehaviour
{
    private IPlayFabService _playFabService, IPlayerStats;

    [Header("User Session Info")]
    [SerializeField] private bool _userSesion; // Indica si hay sesión activa
    [SerializeField] private string _displayName;
    [SerializeField] private string _email;
    [SerializeField] private string _playFabId;


    [Header("Player Stats")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxEnergy = 100;
    [SerializeField] private float currentEnergy;
    [SerializeField] private float maxOtherValue = 100; 
    [SerializeField] private float currentOtherValue;

    private void Start()
    {
        _playFabService = new PlayFabService(); 

        currentHealth = maxHealth;
        currentEnergy = maxEnergy;

        UpdateHUD();
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




   // Métodos para Vida
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHUD();

        if (currentHealth <= 0)
        {
            Debug.Log("Player has died!");
            // Aquí puedes manejar la lógica de muerte
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHUD();
    }

    // Métodos para Energía
    public void UseEnergy(float amount)
    {
        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        UpdateHUD();
    }

    public void RegainEnergy(float amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        UpdateHUD();
    }

    // Métodos para la Tercera Barra
    public void DecreaseOtherValue(float amount)
    {
        currentOtherValue -= amount;
        currentOtherValue = Mathf.Clamp(currentOtherValue, 0, maxOtherValue);
        UpdateHUD();
    }

    public void IncreaseOtherValue(float amount)
    {
        currentOtherValue += amount;
        currentOtherValue = Mathf.Clamp(currentOtherValue, 0, maxOtherValue);
        UpdateHUD();
    }

    // Actualización del HUD
    private void UpdateHUD()
    {
        HUDController hud = FindObjectOfType<HUDController>();
        if (hud != null)
        {
            hud.UpdateLifeBar(currentHealth, maxHealth);
            hud.UpdateEnergyBar(currentEnergy, maxEnergy);
            hud.UpdateOtherBar(currentOtherValue, maxOtherValue); // Actualiza la tercera barra
        }
    }

}
