
using UnityEngine;
using UnityEngine.UI;

public class RecyclableItem : MonoBehaviour
{
    // Attributes
    public int id;                  // Id del item
    public string type;              // Material type (plastic, metal, etc.)
    public string nameItem;         // Name item
    public float economicValue;      // Economic value of the recyclable item
    public Sprite sprite;            // Visual representation of the item
    public AudioClip audioClip;      // Sound to play when the item is collected
    
    private AudioSource audioSource; // Audio source for playing the sound
    private Image imageSource;

    // Initialize the audio source
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();  // Add an AudioSource component
        audioSource.clip = audioClip;                          // Assign the sound clip
         // Añadir el componente Image al mismo objeto
        imageSource = gameObject.AddComponent<Image>();
        imageSource.sprite = sprite;
    }

    // Method to collect the item
    public void Collect()
    {
        // Log the collection action (could be inventory update)
        Debug.Log("Collected item: " + type + " | Value: " + economicValue);

        // Play the collection sound
        PlaySound();

        //LLama a PlayFab

        // Destroy the item from the game scene after collecting
        Destroy(gameObject);
    }

    // Method to play the collection sound
    public void PlaySound()
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.Play();  // Play the sound
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing.");
        }
    }
}
