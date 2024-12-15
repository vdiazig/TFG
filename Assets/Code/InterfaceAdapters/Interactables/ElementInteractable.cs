using UnityEngine;

using InterfaceAdapters.Presentation.Player;
using Infraestructure.Services;

namespace InterfaceAdapters.Interactables
{
    public class ElementInteractable : MonoBehaviour
    {
        private ItemManagerService ItemManagerService; // Gestor de prefabs
        private GameObject rewardPrefab; // Prefab de la recompensa
        private ThirdPersonController thirdPersonController; // Controlador de player
        private bool isDestroyed = false; // Evita múltiples activaciones

        [SerializeField] private GameObject ImageObject; // Imagen del elemento
        [SerializeField] private GameObject animator; // Animator para la animación
        [SerializeField] private AudioClip clip; // Clip de audio
        [SerializeField] private AudioSource aSource; // Fuente de audio

        private void Start()
        {
            // Busca el ItemManagerService en la escena
            ItemManagerService = FindObjectOfType<ItemManagerService>();
            rewardPrefab = ItemManagerService.GetRandomPrefab();
            thirdPersonController = FindObjectOfType<ThirdPersonController>();
            aSource.clip = clip;
        }
        
        // Detecta colisión con el arma del player
        private void OnTriggerEnter(Collider other)
        {
            // Verifica si el objeto que entra tiene el tag "Weapon" y si el personaje lo esta usando
            if (other.CompareTag("Weapon") && !isDestroyed && thirdPersonController.IsUsing)
            {
                isDestroyed = true; 
                TriggerDestroySequence();
            }
        }

        // Activa la animación de destrucción y el sonido
        private void TriggerDestroySequence()
        {
            animator.SetActive(true);
            animator.GetComponent<Animator>().SetTrigger("Destroy"); 
            aSource.Play();
        }


        // Funciones llamadas desde AnimationEventReceiverInteractuables.cs durante la animación.
        public void HiddenElement()
        {
            ImageObject.SetActive(false);
        }


        public void DestroyElement()
        {
            // Instancia la recompensa
            Instantiate(rewardPrefab, this.gameObject.transform.position, Quaternion.identity);

            //Destruye el objeto
            Destroy(gameObject);
        }
    }
}
