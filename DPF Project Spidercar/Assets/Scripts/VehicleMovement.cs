using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    /* SCRIPT FUNCTION:
     * Holds functionality for vehicle movement, including car speed and reversing
     * Also has extra functionality for stopping player input while grappling
     */

    //VARIABLE DECLARATION//
    [SerializeField] private float forwardSpeed = 500;
    [SerializeField] private float reverseSpeed = 300;
    Rigidbody2D rigidBody;
    bool breakState; //Used to determine if the player was breaking or not before grappling
    bool grappleSuccess; //Used to see if control should be suspended if grapple would be successful

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        grappleSuccess = false;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            grappleSuccess = false;
        }
    }

    void FixedUpdate()
    {
        grappleSuccess = gameObject.GetComponent<GrapplingHook>().grappleSuccess;
        
        if (grappleSuccess == false) //Checks to see if player is not grappling, allowing them to freely reverse if not
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rigidBody.AddForce(-transform.up * reverseSpeed * Time.fixedDeltaTime);
                breakState = true; //Sets boolean as true to read when grappling
            }

            else
            {
                rigidBody.AddForce(transform.up * forwardSpeed * Time.fixedDeltaTime);
                breakState = false; //Sets boolean as false to read when grappling
            }
        }

        else //If they were breaking before they grappled, continue in that directions
        {
            if (breakState == true)
            {
                rigidBody.AddForce(-transform.up * reverseSpeed * Time.fixedDeltaTime);
            }

            else if (breakState == false)
            {
                rigidBody.AddForce(transform.up * forwardSpeed * Time.fixedDeltaTime);
            }

            else
            {
                Debug.LogError("Vehicle Movement broke!");
            }
        }
    }
}
