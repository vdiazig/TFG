using UnityEngine;
using System;

using Entities.Types;

namespace InterfaceAdapters.Interfaces
{
    public interface INotification
    {
        void NotificationScreen(string title, Sprite image, string body, Action nextAction);
        void NotificationSidebar(Sprite image, string name, AudioClip clip);
        void NotificationUp(string message, NotificationType type);
        void NotificationUpClose();
        void NotificationClean();
    }
}
