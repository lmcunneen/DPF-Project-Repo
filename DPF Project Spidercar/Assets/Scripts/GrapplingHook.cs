using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    /* SCRIPT FUNCTION:
     * Handles most logic used for the grappling hook, and this is all in one script because...
     *  1. The forces and rotations can be exerted on the player car directly
     *  2. Many of these variables are used in multiple areas of functionality within this script
     * However, other functionality is consolidated like finding relative position of grapple point (CheckIntersection) and breaking the grappling hook around corners (BreakGrapple)
     * 
     * This code is difficult to read and maintain, so I will be cleaning up the formatting and such when I get the chance
     */

    public GameObject grapplePointObject; //The game object that defines where the grapple hook is
    public GameObject gameManager; //The game object that holds scripts that run many external systems
    public SpringJoint2D springJoint; //The component that joins together the car and grapple point by a 'rope' essentially
    private Rigidbody2D rb; //The rigidbody component that calculates physics such as drag, mass and gravity
    public LineRenderer lineRenderer; //The component that draws the rope between the grapple and the car
    private Vector3[] lineRendererPoints; //Array of the points the line renderer conforms to

    float piFloat; //Used for circumference calculations. 

    bool doCalc;
    bool grappleSuccess;
    public bool grappleState;

    bool isAbove;
    float turnMultiplier; //The float used to make the rotationAngle positive or negative, making it turn correctly for left and right
    Vector2 velocity; //Finding velocity for turnMultiplier checks (forwards and backwards changes rotation)
    Vector3 localVelocity;
    bool grappleColliderTopCheck;
    bool grappleColliderBottomCheck;

    public GameObject topCollider;
    public GameObject bottomCollider;
    public GameObject positionChecker;

    void Awake()
    {
        piFloat = 3.141592f;

        springJoint = GetComponent<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();

        doCalc = true;

        grapplePointObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0)) //While the mouse is held down, do the following physics calculations
        {
            if (doCalc == true)
            {
                grappleState = gameManager.GetComponent<CheckAndBreakGrapple>().CheckGrappleFunc();

                if (grappleState == true)
                {
                    grappleSuccess = true;

                    grapplePointObject.GetComponent<SpriteRenderer>().enabled = true;

                    var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Finds position of the mouse on the screen and defines a 'transform' variable
                    mouseWorldPos.z = 0;

                    positionChecker.transform.localPosition = mouseWorldPos;

                    grappleColliderTopCheck = topCollider.GetComponent<CheckIntersection>().IsObjectIntersecting();

                    if (grappleColliderTopCheck == false)
                    {
                        grappleColliderBottomCheck = bottomCollider.GetComponent<CheckIntersection>().IsObjectIntersecting();
                    }

                    else
                    {
                        grappleColliderBottomCheck = false;
                    }

                    Debug.Log("Top state: " + grappleColliderTopCheck);
                    Debug.Log("Bottom state: " + grappleColliderBottomCheck);

                    velocity = rb.velocity;
                    localVelocity = transform.InverseTransformDirection(velocity);

                    FindTurnMultiplier(); //!!!!!Please fix later!!!!!

                    

                    //Debug.Log("Turn calculation is done!");
                }
            }

            doCalc = false;

            if (grappleSuccess == true)
            {
                TurnCalculations();
            }
        }

        else //When LMB is let go, do the following...
        {
            EndGrapple();
        }
    }

    // Update is called once per frame
    void Update()
    {
        grapplePointObject.transform.rotation = transform.rotation;

        if (Input.GetKeyDown(KeyCode.Mouse0)) //When the mouse is pressed down (activating once), find the grapple point
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Finds position of the mouse on the screen and defines a 'transform' variable
            mouseWorldPos.z = 0; //Makes the point conform to the 2D plane
            grapplePointObject.transform.position = mouseWorldPos; //Moves the grapple point to where the mouse was clicked
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            doCalc = true;
        }
    }

    void FindTurnMultiplier()
    {
        if (grappleColliderTopCheck == true)
        {
            isAbove = true;
        }

        else if (grappleColliderBottomCheck == true)
        {
            isAbove = false;
        }

        else if (grappleColliderTopCheck == true && grappleColliderBottomCheck == true)
        {
            Debug.LogWarning("Both checks are true for some reason!!!!!!!!");
        }

        if (isAbove == true)
        {
            turnMultiplier = 1;
        }

        if (isAbove == false)
        {
            turnMultiplier = -1;
        }

        if (grappleColliderTopCheck == false && grappleColliderBottomCheck == false)
        {
            turnMultiplier = 0;
        }
    }

    void TurnCalculations()
    {
        //Finds rotation angle for the RotateAround function with ***MATHS***
        float distanceRadius = springJoint.distance; //Finds grapple distance by reading the distance variable on the spring joint
        float vehicleVelocity = rb.velocity.magnitude; //Finds current vehicle velocity
                                                       //Now the calculations are made
        float grappleCircumference = 2 * piFloat * distanceRadius; //Finds circumference of turning circle (distance)
        float fullRotationTime = grappleCircumference / vehicleVelocity; //Finds the time it would take to finish the circle. Unsure of what measurement of time it would refer to...
        float segmentTimePerUpdate = fullRotationTime * Time.fixedDeltaTime; //This doesn't work, as it rounds down the number to small, making the result to miniscule. Return to this and fix it
        float rotationAngle = (360 / (segmentTimePerUpdate * 50)) * turnMultiplier; //Determines the angle (still needs more work to accurately find it) and filters it through the turnMultiplier

        //Debug.Log("VELOCITY: " + vehicleVelocity);
        //Debug.Log(grappleCircumference + " / " + vehicleVelocity + " = " + fullRotationTime);
        //Debug.Log(rotationAngle);

        //Enables the spring joint and line renderer
        springJoint.enabled = true;
        lineRenderer.enabled = true;
        //Handles Rotation Logic
        Vector3 rotationMask = new Vector3(0, 0, 1); //Only rotates on Z axis
        Vector3 point = grapplePointObject.transform.position; //Assigns the grapple point position to a vector 3 for rotation
        transform.RotateAround(point, rotationMask, Time.fixedDeltaTime * rotationAngle);

        lineRendererPoints = new Vector3[] { grapplePointObject.transform.position, gameObject.transform.position }; //Defines the start and end of the line
        lineRenderer.SetPositions(lineRendererPoints); //Sets the positions to the previously defined positions
    }

    void EndGrapple()
    {
        //Disables the spring joint and line renderer
        springJoint.enabled = false;
        lineRenderer.enabled = false;
        grapplePointObject.GetComponent<SpriteRenderer>().enabled = false;

        turnMultiplier = 0;
        grappleSuccess = false;
    }
}