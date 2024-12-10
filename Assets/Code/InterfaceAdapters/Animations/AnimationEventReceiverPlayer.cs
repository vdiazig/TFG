using UnityEngine;

using InterfaceAdapters.Presentation.Player;

namespace InterfaceAdpaters.Animations{
    public class AnimationEventReceiver : MonoBehaviour
    {
        [SerializeField] private ThirdPersonMovement thirdPersonMovement;
        [SerializeField] private ThirdPersonController thirdPersonController;

        // Métodos llamados desde el Animator al finalizar la animación del player

        public void EndInteract()
        {
            thirdPersonMovement.EndInteract();
            thirdPersonController.EndInteract();

        }
    }
}
