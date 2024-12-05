using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using Game.Player;
using UnityEngine.UI;


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


    [Header("Managers and Objects")]
    [SerializeField][Tooltip("Select from Inspector")] private GameObject prefabAttackHUD;
    [SerializeField][Tooltip("Select from Inspector, attack container")] private GameObject contentSelectAttack;
    [SerializeField][Tooltip("Select from Inspector")] private HUDController HUD;
    [SerializeField][Tooltip("Select from Inspector")] private ItemManager itemManager;


    [Header("Player")]
    [SerializeField][Tooltip("Searched in load scene")] private ThirdPersonController player;
    [SerializeField][Tooltip("Select from Inspector")]private GameObject defaultPrefabAttack;
    [SerializeField][Tooltip("Player change from HUD")] private GameObject prefabActualAttack;
    [SerializeField]private InteractionType interaction;
    public InteractionType Interaction => interaction;
    [SerializeField]private AttackPlayerType attackPlayer;
    public AttackPlayerType AttackPlayer => attackPlayer;


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

    // Actualización de las barras en el HUD
    private void UpdateHUD()
    {
        HUD.UpdateLifeBar(currentHealth, maxHealth);
        HUD.UpdateEnergyBar(currentEnergy, maxEnergy);
        HUD.UpdateOtherBar(currentOtherValue, maxOtherValue); // Actualiza la tercera barra
    }

    

    // Se llama al cargar una escena desde ManagerScenes para construir las opciones del HUD
    public void SetupHUD()
    {   
         prefabActualAttack = prefabActualAttack == null ? defaultPrefabAttack : prefabActualAttack;

        // Genera opciones de ataque en el HUD
        foreach (var weapon in itemManager.AttackObjectsPrefabs)
        {
            var weaponBase = weapon.GetComponent<WeaponBase>();
            if (weaponBase.IsUnlocked)
            {
                // Instanciar y configurar cada HUDAttack
                GameObject hudInstance = Instantiate(prefabAttackHUD, contentSelectAttack.transform, false);
                hudInstance.GetComponent<HUDAttack>().Setup(
                    weaponBase.WeaponID,
                    weaponBase.WeaponIcon,
                    weapon,
                    contentSelectAttack.GetComponent<ToggleGroup>(), 
                    this,
                    HUD
                );
            }
        }
        // Actualiza barras del HUD
        UpdateHUD();

        // Busca el player en la nueva escena
        player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();

        // Actualiza el ataque actual o por defecto
        updateSelectAttack(prefabActualAttack);

    }

    // Selección de ataques en el player y en el HUD
    public void updateSelectAttack(GameObject prefabAttack)
    {
        prefabActualAttack = prefabAttack;
        WeaponBase weapon = prefabActualAttack.GetComponent<WeaponBase>();
        
        attackPlayer = player.ActiveAttack = weapon.AttackPlayer;
        interaction = player.Interaction = weapon.Interaction;
        player.InstantiateWeapon(prefabActualAttack, weapon.RightHand);
    }



    // -------- SINCRONIZAR CON PLAYFAB TODOS LOS DATOS DE AQUÍ-----

}
