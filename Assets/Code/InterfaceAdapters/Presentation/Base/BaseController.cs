using UnityEngine;
using UnityEngine.UI;
using TMPro;

using InterfaceAdapters.Managers;
using InterfaceAdapters.Interactables;
using Infraestructure.Services;
using Entities.Class;

namespace InterfaceAdapters.Presentation.Base
{
    public class BaseController : MonoBehaviour
    {
        [Header("User data")]
        [SerializeField] private TMP_Text nameUser;
        [SerializeField] private TMP_Text emailUser;


        [Header("Scene Elements")]
        [SerializeField] private GameObject mapObject; // Asignar el objeto del mapa en el inspector
        [SerializeField] private GameObject optionsObject; // Asignar el menú de opciones en el inspector
        [SerializeField] private GameObject itemObject;
        [SerializeField] private Transform itemContainer; // Contenedor para los prefabs de ítems
        [SerializeField] private GameObject prefabItemUI;


        [Header("Buttons")]

        [SerializeField] private Button butItems;
        [SerializeField] private Button butMap;
        [SerializeField] private Button butOptions;

        [Header("Managers")]
        [SerializeField] private ManagerScenes managerScenes;
        [SerializeField] private ManagerUser managerUser;
        [SerializeField] private ItemManagerService itemManagerService;




        private void Start()
        {

            managerScenes = FindObjectOfType<ManagerScenes>();
            managerUser = FindObjectOfType<ManagerUser>();
            itemManagerService = FindObjectOfType<ItemManagerService>();
            
            
            // Cargar datos de usuario
            LoadUserData();
            LoadUserItems();

            managerUser.Life(100);
        }

        private void LoadUserData()
        {
            UserProfile profile = managerUser.ProfileUser; 
            if (profile != null) 
            {
                nameUser.text = profile.DisplayName; 
                emailUser.text = profile.Email; 
            }
            else
            {
                Debug.LogError("UserProfile is null.");
            }
        }

        // Botón "Explorar"
        public void OnExploreButtonPressed()
        {
            managerScenes.LoadScene("Area1-01", true);
        }


        // Botón "Ver Objetos"
        public void OnItemButtonPressed()
        {
            UpdateMenu(butItems);
            itemObject.SetActive(true); 
        }

        // Botón "Ver Mapa"
        public void OnMapButtonPressed()
        {
            UpdateMenu(butMap);
            mapObject.SetActive(true); 
        }

        // Botón "Opciones"
        public void OnOptionsButtonPressed()
        {
            UpdateMenu(butOptions);
            optionsObject.SetActive(true); 
        }


        private void UpdateMenu(Button buttonSelect)
        {
            // Define los colores
            Color colorWhite = new Color(0.925f, 0.925f, 0.925f); // #ececec
            Color colorBlack = new Color(0, 0, 0, 0.94f);         // Negro con opacidad 240

            // Desactivar todos los objetos asociados
            mapObject.SetActive(false);
            optionsObject.SetActive(false);
            itemObject.SetActive(false);

            // Lista de botones a procesar
            Button[] buttons = { butItems, butMap, butOptions };

            // Iterar sobre los botones y resetear su estado visual
            foreach (var button in buttons)
            {
                button.GetComponentInChildren<TMP_Text>().color = colorWhite; // Texto blanco
                button.GetComponent<Image>().color = colorBlack;             // Fondo negro
            }

            // Aplicar estilo al botón seleccionado
            buttonSelect.GetComponentInChildren<TMP_Text>().color = colorBlack; // Texto negro
            buttonSelect.GetComponent<Image>().color = colorWhite;             // Fondo blanco
        }



        // Botón "Cerrar Sesión"
        public void OnLogoutButtonPressed()
        {
            managerUser.LogoutUser();

            // Reiniciar cargando la escena Init
            managerScenes.LoadScene("Init", false);
        }


        // Método para cargar los ítems del usuario en pantalla
        private void LoadUserItems()
        {
            // Asegúrate de limpiar el contenedor para evitar duplicados
            foreach (Transform child in itemContainer)
            {
                Destroy(child.gameObject);
            }

            // Obtener la lista de ítems del usuario
            var totalItems = managerUser.GetTotalItems();

            // Iterar sobre los ítems del usuario
            foreach (var userItem in totalItems)
            {
                // Buscar el prefab correspondiente al ID del ítem
                var prefab = itemManagerService.itemPrefabs.Find(p => p.GetComponent<RecyclableItem>().ObjectID == userItem.id);

                if (prefab != null)
                {
                    // Obtener los datos del prefab
                    var recyclableItem = prefab.GetComponent<RecyclableItem>();

                    // Instanciar el prefab de UI
                    var itemPrefabUI = Instantiate(prefabItemUI, itemContainer);

                    // Configurar el UI con los datos del ítem
                    itemPrefabUI.GetComponent<ItemUI>().Setup(
                        recyclableItem.Type,
                        recyclableItem.NameItem,
                        userItem.quantity,
                        recyclableItem.ItemSprite
                    );
                }
                else
                {
                    Debug.LogWarning($"No prefab found for item with ID {userItem.id}");
                }
            }
        }

    }
}
