// NotificationElement.cs (sin cambios, solo asegurarse de que está configurado con el Initialize y AutoDestroy)
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelTest : MonoBehaviour
{
    private INotification notification; 

    public Sprite elSprite;
    public string elName;

    void Start(){
        notification = NotificationManager.Instance;
    }
    public void newElement(){
        //notification.NotificationLeft(elSprite, elName);
        notification.NotificationScreen("titulo", elSprite, "CUERPO DE LA NOTIFICAION", aaa);

    }

    public void aaa()
    {
        Debug.Log("aaaaaaaaa");
    }

}
