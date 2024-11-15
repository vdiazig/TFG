using UnityEngine;
using System;
public interface INotification
{
    void NotificationScreen(string title, Sprite image, string body, Action nextAction);
    void NotificationLeft(Sprite image, string name);
    void NotificationUp(string message, NotificationType type);
    void NotificationUpClose();
    void NotificationClean();
}
