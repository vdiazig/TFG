using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NotificationElement : MonoBehaviour
{
    [SerializeField] private TMP_Text elementNameText;
    [SerializeField] private Image elementImage;
    private NotificationCanvas notificationCanvas; // Referencia al canvas para informar cuando se destruye

    public void Initialize(Sprite image, string name, NotificationCanvas canvas)
    {
        elementImage.sprite = image;
        elementNameText.text = name;
        notificationCanvas = canvas; // Asigna la referencia del canvas
    }

    // Llamada para autodestruir el elemento después de un tiempo y actualizar la lista
    public void AutoDestroy(float delay)
    {
        Invoke(nameof(DestroyElement), delay);
    }

    private void DestroyElement()
    {
        // Notificar al canvas para eliminar de la lista, y luego destruir este objeto
        if (notificationCanvas != null)
        {
            notificationCanvas.RemoveNotification(gameObject);
        }

        Destroy(gameObject); // Asegúrate de que este Destroy esté después de RemoveNotification
    }
}
