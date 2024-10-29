using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

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
    [SerializeField] private NotificationLeft notificationLeftPrefab; 
    private int maxVisibleNotifications = 7;
    [SerializeField] private List<GameObject> activeNotifications = new List<GameObject>(); 

    
    [Header("Screen Notification")]
    [SerializeField] private GameObject screenNotification;
    [SerializeField] private NotificationScreen notificationScreenPrefab;




    private void Start()
    {
        // Oculta todas las notificaciones
        upNotification.SetActive(false);
        leftNotification.SetActive(false);
        screenNotification.SetActive(false);
    }

    //______ NOTIFICACIONES A PANTALLA COMPLETA
    public void NotificationScreen (string title, Sprite image, string body, Action nextAction)
    {   
        screenNotification.SetActive(true);

        NotificationScreen newNotification = Instantiate(notificationScreenPrefab, screenNotification.transform);

        newNotification.Initialize(title, image, body, nextAction);

    }


    //______ NOTIFICACIONES PANEL LATERAL IZQUIERDO
    public void NotificationLeft(Sprite image, string name)
    {
        leftNotification.SetActive(true);

        // Crear nueva notificación y añadirla al contenedor
        NotificationLeft newElement = Instantiate(notificationLeftPrefab, container);
        newElement.Initialize(image, name, this);
        activeNotifications.Add(newElement.gameObject);

        // Eliminar el más antiguo si excede el límite
        if (activeNotifications.Count > maxVisibleNotifications)
        {
            NotificationLeftClose(activeNotifications[0]);
        }

        newElement.AutoDestroy(2f); // Se eliminará automáticamente después de 2 segundos
    }

    // Método para eliminar notificaciones y actualizar la lista
    public void NotificationLeftClose(GameObject notification)
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
    public void NotificationUp(string message, NotificationType type)
    {
        messageText.text = message;

        // Cambia el color según el tipo de notificación
        switch (type)
        {
            case NotificationType.CloseUp:
                NotificationUpClose();
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
    public void NotificationUpClose()
    {
        upNotification.SetActive(false);
    }

}
