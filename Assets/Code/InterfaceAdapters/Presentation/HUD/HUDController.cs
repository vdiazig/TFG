using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class HUDController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    [Header("Joystick")]
    public RectTransform joystickControl; 
    public RectTransform joystickLimits; 
    private Vector2 joystickInitialPosition; 
    private float joystickRadius; 
    private Vector2 joystickDirection;


    [Header("Buttons")]
    [SerializeField] private Button controlXButton; // Atacar/Interactuar
    [SerializeField] private Button controlTButton; // Salto
    [SerializeField] private Button controlSButton; // Defenderse
    [SerializeField] private Button controlMenu; // Menú

    [Header("Energy Bars")]
    [SerializeField] private Scrollbar lifeBar;
    [SerializeField] private Scrollbar energyBar;
    [SerializeField] private Scrollbar otherBar;

    // Event Handlers para conectar con el personaje o sistema
    public System.Action OnJumpPressed;
    public System.Action OnInteractPressed;
    public System.Action OnDefendPressed;
    public System.Action OnMenuPressed;

    void Start()
    {
        // Guardamos la posición inicial del joystick
        joystickInitialPosition = joystickControl.anchoredPosition;

        // Calculamos el radio del contenedor (mitad del tamaño menor del fondo)
        joystickRadius = joystickLimits.sizeDelta.x / 2;

        // Asignar eventos a los botones
        controlXButton.onClick.AddListener(HandleInteractButton);
        controlTButton.onClick.AddListener(HandleJumpButton);
        controlSButton.onClick.AddListener(HandleDefendButton);
        controlMenu.onClick.AddListener(HandleMenuButton);
    }

    private void OnDestroy()
    {
        // Eliminar eventos de los botones al destruir el objeto
        controlXButton.onClick.RemoveListener(HandleInteractButton);
        controlTButton.onClick.RemoveListener(HandleJumpButton);
        controlSButton.onClick.RemoveListener(HandleDefendButton);
        controlMenu.onClick.RemoveListener(HandleMenuButton);
    }

    // --- JOYSTICK ---
    public void OnPointerDown(PointerEventData eventData)
    {
        // No es necesario implementar aquí a menos que quieras realizar acciones específicas al tocar el joystick
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

    public Vector2 GetJoystickDirection()
    {
        return joystickDirection;
    }



    // --- BUTTONS ---
    private void HandleJumpButton()
    {
        Debug.Log("Jump Button Pressed");
        OnJumpPressed?.Invoke(); // Notificar que se presionó el botón de salto
    }

    private void HandleInteractButton()
    {
        Debug.Log("Interact Button Pressed");
        OnInteractPressed?.Invoke(); // Notificar que se presionó el botón de ataque/interacción
    }

    private void HandleDefendButton()
    {
        Debug.Log("Defend Button Pressed");
        OnDefendPressed?.Invoke(); // Notificar que se presionó el botón de defensa
    }

    private void HandleMenuButton()
    {
        Debug.Log("Menu Button Pressed");
        OnMenuPressed?.Invoke(); // Notificar que se presionó el botón de menú
    }


    // --- ENERGY BARS ---
     public void UpdateLifeBar(float currentHealth, float maxHealth)
    {
        lifeBar.size = currentHealth / maxHealth;
    }

    // Actualizar barra de energía
    public void UpdateEnergyBar(float currentEnergy, float maxEnergy)
    {
        energyBar.size = currentEnergy / maxEnergy;
    }

    public void UpdateOtherBar(float currentOtherValue, float maxOtherValue)
    {
        otherBar.size = currentOtherValue / maxOtherValue;
    }

}
