using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class NotificationCanvas : MonoBehaviour
{
    [Header("Up Notification")]
    [SerializeField] private GameObject upNotification;
    [SerializeField] private NotificationUp notificationUpPrefab;
    private int maxVisibleNotificationsUp = 1;
    [SerializeField] private List<GameObject> activeNotificationsUp = new List<GameObject>(); 


    [Header("Left Notification")]
    [SerializeField] private GameObject leftNotification;
    [SerializeField] private Transform leftContainer;
    [SerializeField] private NotificationLeft notificationLeftPrefab; 
    private int maxVisibleNotificationsLeft = 7;
    [SerializeField] private List<GameObject> activeNotificationsLeft = new List<GameObject>(); 

    
    [Header("Screen Notification")]
    [SerializeField] private GameObject screenNotification;
    [SerializeField] private NotificationScreen notificationScreenPrefab;




    private void Start()
    {
       NotificationClean();
    }

    public void NotificationClean()
    {
        // Oculta todas las notificaciones
        upNotification.SetActive(false);
        leftNotification.SetActive(false);
        screenNotification.SetActive(false);
        
        // Destruir todos los objetos en activeNotificationsLeft
        activeNotificationsLeft.ForEach(notification => { if (notification != null) Destroy(notification); });

        // Destruir todos los objetos en activeNotificationsUp
        activeNotificationsUp.ForEach(notification => { if (notification != null) Destroy(notification); });

        // Limpiar las listas
        activeNotificationsLeft.Clear();
        activeNotificationsUp.Clear();

        Debug.Log("All notifications removed");
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
        NotificationLeft newElement = Instantiate(notificationLeftPrefab, leftContainer);
        newElement.Initialize(image, name, this);
        activeNotificationsLeft.Add(newElement.gameObject);

        // Eliminar el más antiguo si excede el límite
        if (activeNotificationsLeft.Count > maxVisibleNotificationsLeft)
        {
            NotificationLeftClose(activeNotificationsLeft[0]);
        }

        newElement.AutoDestroy(2f); // Se eliminará automáticamente después de 2 segundos
    }

    // Método para eliminar notificaciones y actualizar la lista
    public void NotificationLeftClose(GameObject notification)
    {
        if (activeNotificationsLeft.Contains(notification))
        {
            activeNotificationsLeft.Remove(notification);
        }

        if (activeNotificationsLeft.Count == 0)
        {
            leftNotification.SetActive(false);
        }
    }



    //______ NOTIFICACIONES PARTE SUPERIOR
    // Método para mostrar un mensaje según el tipo de notificación
    public void NotificationUp(string message, NotificationType type)
    {
        upNotification.SetActive(true);

        // Crear nueva notificación y añadirla al contenedor
        NotificationUp newElement = Instantiate(notificationUpPrefab, upNotification.transform);
        newElement.Initialize(message, type, this);
        activeNotificationsUp.Add(newElement.gameObject);

        // Eliminar el más antiguo si excede el límite
        if (activeNotificationsUp.Count > maxVisibleNotificationsUp)
        {
            NotificationUpClose(activeNotificationsUp[0]);
            
        }
    }

    // Método para eliminar notificaciones superiores
    public void NotificationUpClose(GameObject notification)
    {
        if (activeNotificationsUp.Contains(notification))
        {
            notification.GetComponent<NotificationUp> ().AutoDestroy(0);
            activeNotificationsUp.Remove(notification);
        }

        if (activeNotificationsUp.Count == 0)
        {
            upNotification.SetActive(false);
        }
    }

}
