using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    private void Awake()
    {
        // Verifica si ya existe otra instancia de este objeto
        if (FindObjectsOfType<PersistentManager>().Length > 1)
        {
            Destroy(gameObject); // Si existe otra instancia, destruye esta
            return;
        }

        DontDestroyOnLoad(gameObject); // Hace que este objeto persista entre escenas
    }
}
