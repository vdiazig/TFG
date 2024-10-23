using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is a recyclable item
        RecyclableItem item = other.gameObject.GetComponent<RecyclableItem>();
        
        if (item != null)
        {
            // Call the collect method to pick up the item
            item.Collect();
        }
    }
}
