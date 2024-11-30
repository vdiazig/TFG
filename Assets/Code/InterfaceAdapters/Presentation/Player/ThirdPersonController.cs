using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(ThirdPersonMovement))]
    public class ThirdPersonController : MonoBehaviour
    {
        private const float FALL_TIMEOUT = 0.15f;

        private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
        private static readonly int JumpHash = Animator.StringToHash("JumpTrigger");
        private static readonly int FreeFallHash = Animator.StringToHash("FreeFall");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");

        private Animator animator;
        private GameObject avatar;
        private ThirdPersonMovement thirdPersonMovement;

        private float fallTimeoutDelta;
        private HUDController hudController;

        [SerializeField][Tooltip("Useful to toggle input detection in editor")]
        private bool inputEnabled = true;

        private void Awake()
        {
            thirdPersonMovement = GetComponent<ThirdPersonMovement>();
        }

        public void Setup(GameObject target, RuntimeAnimatorController runtimeAnimatorController)
        {
            hudController = FindObjectOfType<HUDController>();
            if (hudController == null)
            {
                Debug.LogError("HUDController not found in the scene.");
                return;
            }

            hudController.OnJumpPressed += OnJump;

            avatar = target;
            thirdPersonMovement.Setup(avatar);

            animator = avatar.GetComponent<Animator>();
            animator.runtimeAnimatorController = runtimeAnimatorController;
            animator.applyRootMotion = false;
        }

        private void Update()
        {
            if (inputEnabled)
            {
                var joystickInput = hudController.GetJoystickDirection();
                thirdPersonMovement.Move(joystickInput.x, joystickInput.y);
            }

            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            var isGrounded = thirdPersonMovement.IsGrounded();
            animator.SetFloat(MoveSpeedHash, thirdPersonMovement.CurrentMoveSpeed);
            animator.SetBool(IsGroundedHash, isGrounded);

            if (isGrounded)
            {
                fallTimeoutDelta = FALL_TIMEOUT;
                animator.SetBool(FreeFallHash, false);
            }
            else
            {
                if (fallTimeoutDelta >= 0.0f)
                {
                    fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    animator.SetBool(FreeFallHash, true);
                }
            }
        }

        private void OnJump()
        {
            if (thirdPersonMovement.TryJump())
            {
                animator.SetTrigger(JumpHash);
            }
        }

        private void OnDestroy()
        {
            if (hudController != null)
            {
                hudController.OnJumpPressed -= OnJump;
            }
        }

        public void EnableInput()
        {
            inputEnabled = true;
        }

        public void DisableInput()
        {
            inputEnabled = false;
        }
    }
}
