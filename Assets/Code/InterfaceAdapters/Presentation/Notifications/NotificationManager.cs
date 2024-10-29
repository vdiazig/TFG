using UnityEngine;
using System;

public class NotificationManager : MonoBehaviour, INotification
{
    public static NotificationManager Instance { get; private set; }
    
    [SerializeField] private NotificationCanvas notificationCanvasPrefab; 
    private NotificationCanvas notificationCanvasInstance; 
    

    // Única instancia del objeto que no se destruye entre escenas
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

            // Instanciamos el prefab y lo colocamos como hijo de este GameObject (Notification)
            notificationCanvasInstance = Instantiate(notificationCanvasPrefab, transform);
        }
    }


    //______ NOTIFICACIONES A PANTALLA COMPLETA
    public void NotificationScreen(string title, Sprite image, string body, Action nextAction)
    {
        Debug.Log("Screen Notification Open");
        notificationCanvasInstance.NotificationScreen(title, image, body, nextAction); 

    }




    //______ NOTIFICACIONES PANEL LATERAL IZQUIERDO
        // Mostrar notificación en el panel lateral con imagen y nombre
        public void NotificationLeft(Sprite image, string name)
        {
            notificationCanvasInstance.NotificationLeft(image, name);
        }


    //______ NOTIFICACIONES PARTE SUPERIOR
    // Mostrar una notificación en la parte superior
    public void NotificationUp(string message, NotificationType type)
    {
        notificationCanvasInstance.NotificationUp(message, type); 
    }


    // Mostrar una notificación en la parte superior con cierre automático
    public void NotificationUp(string message, NotificationType type, float duration)
    {
        notificationCanvasInstance.NotificationUp(message, type);
        
        // Cancelar cualquier cierre programado anterior y programar el nuevo cierre
        CancelInvoke("CloseNotificationUp");
        Invoke("CloseNotificationUp", duration);
    }

     // Cerrar el panel de notificaciones superior
    public void NotificationUpClose()
    {
        notificationCanvasInstance.NotificationUp("", NotificationType.CloseUp);
    }
}
