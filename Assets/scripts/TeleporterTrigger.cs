using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleporterTrigger : MonoBehaviour
{
    [SerializeField] private string finishSceneName = "FinishScene";
    [SerializeField] private float teleportDelay = 2.0f;
    [SerializeField] private UIManager uiManager;

    private bool playerInTrigger = false;
    private bool teleporting = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !teleporting)
        {
            playerInTrigger = true;
            StartCoroutine(TeleportPlayer());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    IEnumerator TeleportPlayer()
    {
        teleporting = true;

        //show ui message
        if (uiManager != null)
        {
            uiManager.ShowPopup("teleporting to finish in " + teleportDelay + " seconds...");
        }

        //wait for teleport delay
        float countdown = teleportDelay;
        while (countdown > 0 && playerInTrigger)
        {
            countdown -= Time.deltaTime;
            yield return null;
        }

        //teleport if player still in trigger zone
        if (playerInTrigger)
        {
            //show complete message
            if (uiManager != null)
            {
                uiManager.ShowPopup("level complete!");
            }

            //load finish scene
            SceneManager.LoadScene(finishSceneName);
        }
        else
        {
            //reset teleporting if player left trigger
            teleporting = false;

            if (uiManager != null)
            {
                uiManager.ShowPopup("teleportation canceled!");
            }
        }
    }
}
