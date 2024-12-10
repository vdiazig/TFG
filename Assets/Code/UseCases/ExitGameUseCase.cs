using UnityEngine;

using InterfaceAdapters.Interfaces;
using InterfaceAdapters.Managers;

namespace UseCases
{
    public class ExitGameUseCase
    {
        private readonly ManagerUser _managerUser;
        private INotification _notification;  


        public ExitGameUseCase(ManagerUser managerUser)
        {
            _managerUser = managerUser;
            _notification = NotificationManager.Instance; 

        }

        public void Execute()
        {
            _managerUser.LogoutUser(); 
            _notification.NotificationClean();
            Debug.Log("Close application");
            Application.Quit(); 
        }
    }
}
