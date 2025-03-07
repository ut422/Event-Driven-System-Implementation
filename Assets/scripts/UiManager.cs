using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //ui elements for item counters
    public Text goldCoinText;
    public Text silverKeyText;
    public Text popupText; //shows collected item
    public float popupDuration = 2f; //time before hiding popup

    void Start()
    {
        //listen for inventory updates
        InventoryManager.Instance.OnInventoryUpdated.AddListener(UpdateInventoryUI);
        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        //get item counts safely (if item exists, show count; else show 0)
        int goldCoinCount = InventoryManager.Instance.itemCounts.ContainsKey("Gold Coin") ? InventoryManager.Instance.itemCounts["Gold Coin"] : 0;
        int silverKeyCount = InventoryManager.Instance.itemCounts.ContainsKey("Silver Key") ? InventoryManager.Instance.itemCounts["Silver Key"] : 0;

        //update ui text
        goldCoinText.text = "Gold Coins: " + goldCoinCount;
        silverKeyText.text = "Silver Keys: " + silverKeyCount;
    }

    public void ShowPopup(string itemName)
    {
        popupText.text = "" + itemName;
        popupText.gameObject.SetActive(true);
        StartCoroutine(HidePopup());
    }

    System.Collections.IEnumerator HidePopup()
    {
        yield return new WaitForSeconds(popupDuration);
        popupText.gameObject.SetActive(false);
    }
}
