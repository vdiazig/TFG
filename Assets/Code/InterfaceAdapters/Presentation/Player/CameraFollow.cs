using ReadyPlayerMe.Core;
using UnityEngine;

namespace ReadyPlayerMe.Samples.QuickStart
{
    public class CameraFollow : MonoBehaviour
    {
        private const string TARGET_NOT_SET = "Target not set, disabling component";
        private readonly string TAG = typeof(CameraFollow).ToString();
        [SerializeField][Tooltip("The camera that will follow the target")]
        private Camera playerCamera;
        [SerializeField][Tooltip("The target Transform (GameObject) to follow")]
        private Transform target;
        
        [SerializeField][Tooltip("Defines the camera distance from the player along Y and Z axis")]
        private float cameraDistance;

        [SerializeField] private bool followOnStart = true;
        private bool isFollowing;
        
        private void Start()
        {
            if (target == null)
            {
                SDKLogger.LogWarning(TAG, TARGET_NOT_SET);
                enabled = false;
                return;
            }

            if (followOnStart)
            {
                StartFollow();
            }
        }
        
        private void LateUpdate()
        {
            if (isFollowing)
            {
        // Ajuste: Mantener un offset fijo en Y
        Vector3 targetPosition = target.position + new Vector3(0, cameraDistance, -cameraDistance); 
        playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, targetPosition, Time.deltaTime * 5);
        
        // Permitir movimiento en Z basado en el terreno
        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
    }
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
