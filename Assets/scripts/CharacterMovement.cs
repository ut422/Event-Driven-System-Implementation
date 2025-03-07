using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //movement variables
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float strafeSpeed = 7f;

    //vector3 to store calculations
    private Vector3 movement;

    //rigidbody 2d
    private Rigidbody rb;

    //jump stuff
    [Header("jump tbh")]
    public float jumpHeight = 7f;
    public bool isGrounded;
    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        //initialize rb
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //use a separate function for isGrounded check
        isGrounded = CheckGround();

        //if player is on the ground jumping is allowed
        if (isGrounded)
        {
            Jump();
        }
    }

    //fixedupdate() gets called once per frame at a fixed interval
    private void FixedUpdate()
    {
        //create temp float variables to hold inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        /*now we can use calculator variable
         * set vector3 called movement to whatever transform.forward is (x axis) then
         * multiply it by y. getaxis can be -1, 0, or 1 then multiply that by movespeed
         * samething for horizontal but where multiplying by transform.right (x axis) and strafespeed
         * 
         * before we do anything we eed to nromalize it (scale it to a number between 0 and 1
         */
        Vector3.Normalize(movement);

        movement = (transform.forward * v * moveSpeed) + (transform.right * h * strafeSpeed);

        rb.MovePosition(transform.position + movement * Time.deltaTime);
    }

    void Jump()
    {
        //if jump() was called then player must be on ground
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //add force to rigidbody
            //this is adding a dorce in up direction (0, 1, 0) * jumpHeight
            //forcemode.impulse means "frame its called" aka right away
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }
    //function is of type bool
    bool CheckGround()
    {
        //special type of variable that stores collision info of raycast
        RaycastHit hit;

        //raycast is a ine of collision
        /*
         * parameters are (starting point of ray, ray direction, where to store results, length, layer to check
         * 
         * 
         */
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, groundLayer))
        {
            //if theres a collision return true
            return true;
        }

        //otherwise return false
        return false;
    }
}