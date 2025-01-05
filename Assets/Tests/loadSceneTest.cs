using UnityEngine;
using InterfaceAdapters.Managers;

public class loadSceneTest : MonoBehaviour
{

    public AudioSource audioSource;

    private ManagerUser managerUser;

    [Header("Damage Settings")]
    [SerializeField] private float damageAmount = 10f; // Cantidad de daño por golpe

    void Start()
    {
        // Busca la instancia de ManagerUser en la escena
        managerUser = FindObjectOfType<ManagerUser>();

        if (managerUser == null)
        {
            Debug.LogError("ManagerUser not found in the scene. Please ensure it is present.");
        }
    }

    public void loadTest()
    {
        ManagerScenes.Instance.LoadScene("Area1-02", true, () => 
        {
            Debug.Log("TEST: scene test3 load.");
        });
    }

    public void Sound()
    {
        audioSource.Play();
    }

    public void Damage()
    {

        managerUser.Life(damageAmount);
        Debug.Log($"Inflicted {damageAmount} damage to the player.");
    
    }



}
