using UnityEngine;
using InterfaceAdapters.Managers;

public class loadSceneTest : MonoBehaviour
{

    public AudioSource audioSource;
    public void loadTest()
    {
        ManagerScenes.Instance.LoadScene("Area1-01", true, () => 
        {
            Debug.Log("TEST: scene test3 load.");
        });
    }

    public void Sound()
    {
        audioSource.Play();
    }

}
