using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    /* SCRIPT FUNCTION:
     * Handles most logic and all references used for the grappling hook. This includes (in order of operations):
     *  - Finding the /POSITION OF THE MOUSE/ in world space
     *  - Referencing CheckAndBreakGrapple to see if the grapple was successful for calculations or is intersecting with something
     *  - Referencing CheckIntersection to determine /RELATIVE POSITION OF GRAPPLE POINT/
     *  - Finding /RELATIVE MOVEMENT/ direction of car (forwards or backwards)
     *  - Determining the /TURN DIRECTION/ by factoring in relative position and movement direction (movement direction not implemented)
     *  - /CALCULATING AND APPLIES ROTATIONS/ of the car turning around the grapple
     *  - /ENDING GRAPPLE/ under specified circumstances
     */

    public GameObject grapplePointObject; //The game object that defines where the grapple hook is
    public GameObject gameManager; //The game object that holds scripts that run many external systems
    private Vector3 mouseWorldPos; //Stored mouse position

    private bool doCalc; //Allows for certain calculations in FixedUpdate to only run once

    public bool grappleState; //Referenced bool from CheckAndBreakGrapple script, to see if raycast was successful
    private bool isBroken; //Referenced bool from CheckAndBreakGrapple script, to see if grapple is deemed broken
    public bool brokenDistanceCheck; //Unused boolean that was supposed to ignore the first calculation of the broken distance, but didn't. Archived for future debugging

    public GameObject topCollider;
    public GameObject bottomCollider;
    private Vector2 velocity; //Finding velocity for turnMultiplier checks (forwards and backwards changes rotation)
    private Vector3 localVelocity; //CURRENTLY UNUSED VARIABLE THAT WILL LATER HOLD FUNCTIONALITY FOR DETERMINING RELATIVE DIRECTION, AS A WORKING METHOD HAS NOT BEEN FOUND YET!!!!!!
    private bool grappleColliderTopCheck; //Bool that returns holds state for the TOP collider's CheckIntersection calculations
    private bool grappleColliderBottomCheck;//Bool that returns holds state for the BOTTOM collider's CheckIntersection calculations
    private bool isAbove; //Bool that is used for finding the turn multiplier (found through the CheckIntersection script calculations)
    private float turnMultiplier; //The float used to make the rotationAngle positive or negative, making it turn correctly for left and right

    public SpringJoint2D springJoint; //The component that joins together the car and grapple point by a 'rope' essentially
    private Rigidbody2D rb; //The rigidbody component that calculates physics such as drag, mass and gravity
    public LineRenderer lineRenderer; //The component that draws the rope between the grapple and the car
    private Vector3[] lineRendererPoints; //Array of the points the line renderer conforms to
    private float piFloat; //Used for circumference calculations

    void Awake()
    {
        grapplePointObject.GetComponent<SpriteRenderer>().enabled = false;

        doCalc = true;
        brokenDistanceCheck = false;

        springJoint = GetComponent<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        piFloat = 3.141592f;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0)) //While the mouse is held down, do the following physics calculations
        {
            if (doCalc == true)
            {
                mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Finds position of the mouse on the screen and defines a 'transform' variable
                mouseWorldPos.z = 0; //Makes the point conform to the 2D plane
                grapplePointObject.transform.position = mouseWorldPos; //Moves the grapple point to where the mouse was clicked

                grappleState = gameManager.GetComponent<CheckAndBreakGrapple>().CheckGrappleFunc();

                if (grappleState == true)
                {
                    brokenDistanceCheck = false;

                    grapplePointObject.GetComponent<SpriteRenderer>().enabled = true;

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

            if (grappleState == true)
            {
                isBroken = gameManager.GetComponent<CheckAndBreakGrapple>().BreakGrappleFunc();

                if (isBroken == false)
                {
                    TurnCalculations();
                }

                else
                {
                    EndGrapple();
                }
            }
        }

        else //When LMB is let go, do the following...
        {
            EndGrapple();
        }
    }

    void Update()
    {
        grapplePointObject.transform.rotation = transform.rotation;

        if (Input.GetKeyDown(KeyCode.Mouse0)) //When the mouse is pressed down (activating once), find the grapple point
        {
            /*mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Finds position of the mouse on the screen and defines a 'transform' variable
            mouseWorldPos.z = 0; //Makes the point conform to the 2D plane
            grapplePointObject.transform.position = mouseWorldPos; //Moves the grapple point to where the mouse was clicked
            */
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            doCalc = true;
            brokenDistanceCheck = false;
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
        Vector3 point = grapplePointObject.transform.position; //Assigns the grapple point position to a Vector3 for rotation
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
        grappleState = false;
    }
}