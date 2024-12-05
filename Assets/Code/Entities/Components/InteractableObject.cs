using UnityEngine;

namespace Game.Player
{
    public class InteractableObject : MonoBehaviour
    {
        [SerializeField] private InteractionType interactionType;

        // Detectar cuando el jugador entra en el trigger
        private void OnTriggerEnter(Collider other)
        {
            var playerController = other.GetComponent<ThirdPersonController>();
            if (playerController != null)
            {
                playerController.Interaction = interactionType;
            }
        }

        // Detectar cuando el jugador sale del trigger
        private void OnTriggerExit(Collider other)
        {
            var playerController = other.GetComponent<ThirdPersonController>();
            if (playerController != null)
            {
                playerController.ClearInteractionType(); // Restablece el tipo de interacción
            }
        }
    }
}
