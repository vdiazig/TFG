using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameText; 
    [SerializeField] private TMP_Text itemQuantityText; 
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemTypeText; 
    public void Setup(string type, string name, int quantity, Sprite icon)
    {
        itemTypeText.text = type;
        itemNameText.text = name;
        itemQuantityText.text = $"x{quantity}";
        itemIcon.sprite = icon;
    }
}

