using UnityEngine;

namespace Game.Player{
    public class AnimationEventReceiver : MonoBehaviour
    {
        [SerializeField] private ThirdPersonMovement thirdPersonMovement;
        [SerializeField] private ThirdPersonController thirdPersonController;

        // Método que Unity llamará desde el Animator
        public void EndInteract()
        {
            thirdPersonMovement.EndInteract();
            thirdPersonController.EndInteract();

        }
    }
}
