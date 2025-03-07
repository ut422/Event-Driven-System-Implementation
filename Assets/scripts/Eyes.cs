using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class E : MonoBehaviour
{
    //variables for mouse movement
    public float mouseSensitivity = 5f;

    //smoothing
    public float smoothing = 1.5f;

    //2 vectors that store calculations
    private Vector2 mouseLook;
    private Vector2 smoothMovement;

    //player referece
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //reference to player needs getting
        player = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //cursor invisible line here
        Cursor.lockState = CursorLockMode.Locked;

        //mouse movement local variable
        Vector2 mouseDirection = new Vector2(Input.GetAxis("Mouse X"),
                                Input.GetAxis("Mouse Y"));

        //apply sensitivity and smoothing to mouse input
        mouseDirection.x *= mouseSensitivity * smoothing;
        mouseDirection.y *= mouseSensitivity * smoothing;

        //smoothly transition between current and target mouse positions using lerp (linear interpolation)
        smoothMovement.x = Mathf.Lerp(smoothMovement.x, mouseDirection.x, 1f / smoothing);
        smoothMovement.y = Mathf.Lerp(smoothMovement.y, mouseDirection.y, 1f / smoothing);

        mouseLook += smoothMovement;

        //restrict vertical look to the range -80 to 90 degrees
        mouseLook.y = Mathf.Clamp(mouseLook.y, -80f, 90f);

        //rotate camera based on vertical mouse movement
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);

        //rotate player based on horizontal mouse movement
        player.transform.rotation = Quaternion.AngleAxis(mouseLook.x, player.transform.up);
    }
}

//thign that fixes some issue: Spawner.onTreatureCollected();
