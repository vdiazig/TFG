using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

using Entities.Types;
using InterfaceAdapters.Managers;


namespace InterfaceAdapters.Presentation.HUD
{
    public class HUDController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {

        [Header("Joystick")]
        public RectTransform joystickControl; 
        public RectTransform joystickLimits; 
        private Vector2 joystickInitialPosition; 
        private float joystickRadius; 
        private Vector2 joystickDirection;


        [Header("Buttons")]
        [SerializeField] private float touchLarge = 0.3f;
        [SerializeField] private Button controlXButton; // Atacar/Interactuar/Seleccionar arma
        [SerializeField] private Button controlTButton; // Saltar/agacharse
        [SerializeField] private Button controlSButton; // Sin acciones
        [SerializeField] private Button controlMenu; // Menú

        [Header("Energy Bars")]
        [SerializeField] private Scrollbar lifeBar;
        [SerializeField] private Scrollbar energyBar;
        [SerializeField] private Scrollbar otherBar;

        [Header("Menus")]
        [SerializeField] private GameObject menuAttack;

        [Header("Contenedores")]
        [SerializeField] private GameObject contentSelectAttack;
            public GameObject ContentSelectAttack => contentSelectAttack;
        
        [SerializeField] private GameObject menuGamePause;
            public GameObject MenuGamePause => menuGamePause;



        // Controladores de interacción
        private Coroutine buttonHoldCoroutine = null; 
        private float buttonHoldTime = 0f; // Controla el tiempo de presión del botón
        private HUDActionType currentButton; // Identifica el tipo de botón presionado

        // Eventos generales para botones
        public System.Action OnTrianglePressed;
        public System.Action OnTriangleHeld;
        public System.Action OnCrossPressed;
        public System.Action OnCrossHeld;
        public System.Action OnSquarePressed;
        public System.Action OnSquareHeld;
        

        void Start()
        {
            // Guardamos la posición inicial del joystick
            joystickInitialPosition = joystickControl.anchoredPosition;

            // Calculamos el radio del contenedor (mitad del tamaño menor del fondo)
            joystickRadius = joystickLimits.sizeDelta.x / 2;

            menuAttack.SetActive(false);

        }

        // --- BUTTONS ---

        public void StartTriangleHoldRoutine()
        {
            StartButtonHoldRoutine(HUDActionType.Triangle,  OnTriangleHeld);
        }

        public void StartCrossHoldRoutine()
        { 
            
            StartButtonHoldRoutine(HUDActionType.Cross, () =>
            {
                menuAttack.SetActive(true);
                //OnCrossHeld?.Invoke(); 
            });
        }

        public void StartSquareHoldRoutine()
        {
            StartButtonHoldRoutine(HUDActionType.Square, OnSquareHeld);
        }


        public void StartButtonHoldRoutine(HUDActionType buttonType, System.Action onHeld)
        {
            if (buttonHoldCoroutine != null)
            {
                StopCoroutine(buttonHoldCoroutine);
            }

            currentButton = buttonType; // Almacena el botón presionado
            buttonHoldTime = 0f; // Reinicia el tiempo de presión
            buttonHoldCoroutine = StartCoroutine(ButtonHoldRoutine(onHeld));
            
        }


        public void StopButtonHoldRoutine()
        {
            if (buttonHoldCoroutine != null)
            {
                StopCoroutine(buttonHoldCoroutine);
                buttonHoldCoroutine = null;

                if (buttonHoldTime < touchLarge)
                {
                    //Debug.Log("Toque corto detectado.");
                    switch (currentButton)
                    {
                        case HUDActionType.Triangle:
                            OnTrianglePressed?.Invoke();
                            break;
                        case HUDActionType.Cross:
                            OnCrossPressed?.Invoke();
                            break;
                        case HUDActionType.Square:
                            OnSquarePressed?.Invoke();
                            break;
                    }
                }
            }
        }


        private IEnumerator ButtonHoldRoutine(System.Action onHeld)
        {
            while (buttonHoldTime < touchLarge) // Espera 1 segundo
            {
                buttonHoldTime += Time.deltaTime;
                yield return null;
            }

            onHeld?.Invoke(); // Invoca la acción prolongada
            buttonHoldCoroutine = null;
        }


        public void CloseMenuAttack()
        {
            menuAttack.SetActive(false);

        }

        // Volver a la base desde el menu pausa
        public void ReturnToBase()
        {
            ManagerScenes.Instance.LoadScene("Base", false);
        }


    // --- JOYSTICK ---
        public void ResetJoystick()
        {
            // Vuelve a colocar el joystick en su posición inicial
            joystickControl.anchoredPosition = joystickInitialPosition;

            // Resetea la dirección del joystick
            joystickDirection = Vector2.zero;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Acciones específicas al tocar el joystick
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Convertimos la posición del mouse o dedo al espacio local del contenedor
            Vector2 inputPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickLimits,
                eventData.position,
                eventData.pressEventCamera,
                out inputPosition
            );

            // Limitamos el movimiento al radio del contenedor
            Vector2 clampedPosition = Vector2.ClampMagnitude(inputPosition, joystickRadius);

            // Actualizamos la posición del joystick
            joystickControl.anchoredPosition = clampedPosition;

            // Calculamos la dirección del joystick
            joystickDirection = clampedPosition / joystickRadius;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Al soltar el joystick, vuelve a la posición inicial
            joystickControl.anchoredPosition = joystickInitialPosition;

            // Resetear la dirección del joystick
            joystickDirection = Vector2.zero;
        }

        // Dirección de joystick
        public Vector2 GetJoystickDirection()
        {
            //Debug.Log($"Joystick Direction: {joystickDirection}");
            return joystickDirection;
        }

        // Intensidad del joystick
        public float GetJoystickVelocity()
        {
            float magnitude = joystickDirection.magnitude; 
            return Mathf.Clamp01(magnitude);
        }





        // --- ENERGY BARS ---
        // Actualizar barra de vida
        public void UpdateLifeBar(float currentHealth, float maxHealth)
        {
            lifeBar.size = currentHealth / maxHealth;
        }

        // Actualizar barra de energía
        public void UpdateEnergyBar(float currentEnergy, float maxEnergy)
        {
            energyBar.size = currentEnergy / maxEnergy;
        }

        // Actualizar barra temporal
        public void UpdateOtherBar(float currentOtherValue, float maxOtherValue)
        {
            otherBar.size = currentOtherValue / maxOtherValue;
        }

    }
}
