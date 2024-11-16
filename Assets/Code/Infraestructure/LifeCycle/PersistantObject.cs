using UnityEngine;

public class PersistantObject : MonoBehaviour
{
    private void Awake()
    {
        // Verifica si ya existe otra instancia de este objeto
        if (FindObjectsOfType<PersistantObject>().Length > 1)
        {
            Destroy(gameObject); // Si existe otra instancia, destruye esta
            return;
        }

        DontDestroyOnLoad(gameObject); // Hace que este objeto persista entre escenas
    }
}
