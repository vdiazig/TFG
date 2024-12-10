using UnityEngine;

using InterfaceAdapters.Interactables;

namespace InterfaceAdpaters.Animations{
    public class AnimationEventReceiverInteractables : MonoBehaviour
    {
        [SerializeField] private ElementInteractable elementInteractable;

        // Métodos llamados desde el Animator para interactuar con los objetos de juego
        public void HiddenElement()
        {
            elementInteractable.HiddenElement();
        }

        public void DestroyElement()
        {
            elementInteractable.DestroyElement();
        }
    }
}
