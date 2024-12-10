using UnityEngine;
using TMPro;
using UnityEngine.UI;

using InterfaceAdapters.Presentation.Notification;


namespace InterfaceAdapters.Presentation.Notification
{
    public class NotificationLeft : MonoBehaviour
    {
        [SerializeField] private TMP_Text _elementNameText;
        [SerializeField] private Image _elementImage;
        private NotificationCanvas _notificationCanvas; // Referencia al canvas para informar cuando se destruye
        private AudioSource _audioSource;

        public void Initialize(Sprite image, string name, NotificationCanvas canvas, AudioClip clip)
        {
            _elementImage.sprite = image;
            _elementNameText.text = name;
            _notificationCanvas = canvas; // Asigna la referencia del canvas
            _audioSource = this.GetComponent<AudioSource>();
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        // Llamada para autodestruir el elemento después de un tiempo y actualizar la lista
        public void AutoDestroy(float delay)
        {
            Invoke(nameof(DestroyElement), delay);
        }

        private void DestroyElement()
        {
            // Notificar al canvas para eliminar de la lista, y luego destruir este objeto
            if (_notificationCanvas != null)
            {
                _notificationCanvas.NotificationLeftClose(gameObject);
            }

            Destroy(gameObject); 
        }
    }
}