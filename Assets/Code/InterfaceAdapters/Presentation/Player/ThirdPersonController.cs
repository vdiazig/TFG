using UnityEditor.Timeline.Actions;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(ThirdPersonMovement))]
    public class ThirdPersonController : MonoBehaviour
    {
        // Animator hashs
        private static readonly int JoystickDirectionHash = Animator.StringToHash("JoystickDirection");
        private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private static readonly int FreeFallHash = Animator.StringToHash("FreeFall");
        private static readonly int JumpTriggerHash = Animator.StringToHash("JumpTrigger");
        private static readonly int IsCrouchingHash = Animator.StringToHash("IsCrouching");
        private static readonly int DodgeTriggerHash = Animator.StringToHash("DodgeTrigger");
        private static readonly int LifeValueHash = Animator.StringToHash("LifeValue");
        private static readonly int InteractionTypeHash = Animator.StringToHash("InteractionType"); // Animations call to EndInteract
        private static readonly int AttackTypeHash = Animator.StringToHash("AttackType");
        private static readonly int DamageTriggerHash = Animator.StringToHash("DamageTrigger"); // Animation call to EndInteract


        private ThirdPersonMovement thirdPersonMovement;
        private HUDController hudController;
        public event System.Action OnLoadComplete;

       
        [Header("Avatar, Animator")]
        [SerializeField][Tooltip("Avatar to load (assign in the inspector)")]
        private GameObject avatar;
        private readonly Vector3 avatarPositionOffset = new Vector3(0, -0.08f, 0);
        private Animator animator;


        [Header("Interaction Type")]
        [SerializeField] private InteractionType interaction;
        public InteractionType Interaction
        {
            get => interaction;
            set => interaction = value;
        }
        [SerializeField] private AttackPlayerType activeAttack;
        public AttackPlayerType ActiveAttack
        {
            get => activeAttack;
            set => activeAttack = value;
        }
        [SerializeField] private GameObject RightHandWeapon;
        [SerializeField] private GameObject LeftHandWeapon;





        private void Awake()
        {
            thirdPersonMovement = GetComponent<ThirdPersonMovement>();
        }

        private void Start()
        {
            SetupAvatar();
            SetupHUDController();
            ClearInteractionType();
        }

        private void SetupAvatar()
        {
            if (avatar == null)
            {
                Debug.LogError("Avatar not assigned in the inspector.");
                return;
            }

            // Re-parent and reset transforms
            avatar.transform.parent = transform;
            avatar.transform.localPosition = avatarPositionOffset;
            avatar.transform.localRotation = Quaternion.identity;

            // Setup animator
            thirdPersonMovement.Setup(avatar);
            animator = avatar.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator not found on the avatar.");
                return;
            }
            animator.applyRootMotion = false;

            OnLoadComplete?.Invoke();
        }

        private void SetupHUDController()
        {
            hudController = FindObjectOfType<HUDController>();
            if (hudController == null)
            {
                Debug.LogError("HUDController not found in the scene.");
                return;
            }

            // Register HUD events
            hudController.OnTrianglePressed += OnTriangle;
            hudController.OnTriangleHeld += OnTriangleHeld;

            hudController.OnCrossPressed += OnCross;
            //hudController.OnCrossHeld += OnCrossHeld;

            hudController.OnSquarePressed += OnSquare;
            //hudController.OnSquareHeld += OnSquareHeld;   
        }




        private void Update()
        {
            var joystickInput = hudController.GetJoystickDirection();
            float speedMultiplier = hudController.GetJoystickVelocity();


            // Move character
            thirdPersonMovement.Move(joystickInput.x, joystickInput.y, speedMultiplier);


            // Update Animator MoveSpeed and JoystickDirection
            animator.SetFloat(MoveSpeedHash, thirdPersonMovement.CurrentMoveSpeed);
            //animator.SetFloat(JoystickDirectionHash, thirdPersonMovement.MapJoystickDirectionToDegrees(joystickInput));

            UpdateAnimator();

        }

        private void UpdateAnimator()
        {
            var isGrounded = thirdPersonMovement.IsGrounded();
            animator.SetBool(IsGroundedHash, isGrounded);

            var isCrouching = thirdPersonMovement.IsCrouching;
            animator.SetBool(IsCrouchingHash, isCrouching);

            var isFreeFalling = thirdPersonMovement.IsFreeFalling();
            animator.SetBool(FreeFallHash, isFreeFalling);

        }


        // BUTTON ACTIONS
        private void OnTriangle()
        {
            Debug.Log("Triangle short: jump");
            if (thirdPersonMovement.TryJump())
            {
                animator.SetTrigger(JumpTriggerHash);
            }
        }

        private void OnTriangleHeld()
        {
            Debug.Log("Triangle held: crouch");
            if (thirdPersonMovement.IsGrounded())
            {
                animator.SetBool(IsCrouchingHash, thirdPersonMovement.ToggleCrouching());
            }
        }

        private void OnCross()
        {
            Debug.Log("Cross pressed: attack/interact");
            if (thirdPersonMovement.TryInteract())
            {
                switch (interaction)
                {

                    case InteractionType.Attack:
                        Debug.Log("Attack");
                        
                        InteractionAttack(activeAttack);
                        
                        break;
                        
                    case InteractionType.Confused:
                        Debug.Log("Confused");
                        animator.SetInteger(InteractionTypeHash, 2);
                        break;

                    case InteractionType.Speak:
                        Debug.Log("Speak");
                        animator.SetInteger(InteractionTypeHash, 3);
                        break;

                    case InteractionType.OpenDoor:
                        Debug.Log("Open");
                        animator.SetInteger(InteractionTypeHash, 4);
                        break;

                    case InteractionType.SearchObject:
                        Debug.Log("Take");
                        animator.SetInteger(InteractionTypeHash, 5);
                        break;
                    
                    default:
                        Debug.Log("Default Confused");
                        animator.SetInteger(InteractionTypeHash, 2);
                        break;
                }
            }
            
        }

        private void InteractionAttack(AttackPlayerType attackType)
        {
           animator.SetInteger(InteractionTypeHash, 1);
            switch(attackType)
            {
                case AttackPlayerType.None:
                    Debug.Log("Attack None");
                    animator.SetInteger(AttackTypeHash, 1);
                    break;

                case AttackPlayerType.Sword:
                    Debug.Log("Attack Sword");
                    animator.SetInteger(AttackTypeHash, 2);
                    break;

                case AttackPlayerType.SPLauncher:
                    Debug.Log("Attack SPLauncher");
                    animator.SetInteger(AttackTypeHash, 3);
                    break;

                default:
                    Debug.Log("Attack None");
                    animator.SetInteger(AttackTypeHash, 1);
                    break;
            }
            
        }

         // Al finalizar las animaciones de interacción
        public void EndInteract(){
            animator.SetInteger(InteractionTypeHash, 0);
            animator.SetInteger(AttackTypeHash, 0);
        }


        /*private void OnCrossHeld()
        {
            Debug.Log("Cross held: select weapon");


        }*/

        private void OnSquare()
        {
            Debug.Log("Square pressed: Dodge.");


        }


       /* private void OnSquareHeld()
        {
            Debug.Log("Square prolonged: not function");


        }*/


        // --- INTERACCIONES Y ATAQUES ---
        // Interacción con el jugador por otros objetos
        public void ClearInteractionType()
        {
            if (activeAttack == AttackPlayerType.None)
            {
                interaction = InteractionType.None; 
            }
            else
            {
                interaction = InteractionType.Attack; 
            }
            Debug.Log("InteractionType cleared.");
        }

        // Genera un nuevo arma desde el HUD selection, se llama desde ManagerUser
        public void InstantiateWeapon(GameObject prefabWeapon, bool rightHand)
        {
            if (rightHand)
            {
                ClearContainer(RightHandWeapon);
                Instantiate(prefabWeapon, RightHandWeapon.transform);
            }
            else
            {
                ClearContainer(LeftHandWeapon);
                Instantiate(prefabWeapon, LeftHandWeapon.transform);
            }
        }

        // Método para limpiar los contenedores
        private void ClearContainer(GameObject container)
        {
            foreach (Transform child in container.transform)
            {
                Destroy(child.gameObject); // Elimina todos los hijos del contenedor
            }
        }
    
       

    }
}
