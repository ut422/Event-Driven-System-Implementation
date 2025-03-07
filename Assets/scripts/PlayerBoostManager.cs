using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBoostManager : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private int requiredGoldCoins = 5;
    [SerializeField] private int requiredSilverKeys = 5;

    [Header("Boost Settings")]
    [SerializeField] private float speedBoostMultiplier = 1.5f;
    [SerializeField] private float jumpBoostMultiplier = 1.5f;
    [SerializeField] private float boostDuration = 10f;

    [Header("UI Feedback")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Text boostActiveText;  //reference to ui text showing boost

    //references
    private CharacterController playerController;
    private InventoryManager inventoryManager;

    //state tracking
    private bool boostsActive = false;
    private bool boostUsed = false;  //track if boost is used
    private float originalMoveSpeed;
    private float originalStrafeSpeed;
    private float originalJumpHeight;
    private Coroutine activeBoostCoroutine;

    void Start()
    {
        //get references
        playerController = GetComponent<CharacterController>();
        inventoryManager = InventoryManager.Instance;

        //store original values
        originalMoveSpeed = playerController.moveSpeed;
        originalStrafeSpeed = playerController.strafeSpeed;
        originalJumpHeight = playerController.jumpHeight;

        //listen for inventory updates
        inventoryManager.OnInventoryUpdated.AddListener(CheckBoostRequirements);

        //initialize boost active text
        if (boostActiveText != null)
        {
            boostActiveText.gameObject.SetActive(false);
        }
    }

    void CheckBoostRequirements()
    {
        //skip if boosts already active or used
        if (boostsActive || boostUsed)
            return;

        //check if player has required items
        int goldCoins = inventoryManager.itemCounts.ContainsKey("Gold Coin") ?
            inventoryManager.itemCounts["Gold Coin"] : 0;

        int silverKeys = inventoryManager.itemCounts.ContainsKey("Silver Key") ?
            inventoryManager.itemCounts["Silver Key"] : 0;

        //apply boosts if requirements are met
        if (goldCoins >= requiredGoldCoins && silverKeys >= requiredSilverKeys)
        {
            //show notification text
            if (uiManager != null)
            {
                uiManager.ShowPopup("requirements met! activating speed and jump boost!");
            }

            //slight delay before applying boost
            StartCoroutine(ActivateBoostWithDelay(0.5f));
        }
    }

    IEnumerator ActivateBoostWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ApplyBoosts();
    }

    void ApplyBoosts()
    {
        //set flags
        boostsActive = true;
        boostUsed = true;  //mark boost used

        //apply speed and jump boosts
        playerController.moveSpeed = originalMoveSpeed * speedBoostMultiplier;
        playerController.strafeSpeed = originalStrafeSpeed * speedBoostMultiplier;
        playerController.jumpHeight = originalJumpHeight * jumpBoostMultiplier;

        // show ui feedback
        if (uiManager != null)
        {
            uiManager.ShowPopup("speed + jump boost");
        }

        //show boost active text
        if (boostActiveText != null)
        {
            boostActiveText.text = "boost active: " + boostDuration.ToString("F1") + "s";
            boostActiveText.gameObject.SetActive(true);
            StartCoroutine(UpdateBoostTimeRemaining());
        }

        Debug.Log("player boosted! speed: " + playerController.moveSpeed +
                  ", strafe: " + playerController.strafeSpeed +
                  ", jump: " + playerController.jumpHeight);

        //start timer to remove boosts
        if (activeBoostCoroutine != null)
        {
            StopCoroutine(activeBoostCoroutine);
        }

        activeBoostCoroutine = StartCoroutine(RemoveBoostsAfterDelay());

        //optionally consume items (uncomment to enable)
        /*
        if (inventoryManager.itemCounts.ContainsKey("Gold Coin"))
        {
            inventoryManager.itemCounts["Gold Coin"] -= requiredGoldCoins;
        }
        if (inventoryManager.itemCounts.ContainsKey("Silver Key"))
        {
            inventoryManager.itemCounts["Silver Key"] -= requiredSilverKeys;
        }
        //trigger ui update event
        if (inventoryManager.OnInventoryUpdated != null)
        {
            inventoryManager.OnInventoryUpdated.Invoke();
        }
        */
    }

    IEnumerator UpdateBoostTimeRemaining()
    {
        float timeRemaining = boostDuration;

        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (boostActiveText != null)
            {
                boostActiveText.text = "boost active: " + timeRemaining.ToString("F1") + "s";
            }
            yield return null;
        }
    }

    IEnumerator RemoveBoostsAfterDelay()
    {
        yield return new WaitForSeconds(boostDuration);

        //reset to original values
        playerController.moveSpeed = originalMoveSpeed;
        playerController.strafeSpeed = originalStrafeSpeed;
        playerController.jumpHeight = originalJumpHeight;

        //hide boost active text
        if (boostActiveText != null)
        {
            boostActiveText.gameObject.SetActive(false);
        }

        //show ui feedback
        if (uiManager != null)
        {
            uiManager.ShowPopup("ended");
        }

        Debug.Log("boosts expired! speed and jump returned to normal.");

        //reset active flag but keep used flag true
        boostsActive = false;
    }

    //optional: public method to reset one-time use (for testing or new game)
    public void ResetBoostUsage()
    {
        boostUsed = false;
        ResetBoosts();
    }

    //optional: public method to force reset boosts (e.g., on player death)
    public void ResetBoosts()
    {
        if (boostsActive)
        {
            playerController.moveSpeed = originalMoveSpeed;
            playerController.strafeSpeed = originalStrafeSpeed;
            playerController.jumpHeight = originalJumpHeight;

            if (activeBoostCoroutine != null)
            {
                StopCoroutine(activeBoostCoroutine);
            }

            if (boostActiveText != null)
            {
                boostActiveText.gameObject.SetActive(false);
            }

            boostsActive = false;
        }
    }
}
