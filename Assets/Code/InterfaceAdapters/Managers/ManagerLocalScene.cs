using Unity.Mathematics;
using UnityEngine;

using InterfaceAdapters.Presentation.Player;

namespace InterfaceAdapters.Managers
{

    public class ManagerLocalScene : MonoBehaviour
    {
        [SerializeField] private Vector3 playerStartPosition;
        [SerializeField] private bool isScene2D;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject containerPlayer;
        


        private void Awake()
        {
            // Instanciar al jugador
            GameObject playerInstance = Instantiate(playerPrefab, playerStartPosition, quaternion.identity);
            playerInstance.transform.SetParent(containerPlayer.transform);

            // Configurar CameraFollow
            playerInstance.GetComponentInChildren<CameraFollow>().Set2DMode(isScene2D);
            
        }


        public void HandleSceneChange(string nameScene)
        {
            ManagerScenes.Instance.LoadScene(nameScene, false);
        }
    }

}