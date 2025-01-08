using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

using Entities.Items;
using Entities.Types;
using Entities.Class;
using UseCases.Services;
using Infraestructure.Services;
using InterfaceAdapters.Presentation.HUD;
using InterfaceAdapters.Presentation.Player;
using InterfaceAdapters.Interfaces;
using InterfaceAdapters.Adapters;


namespace InterfaceAdapters.Managers
{
    public class ManagerUser : MonoBehaviour
    {
        private IPlayFabService _playFabService, IPlayerStats;
        private INotification _notification;  
        [SerializeField] ManagerScenes managerScenes;

        [Header("Player Info")]
        [SerializeField] private UserProfile profileUser;
            public UserProfile ProfileUser => profileUser;
        [SerializeField] private bool _userSesion; // Indica si hay sesión activa
        [SerializeField] private string _displayName;
        [SerializeField] private string _email;
        [SerializeField] private string _playFabId;
        [SerializeField] private IPlayerStats playerStats = new PlayerStatsData();
        public IPlayerStats PlayerStats => playerStats;
        private bool isPlayerDead = false;


        [Header("Items collected")]
        [SerializeField] private List<NewItem> listTotalItems;
        [SerializeField] private List<NewItem> listCollectedItems;


        [Header("Managers and Objects")]
        [SerializeField][Tooltip("Select from Inspector")] private GameObject prefabAttackHUD;
        [SerializeField][Tooltip("Select from Inspector, attack container")] private GameObject contentSelectAttack;
        [SerializeField][Tooltip("Select from Inspector")] private HUDController HUD;
        [SerializeField][Tooltip("Select from Inspector")] private ItemManagerService ItemManagerService;


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
            _notification = NotificationManager.Instance; 

        }

        // -------- SESIÓN DE USUARIO -----
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
            profileUser = profile;
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



        // -------- BARRAS DE VIDA Y ENERGÍA -----
        // Modifica la barra de vida y detecta vida agotada
        public void Life(float amount)
        {
            playerStats.Life(amount);
            UpdateHUD();

            if (isPlayerDead) return;

            if (playerStats.CurrentHealth <= 0)
            {
                Debug.Log("Player has died!");
                player.PlayerDie();
                isPlayerDead = true;

                // Muestra la notificación de Game Over
                _notification.NotificationScreen(
                    "No te queda energía para continuar.",                
                    null,                       
                    "Descansa y continua explorando mas adelante", 
                    () => managerScenes.LoadScene("Base", false)
                );
            }
        }

        // Modifica la barra de energía principal
        public void Energy(float amount)
        {
            playerStats.Energy(amount);
            UpdateHUD();
        }

        // Modifica la barra de energía secundaria
        public void OtherValue(float amount)
        {
            playerStats.OtherValue(amount);
            UpdateHUD();
        }



        // -------- CARGA INICIAL DEL HUD -----
        // Actualización de las barras en el HUD
        private void UpdateHUD()
        {
            if (HUD != null){
                HUD.UpdateLifeBar(playerStats.CurrentHealth, playerStats.MaxHealth);
                HUD.UpdateEnergyBar(playerStats.CurrentEnergy, playerStats.MaxEnergy);
                HUD.UpdateOtherBar(playerStats.CurrentOtherValue, playerStats.MaxOtherValue);
            }
        }

        // Se llama al cargar una escena desde ManagerScenes para construir las opciones del HUD
        public void SetupHUD(HUDController HUDcontroller, GameObject contSelectAttack)
        {   
            HUD = HUDcontroller;
            contentSelectAttack = contSelectAttack;

            // Resetea el joystick
            HUD.ResetJoystick();

            // Busca el player en la nueva escena
            player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();

            prefabActualAttack = prefabActualAttack == null ? defaultPrefabAttack : prefabActualAttack;

            // Elimina el contenido actual de contentSelectAttack
            foreach (Transform child in contentSelectAttack.transform)
            {
                Destroy(child.gameObject); // Destruye cada hijo
            }

            // Genera opciones de ataque en el HUD
            foreach (var weapon in ItemManagerService.AttackObjectsPrefabs)
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

            
            // Actualiza el ataque actual o por defecto
            updateSelectAttack(prefabActualAttack);

        }

