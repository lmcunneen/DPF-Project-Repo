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
    bool breakState;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (!Input.GetKey(KeyCode.Mouse0)) //Checks to see if player is not grappling, allowing them to freely reverse
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

        else
        {
            if (breakState == true) //If they were breaking before they grappled, continue in that directions
            {
                rigidBody.AddForce(-transform.up * reverseSpeed * Time.fixedDeltaTime);
                breakState = true;
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
