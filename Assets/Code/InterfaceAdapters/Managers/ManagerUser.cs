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


namespace InterfaceAdapters.Managers
{
    public class ManagerUser : MonoBehaviour
    {
        private IPlayFabService _playFabService, IPlayerStats;

        [Header("User Session Info")]
        [SerializeField] private UserProfile profileUser;
            public UserProfile ProfileUser => profileUser;
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

            currentHealth = maxHealth;
            currentEnergy = maxEnergy;

            //UpdateHUD();
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

        
        // -------- CARGA INICIAL DEL HUD -----
        // Actualización de las barras en el HUD
        private void UpdateHUD()
        {
            HUD.UpdateLifeBar(currentHealth, maxHealth);
            HUD.UpdateEnergyBar(currentEnergy, maxEnergy);
            HUD.UpdateOtherBar(currentOtherValue, maxOtherValue); // Actualiza la tercera barra
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


        // -------- SINCRONIZAR CON PLAYFAB TODOS LOS DATOS DE AQUÍ -----

         // Carga los ítems desde PlayFab y actualiza listTotalItems
        public void LoadItems(Action onSuccess, Action<string> onFailure)
        {
            _playFabService.LoadPlayerData(
                data =>
                {
                    if (data.ContainsKey("Items"))
                    {
                        var itemsString = data["Items"]; // Recupera los ítems como JSON
                        listTotalItems = JsonUtility.FromJson<ItemListWrapper>(itemsString).items; // Deserializa
                        Debug.Log("Items loaded successfully.");
                        onSuccess?.Invoke(); // Llama al callback de éxito
                    }
                    else
                    {
                        Debug.LogWarning("No items found in PlayFab.");
                        onSuccess?.Invoke(); // Llama al callback de éxito aunque no haya datos
                    }
                },
                error =>
                {
                    Debug.LogError($"Error loading items: {error}");
                    onFailure?.Invoke(error); // Llama al callback de error con el mensaje correspondiente
                }
            );
        }


         // Guarda los ítems actuales en PlayFab
        public void SaveItems(Action onSuccess = null, Action<string> onFailure = null)
        {
            var data = new Dictionary<string, string>
            {
                { "Items", JsonUtility.ToJson(new ItemListWrapper { items = listTotalItems }) }
            };

            _playFabService.SavePlayerData(data,
                () =>
                {
                    Debug.Log("Items saved successfully.");
                    onSuccess?.Invoke(); // Llama al callback de éxito si está definido
                },
                error =>
                {
                    Debug.LogError($"Error saving items: {error}");
                    onFailure?.Invoke(error); // Llama al callback de error si está definido
                }
            );
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
                SaveItems(); // Guarda el estado actualizado
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