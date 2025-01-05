using Unity.Mathematics;
using UnityEngine;

using Entities.Types;
using InterfaceAdapters.Presentation.Player;

namespace InterfaceAdapters.Managers
{

    public class ManagerLocalScene : MonoBehaviour
    {
        [SerializeField] private Vector3 playerStartPosition;
        [SerializeField] private bool isScene2D;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject containerPlayer;
        [SerializeField] private ManagerUser managerUser; 
        


        private void Awake()
        {
            managerUser = FindObjectOfType<ManagerUser>();
            
            // Instanciar al jugador
            GameObject playerInstance = Instantiate(playerPrefab, playerStartPosition, quaternion.identity);
            playerInstance.transform.SetParent(containerPlayer.transform);

            // Configurar CameraFollow
            playerInstance.GetComponentInChildren<CameraFollow>().Set2DMode(isScene2D);
            
        }


        public void HandleSceneChange(string nameScene, bool HUD)
        {
            managerUser.AddCollectedItemsToTotal(); // Agrega los ítems recolectados a la lista total
            managerUser.SaveData(
                // En caso de éxito
                () => ManagerScenes.Instance.LoadScene(nameScene, HUD),
                
                // En caso de error
                error =>
                {
                    var notification = NotificationManager.Instance;
                    notification.NotificationUp($"Error saving data: {error}", NotificationType.Error);
                }
            );
        }


    }

}