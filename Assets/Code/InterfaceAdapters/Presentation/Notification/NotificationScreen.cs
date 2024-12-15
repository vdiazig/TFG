using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

namespace InterfaceAdapters.Presentation.Notification
{
    public class NotificationScreen : MonoBehaviour
    {

        [SerializeField] private  TMP_Text _titleText;
        [SerializeField] private TMP_Text _bodyText;
        [SerializeField] private Image _imageItem;
        [SerializeField] private Action _onButtonClick;

        public void Initialize(string title, Sprite image, string body, Action onButtonClickAction)
        {
            _titleText.text = title;
            if (image != null)
            {
                _imageItem.sprite = image;
            }
            else
            {
                _imageItem.gameObject.SetActive(false);
            }
            
            _bodyText.text = body;
            _onButtonClick = onButtonClickAction;

        }

        // Ejecuta la acción que se le indica
        public void OnButtonClick()
        {
        _onButtonClick?.Invoke(); // Ejecuta la acción (si no es null)
            DestroyElement(); 
        }

        // Llamada para destruir el elemento
        private void DestroyElement()
        {
            Destroy(gameObject); 
        }
    }
}