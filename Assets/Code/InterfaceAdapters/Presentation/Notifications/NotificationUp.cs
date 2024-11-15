using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NotificationUp : MonoBehaviour
{
    [SerializeField] private TMP_Text _elementNameText;

    // Colores para diferentes tipos de mensajes
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color errorColor = Color.red;

    private NotificationCanvas _notificationCanvas; // Referencia al canvas para informar cuando se destruye

    public void Initialize(string message, NotificationType type, NotificationCanvas canvas)
    {
        _elementNameText.text = message;
        _notificationCanvas = canvas; // Asigna la referencia del canvas

        switch (type)
        {
            case NotificationType.Success:
                _elementNameText.color = successColor;
                break;
            case NotificationType.Warning:
                _elementNameText.color = warningColor;
                break;
            case NotificationType.Error:
                _elementNameText.color = errorColor;
                break;

        }
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
            _notificationCanvas.NotificationUpClose(gameObject);
        }

        Destroy(gameObject); 
    }
}
