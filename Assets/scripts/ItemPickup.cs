using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName; //defined in unity inspector (e.g., "Gold Coin", "Silver Key")

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //check if the player touches it
        {
            InventoryManager.Instance.AddItem(itemName);
            FindObjectOfType<UIManager>().ShowPopup(itemName);
            Destroy(gameObject); //remove item once collected
        }
    }
}
