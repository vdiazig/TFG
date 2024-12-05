// Script tests
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelTest : MonoBehaviour
{
    private INotification _notification; 

    public Sprite elSprite;
    public Sprite machineSprite;
    public string elName;

    void Start(){
        _notification = NotificationManager.Instance;
    }

    public void TestLefNot(){
        _notification.NotificationLeft(elSprite, elName);

    }

    public void TestScreenNot(){
        _notification.NotificationScreen("Title", machineSprite, "Body _notification", closeNotifications);

    }

    public void TestUpNot(){
        _notification.NotificationUp("System _notification", NotificationType.Success); 

    }

    public void TestLoadInit()
    {
        LoadGameScene();
    }

    private void closeNotifications()
    {
        _notification.NotificationClean();
    }

   private void LoadGameScene()
    {
        _notification.NotificationClean();
        ManagerScenes.Instance.LoadScene("Init", false, () => 
        {
            Debug.Log("Game scene Init loaded.");
        });
    }

    public void aaa()
    {
        Debug.Log("aaaaaaaaa");
    }

  

}
