using UnityEngine;
using System;

using Entities.Types;
using InterfaceAdapters.Interfaces;
using InterfaceAdapters.Presentation.Notification;


namespace InterfaceAdapters.Managers
{
    public class NotificationManager : MonoBehaviour, INotification
    {
        public static NotificationManager Instance { get; private set; }
        
        [SerializeField] private NotificationCanvas _notificationCanvasPrefab; 
        private NotificationCanvas _notificationCanvasInstance; 
        

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
                _notificationCanvasInstance = Instantiate(_notificationCanvasPrefab, transform);
            }
        }

        public void NotificationClean(){
            
            _notificationCanvasInstance.NotificationClean();
        }


        //______ NOTIFICACIONES A PANTALLA COMPLETA
        public void NotificationScreen(string title, Sprite image, string body, Action nextAction)
        {
            Debug.Log("Screen Notification Open");
            _notificationCanvasInstance.NotificationScreen(title, image, body, nextAction); 

        }



        //______ NOTIFICACIONES PANEL LATERAL IZQUIERDO
            // Mostrar notificación en el panel lateral con imagen y nombre
            public void NotificationSidebar(Sprite image, string name, AudioClip clip)
            {
                _notificationCanvasInstance.NotificationLeft(image, name, clip);
            }



        //______ NOTIFICACIONES PARTE SUPERIOR
        // Mostrar una notificación en la parte superior
        public void NotificationUp(string message, NotificationType type)
        {
            _notificationCanvasInstance.NotificationUp(message, type); 
        }


        // Cerrar el panel de notificaciones superior
        public void NotificationUpClose()
        {
            _notificationCanvasInstance.NotificationUp("", NotificationType.CloseUp);
        }
    }
}