using UnityEngine;
using InterfaceAdapters.Managers;

namespace  InterfaceAdapters.Interactables
{
    public class SceneTrigger : MonoBehaviour
    {
        public string sceneToLoad; // Nombre de la escena que se cargará
        public bool needHUD;
        public ManagerLocalScene localManager; // Referencia al LocalManagerScene

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // Verificar que el objeto que entra es el jugador
            {
                localManager.HandleSceneChange(sceneToLoad, needHUD); // Notificar al LocalManagerScene
            }
        }
    }
}
