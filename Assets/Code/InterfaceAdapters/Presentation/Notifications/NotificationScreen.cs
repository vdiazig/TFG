using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class NotificationScreen : MonoBehaviour
{

    [SerializeField] private  TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private Image imageItem;
    [SerializeField] private Action onButtonClick;

    public void Initialize(string title, Sprite image, string body, Action onButtonClickAction)
    {
        titleText.text = title;
        imageItem.sprite = image;
        bodyText.text = body;
        onButtonClick = onButtonClickAction;

    }

    // Ejecuta la acción que se le indica
    public void OnButtonClick()
    {
        onButtonClick?.Invoke(); // Ejecuta la acción (si no es null)
        DestroyElement(); 
    }

    // Llamada para destruir el elemento
    private void DestroyElement()
    {
        Destroy(gameObject); 
    }
}