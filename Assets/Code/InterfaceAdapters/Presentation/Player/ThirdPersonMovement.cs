using UnityEngine;
using System.Collections;



namespace Game.Player
{
    [RequireComponent(typeof(CharacterController), typeof(GroundCheck))]
    public class ThirdPersonMovement : MonoBehaviour
    {
        private const float TURN_SMOOTH_TIME = 0.05f;

        [SerializeField][Tooltip("Used to determine movement direction based on input and camera forward axis")] 
        private Transform playerCamera;

        [Header("Movement Settings")]

        [SerializeField][Tooltip("Run speed of the character")] 
        private float runSpeed = 6f;
        [SerializeField][Tooltip("Multiplier to apply to movement speed when crouching")]
        private float crouchSpeed = 3f;

        [Header("Jump and Gravity")]
        [SerializeField][Tooltip("Gravity")] 
        private float gravity = -18f;
        [SerializeField][Tooltip("The height the player can jump ")] 
        private float jumpHeight = 3f;

        private CharacterController controller;
        private GameObject avatar;

        private float verticalVelocity;
        private float turnSmoothVelocity;

        public float CurrentMoveSpeed { get; private set; }
        public float CurrentJoystickDirection {get; private set;}
        public bool IsCrouching { get; private set; }
        private GroundCheck groundCheck;
        private bool jumpTrigger;
        private bool isInteracting;


        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            groundCheck = GetComponent<GroundCheck>();
        }

        public void Setup(GameObject target)
        {
            avatar = target;
        }




        // Movimiento y velocidad
        public void Move(float inputX, float inputY, float speedMultiplier)
        {
            if (isInteracting)
            {
                // Si está interactuando, no hacer nada
                CurrentMoveSpeed = 0f;
                return;
            }

            var moveDirection = (playerCamera.right * inputX + playerCamera.forward * inputY).normalized;
           
            // Velocidad de movimiento según la acción
            var moveSpeed = IsCrouching ? crouchSpeed : runSpeed;  
            moveSpeed *= speedMultiplier;


            JumpAndGravity();
            controller.Move(moveDirection * (moveSpeed * Time.deltaTime) + Vector3.up * verticalVelocity * Time.deltaTime);

            CurrentMoveSpeed = moveSpeed * moveDirection.magnitude;

            RotateAvatarTowardsMoveDirection(moveDirection);

        }


        private void RotateAvatarTowardsMoveDirection(Vector3 moveDirection)
        {
            if (moveDirection.magnitude == 0) return;

            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + transform.rotation.y;
            float angle = Mathf.SmoothDampAngle(avatar.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, TURN_SMOOTH_TIME);
            avatar.transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        // Acción de salto
        public bool TryJump()
        {
            if (IsGrounded() && !IsCrouching && !isInteracting)
            {
                jumpTrigger = true;
                return true;
            }

            jumpTrigger = false;
            return false;
        }

        private void JumpAndGravity()
        {
            if (IsGrounded() && verticalVelocity < 0)
            {
                verticalVelocity = -6f;
            }

            if (jumpTrigger && IsGrounded())
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpTrigger = false;
            }

            verticalVelocity += gravity * Time.deltaTime;
        }


        // Acción de agacharse
        public bool ToggleCrouching()
        {
            IsCrouching = !IsCrouching;
            return IsCrouching;
        }
    

        // Acciones de ataque
        public bool TryInteract()
        {
            if (IsGrounded() && !isInteracting)
            {
                isInteracting = true;
                return true;
            }

            return false;
        }

        public void EndInteract()
        {
            // Se llama desde el Animation SwordAttack con el AnimationEventReceiverPlayer.cs
            isInteracting = false; 
        }


        // Esta en el suelo
        public bool IsGrounded()
        {
            return groundCheck.IsGrounded();
        }

        // Esta cayendo
        public bool IsFreeFalling()
        {
            return !controller.isGrounded && verticalVelocity < 0;
        }


        //Calcula la dirección del joystick
        public float MapJoystickDirectionToDegrees(Vector2 direction)
        {
            if (direction.magnitude == 0)
            {
                CurrentJoystickDirection = 0f; // Sin dirección, 0 grados
                return CurrentJoystickDirection;
            }

            // Calcula el ángulo en grados y ajusta para que 0° esté hacia arriba
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg; // Nota: x e y están invertidos
            if (angle < 0) angle += 360f; // Normaliza el ángulo para estar entre 0° y 360°

            CurrentJoystickDirection = angle;
            return CurrentJoystickDirection;
        }



    }
}
