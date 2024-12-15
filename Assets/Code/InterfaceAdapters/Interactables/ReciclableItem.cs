using UnityEngine;
using UnityEngine.UI;

using InterfaceAdapters.Interfaces;
using InterfaceAdapters.Managers;

namespace InterfaceAdapters.Interactables
{
    public class RecyclableItem : MonoBehaviour
    {
        // Attributes
        [SerializeField]private int objectID;                  // Id del item
            public int ObjectID => objectID;
        [SerializeField]private string type;              // Material type (plastic, metal, etc.)
            public string Type => type;
        [SerializeField]private string nameItem;         // Name item
            public string NameItem => nameItem;
        [SerializeField]private float economicValue;      // Economic value of the recyclable item
            public float EconomicValue => economicValue;
        [SerializeField]private int quantity;            // Cantidad
            public int Quantity => quantity;

        [SerializeField]private Sprite itemSprite; 
            public Sprite ItemSprite => itemSprite;           // Visual representation of the item
        [SerializeField]private AudioClip audioClip;      // Sound to play when the item is collected
        
        private INotification _notification; 
        private ManagerUser managerUser;
        private Image imageSource;
        

        // Initialize the audio source
        void Start()
        {
            imageSource = gameObject.AddComponent<Image>();
            imageSource.sprite = ItemSprite;

            managerUser = FindObjectOfType<ManagerUser>();
            _notification = NotificationManager.Instance;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                managerUser.AddNewItem(objectID, quantity);
                _notification.NotificationSidebar(ItemSprite, nameItem, audioClip);

                // Desactiva el objeto
                gameObject.SetActive(false);

                // Destruye después de que termine el sonido
                Destroy(gameObject, audioClip.length);
            }
        }

    }
}
