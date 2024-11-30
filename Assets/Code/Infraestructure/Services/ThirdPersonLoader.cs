using System;
using UnityEngine;

namespace Game.Player
{
    public class ThirdPersonLoader : MonoBehaviour
    {
        [SerializeField][Tooltip("Avatar to load (assign in the inspector)")]
        private GameObject avatar;

        [SerializeField][Tooltip("Animator to use on loaded avatar")]
        private RuntimeAnimatorController animatorController;

        private readonly Vector3 avatarPositionOffset = new Vector3(0, -0.08f, 0);

        public event Action OnLoadComplete;

        private void Start()
        {
            SetupAvatar();
        }

        private void SetupAvatar()
        {

            // Re-parent and reset transforms
            avatar.transform.parent = transform;
            avatar.transform.localPosition = avatarPositionOffset;
            avatar.transform.localRotation = Quaternion.identity;

            // Set up the controller
            var controller = GetComponent<ThirdPersonController>();
            if (controller != null)
            {
                controller.Setup(avatar, animatorController);
            }

            OnLoadComplete?.Invoke();
        }
    }
}
