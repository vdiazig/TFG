
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDAttack : MonoBehaviour
{   
    [SerializeField] private int attackID;
    [SerializeField] private Image iconAttack;
    [SerializeField] private GameObject prefabAttack;
    [SerializeField] private ManagerUser managerUser;
    [SerializeField] private HUDController hudController;
    [SerializeField] private Toggle toggle;


    public void Setup(int ID, Sprite icon, GameObject prefab, ToggleGroup  toggleGroup, ManagerUser manager, HUDController HUD)
    {
        attackID = ID;
        iconAttack.sprite = icon;
        prefabAttack = prefab;
        managerUser = manager;
        hudController = HUD;
        toggle = this.GetComponent<Toggle>();
        toggle.group = toggleGroup;
        
    }


    public void SelectAttack()
    {
        managerUser.updateSelectAttack(prefabAttack);
        hudController.CloseMenuAttack();
    }

}