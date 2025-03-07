using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    //dictionary to store item names and counts
    public Dictionary<string, int> itemCounts = new Dictionary<string, int>();

    //event to update UI
    public UnityEvent OnInventoryUpdated;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(string itemName)
    {
        //if item exists, increase count
        if (itemCounts.ContainsKey(itemName))
        {
            itemCounts[itemName]++;
        }
        else
        {
            itemCounts[itemName] = 1;
        }

        Debug.Log("Collected: " + itemName + " (Total: " + itemCounts[itemName] + ")");

        //call event to update UI
        if (OnInventoryUpdated != null)
        {
            OnInventoryUpdated.Invoke();
        }
    }
}
