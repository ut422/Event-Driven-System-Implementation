using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleporterManager : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private int requiredGoldCoins = 10;
    [SerializeField] private int requiredSilverKeys = 10;

    [Header("Teleporter Settings")]
    [SerializeField] private GameObject teleporterObject;
    [SerializeField] private string finishSceneName = "FinishScene";
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float hoverSpeed = 0.5f;
    [SerializeField] private float hoverHeight = 0.3f;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem teleporterParticles;

    [Header("UI Feedback")]
    [SerializeField] private UIManager uiManager;

    //references
    private InventoryManager inventoryManager;
    private Vector3 startPosition;
    private bool teleporterActivated = false;

    void Start()
    {
        //get inventory manager reference
        inventoryManager = InventoryManager.Instance;

        //listen for inventory updates
        inventoryManager.OnInventoryUpdated.AddListener(CheckTeleporterRequirements);

        //hide teleporter at start
        if (teleporterObject != null)
        {
            startPosition = teleporterObject.transform.position;
            teleporterObject.SetActive(false);
        }

        //particle effects (couldn't be bothered)
        if (teleporterParticles != null)
        {
            teleporterParticles.Stop();
        }
    }

    void CheckTeleporterRequirements()
    {
        //skip if teleporter is already activated
        if (teleporterActivated)
            return;

        //get player’s gold coins and silver keys
        int goldCoins = inventoryManager.itemCounts.ContainsKey("Gold Coin") ?
            inventoryManager.itemCounts["Gold Coin"] : 0;

        int silverKeys = inventoryManager.itemCounts.ContainsKey("Silver Key") ?
            inventoryManager.itemCounts["Silver Key"] : 0;

        //if player has enough items, activate teleporter
        if (goldCoins >= requiredGoldCoins && silverKeys >= requiredSilverKeys)
        {
            ActivateTeleporter();
        }
    }

    void ActivateTeleporter()
    {
        teleporterActivated = true;

        //show teleporter object if it exists
        if (teleporterObject != null)
        {
            teleporterObject.SetActive(true);

            //start visual effects if they exist
            if (teleporterParticles != null)
            {
                teleporterParticles.Play();
            }
        }

        //show ui feedback if uiManager exists
        if (uiManager != null)
        {
            uiManager.ShowPopup("Teleporter Activated! Find it to finish the level!");
        }

        Debug.Log("Teleporter activated! Player can now finish the level.");
    }

    void Update()
    {
        //animate teleporter if activated
        if (teleporterActivated && teleporterObject != null)
        {
            //rotate teleporter
            teleporterObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            //make teleporter hover up and down
            float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
            teleporterObject.transform.position = new Vector3(
                teleporterObject.transform.position.x,
                newY,
                teleporterObject.transform.position.z
            );
        }
    }
}