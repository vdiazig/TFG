using UnityEngine;

namespace InterfaceAdapters.Presentation.Player
{
    public class CameraFollow : MonoBehaviour
    {
        private const string TARGET_NOT_SET = "Target not set, disabling component";
        private readonly string TAG = typeof(CameraFollow).ToString();
        [SerializeField][Tooltip("The camera that will follow the target")]
        private Camera playerCamera;
        [SerializeField][Tooltip("The target Transform (GameObject) to follow")]
        private Transform target;

        [SerializeField] private bool followOnStart = true;
        private bool isFollowing;


        [Header("3D Settings")]
        [SerializeField] private Vector3 offset3D;

        [Header("2D Settings")]
        [SerializeField] private bool is2D = false; // Define si la escena es en 2D
        [SerializeField] private Vector3 positionOffset2D = new Vector3(0, 20, 0); // Posición relativa en 2D
        [SerializeField] private Vector3 rotation2D = new Vector3(90, 0, 0); // Rotación fija en 2D


        
        private void Start()
        {

            if (followOnStart)
            {
                StartFollow();
            }
        }
        
        // Actualiza la posición de la cámara en cada frame según el player
        private void LateUpdate()
        {
            if (isFollowing && target != null)
            {
                if (!is2D)
                {
                    Vector3 targetPosition = target.position + offset3D;
                    playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, targetPosition, Time.deltaTime * 5);
                }
                else
                {
                    playerCamera.transform.position = target.position + positionOffset2D;
                    playerCamera.transform.rotation = Quaternion.Euler(rotation2D);
                }

                // Ajuste para mantener sincronización con el eje Z
                transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
            }
        }


        // Método para cambiar entre modos 2D y 3D
        public void Set2DMode(bool value)
        {
            is2D = value;
            playerCamera.GetComponent<Camera>().orthographic  = true;
        }

        public void StopFollow()
        {
            isFollowing = false;
        }

        public void StartFollow()
        {
            isFollowing = true;
        }
    }
}
