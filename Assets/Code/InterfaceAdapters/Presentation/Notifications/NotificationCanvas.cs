using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class NotificationCanvas : MonoBehaviour
{
    [Header("Up Notification")]
    [SerializeField] private GameObject upNotification;
    [SerializeField] private TMP_Text messageText; 

    // Colores para diferentes tipos de mensajes
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color errorColor = Color.red;


    [Header("Left Notification")]
    [SerializeField] private GameObject leftNotification;
    [SerializeField] private Transform container;
    [SerializeField] private NotificationElement notificationElementPrefab; 
    private int maxVisibleNotifications = 7;
    [SerializeField] private List<GameObject> activeNotifications = new List<GameObject>(); 



    private void Start()
    {
        // Oculta todas las notificaciones
        upNotification.SetActive(false);
        leftNotification.SetActive(false);
    }

    //______ NOTIFICACIONES PANEL LATERAL IZQUIERDO
    public void AddLeftNotification(Sprite image, string name)
    {
        leftNotification.SetActive(true);

        // Crear nueva notificación y añadirla al contenedor
        NotificationElement newElement = Instantiate(notificationElementPrefab, container);
        newElement.Initialize(image, name, this);
        activeNotifications.Add(newElement.gameObject);

        // Eliminar el más antiguo si excede el límite
        if (activeNotifications.Count > maxVisibleNotifications)
        {
            RemoveNotification(activeNotifications[0]);
        }

        newElement.AutoDestroy(2f); // Se eliminará automáticamente después de 2 segundos
    }

    // Método para eliminar notificaciones y actualizar la lista
    public void RemoveNotification(GameObject notification)
    {
        if (activeNotifications.Contains(notification))
        {
            activeNotifications.Remove(notification);
        }

        if (activeNotifications.Count == 0)
        {
            leftNotification.SetActive(false);
        }
    }



    //______ NOTIFICACIONES PARTE SUPERIOR
    // Método para mostrar un mensaje según el tipo de notificación
    public void Notification(string message, NotificationType type)
    {
        messageText.text = message;

        // Cambia el color según el tipo de notificación
        switch (type)
        {
            case NotificationType.CloseUp:
                CloseNotificationUp();
                break;
            case NotificationType.Success:
                messageText.color = successColor;
                break;
            case NotificationType.Warning:
                messageText.color = warningColor;
                break;
            case NotificationType.Error:
                messageText.color = errorColor;
                break;

        }

        upNotification.SetActive(true); // Muestra el panel de notificación
    }

    // Método para ocultar el panel de notificación superior
    public void CloseNotificationUp()
    {
        upNotification.SetActive(false);
    }

}
