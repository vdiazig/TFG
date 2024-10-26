using UnityEngine;
using System.Collections.Generic;

public class NotificationManager : MonoBehaviour
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


    //______ NOTIFICACIONES PANEL LATERAL IZQUIERDO
        // Mostrar notificación en el panel lateral con imagen y nombre
        public void ShowNotification(Sprite image, string name)
        {
            notificationCanvasInstance.AddLeftNotification(image, name);
        }


    //______ NOTIFICACIONES PARTE SUPERIOR
    // Mostrar una notificación en la parte superior
    public void ShowNotification(string message, NotificationType type)
    {
        notificationCanvasInstance.Notification(message, type); 
    }


    // Mostrar una notificación en la parte superior con cierre automático
    public void ShowNotification(string message, NotificationType type, float duration)
    {
        notificationCanvasInstance.Notification(message, type);
        
        // Cancelar cualquier cierre programado anterior y programar el nuevo cierre
        CancelInvoke("CloseNotificationUp");
        Invoke("CloseNotificationUp", duration);
    }

     // Cerrar el panel de notificaciones superior
    public void CloseNotificationUp()
    {
        notificationCanvasInstance.Notification("", NotificationType.CloseUp);
    }
}
