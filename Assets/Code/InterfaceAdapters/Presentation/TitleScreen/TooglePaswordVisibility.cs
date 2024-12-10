using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InterfaceAdapters.Presentation.TitleSceen
{
    public class TogglePasswordVisibility : MonoBehaviour
    {
        private bool isPasswordVisible = false; 

        public TMP_InputField passwordInputField; 
        public Image visible;
        public Image nonVisible;


        public void TogglePassword()
        {
            // Alterna el estado de visibilidad
            isPasswordVisible = !isPasswordVisible;

            // Cambia el tipo de contenido según el estado de visibilidad
            if (isPasswordVisible)
            {
                passwordInputField.contentType = TMP_InputField.ContentType.Standard;
                visible.gameObject.SetActive(true);
                nonVisible.gameObject.SetActive(false);
            }
            else
            {
                passwordInputField.contentType = TMP_InputField.ContentType.Password;
                visible.gameObject.SetActive(false);
                nonVisible.gameObject.SetActive(true);
            }

            // Refresca el campo de texto para que el cambio surta efecto
            passwordInputField.ForceLabelUpdate();
        }
    }
}