        // Selección de ataques en el player y en el HUD
        public void updateSelectAttack(GameObject prefabAttack)
        {
            if (player != null)
            {
            prefabActualAttack = prefabAttack;
            WeaponBase weapon = prefabActualAttack.GetComponent<WeaponBase>();

            attackPlayer = player.ActiveAttack = weapon.AttackPlayer;
            interaction = player.Interaction = weapon.Interaction;
            player.InstantiateWeapon(prefabActualAttack, weapon.RightHand);
            }

        }


        // -------- NUEVOS ITEM TEMPORALES CONSEGUIDOS -----
        public void AddNewItem(int id, int quantity)
        {
            // Busca si el ítem ya existe en la lista
            NewItem existingItem = listCollectedItems.Find(item => item.id == id);
            if (existingItem != null)
            {
                // Si existe, aumenta la cantidad
                existingItem.quantity += quantity;
            }
            else
            {
                // Si no existe, añade un nuevo ítem
                listCollectedItems.Add(new NewItem(id, quantity));
            }
        }



        // -------- SINCRONIZAR DATOS CON PLAYFAB -----
        // Guardado de datos
        public void SaveData(Action onSuccess = null, Action<string> onFailure = null)
        {
            var serializedItems = JSONDataAdapter.SerializeItems(listTotalItems);
            var serializedStats = JSONDataAdapter.SerializePlayerStats((PlayerStatsData)playerStats);
            var serializedExplorationData = JSONDataAdapter.SerializeExplorationData(
                    managerScenes.GetAllExplorationAreas()
                );
            var gameData = new Dictionary<string, string>
            {
                { "Items", serializedItems },
                { "Stats", serializedStats },
                { "Exploration", serializedExplorationData }
            };

            _playFabService.SavePlayerData(gameData,
                () =>
                {
                    Debug.Log("Game data saved successfully.");
                    onSuccess?.Invoke();
                },
                error =>
                {
                    Debug.LogError($"Error saving game data: {error}");
                    onFailure?.Invoke(error);
                });
        }

        // Carga de datos
        public void LoadData(Action onSuccess = null, Action<string> onFailure = null)
        {
            _playFabService.LoadPlayerData(
                gameData =>
                {
                    if (gameData.ContainsKey("Items"))
                    {
                        listTotalItems = JSONDataAdapter.DeserializeItems(gameData["Items"]);
                        Debug.Log("Items loaded successfully.");
                    }

                    if (gameData.ContainsKey("Stats"))
                    {
                        playerStats = JSONDataAdapter.DeserializePlayerStats(gameData["Stats"]);
                        Debug.Log("Player stats loaded successfully.");
                    }

                    if (gameData.ContainsKey("Exploration"))
                    {
                        var explorationData = JSONDataAdapter.DeserializeExplorationData(gameData["Exploration"]);
                        managerScenes.UpdateExplorationData(explorationData); 
                        Debug.Log("Exploration data loaded successfully.");
                    }

                    onSuccess?.Invoke();
                },
                error =>
                {
                    Debug.LogError($"Error loading game data: {error}");
                    onFailure?.Invoke(error);
                });
        }



        // Combina listCollectedItems con listTotalItems
        public void AddCollectedItemsToTotal()
        {
            foreach (var collectedItem in listCollectedItems)
            {
                var existingItem = listTotalItems.FirstOrDefault(item => item.id == collectedItem.id);
                if (existingItem != null)
                {
                    existingItem.quantity += collectedItem.quantity; // Suma la cantidad
                }
                else
                {
                    listTotalItems.Add(new NewItem(collectedItem.id, collectedItem.quantity)); // Añade el nuevo ítem
                }
            }
            listCollectedItems.Clear();
            Debug.Log("Collected items added to total.");
        }

        // Resta ítems usados
        public bool UseItems(int id, int quantity)
        {
            var existingItem = listTotalItems.FirstOrDefault(item => item.id == id);
            if (existingItem != null && existingItem.quantity >= quantity)
            {
                existingItem.quantity -= quantity; // Resta la cantidad
                if (existingItem.quantity == 0)
                {
                    listTotalItems.Remove(existingItem); // Elimina el ítem si llega a 0
                }
                SaveData(); // Guarda el estado actualizado
                Debug.Log($"Used {quantity} of item {id}. Remaining: {existingItem.quantity}");
                return true;
            }
            else
            {
                Debug.LogWarning("Not enough items to use.");
                return false;
            }
        }

        // Devuelve los ítems totales del usuario
        public List<NewItem> GetTotalItems()
        {
            return listTotalItems;
        }


    }
}