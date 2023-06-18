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
    public GameObject forwardCollider;
    public GameObject reverseCollider;
    private bool grappleColliderTop; //Bool that returns holds state for the TOP collider's CheckIntersection calculations
    private bool grappleColliderBottom; //Bool that returns holds state for the BOTTOM collider's CheckIntersection calculations
    private bool directionColliderForward;
    private bool directionColliderReverse;
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

        grappleColliderTop = false;
        grappleColliderBottom = false;
        isBroken = false;

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
                    grapplePointObject.GetComponent<SpriteRenderer>().enabled = true;

                    StartCoroutine(CheckTurnNextFixedUpdate());
                }
            }

            if (grappleState == true)
            {
                StartCoroutine(CheckBreakNextFixedUpdate());

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
    }

    IEnumerator CheckTurnNextFixedUpdate()
    {
        yield return new WaitForFixedUpdate();
        //Find Relative Grapple Position (above or below)
        grappleColliderTop = topCollider.GetComponent<CheckIntersection>().IsObjectIntersecting();

        if (grappleColliderTop == false)
        {
            grappleColliderBottom = bottomCollider.GetComponent<CheckIntersection>().IsObjectIntersecting();
        }

        else
        {
            grappleColliderBottom = false;
        }

        Debug.Log("Top state: " + grappleColliderTop);
        Debug.Log("Bottom state: " + grappleColliderBottom);

        //Find Relative Direction (forward or reverse)
        directionColliderForward = forwardCollider.GetComponent<CheckIntersection>().IsObjectIntersecting();

        if (directionColliderForward == false)
        {
            directionColliderReverse = reverseCollider.GetComponent<CheckIntersection>().IsObjectIntersecting();
        }

        else
        {
            directionColliderReverse = false;
        }

        Debug.Log("Forward state: " + directionColliderForward);
        Debug.Log("Reverse state: " + directionColliderReverse);

        FindTurnMultiplier();

        Debug.Log("Turn calculation is done!");

        doCalc = false;
    }

    void FindTurnMultiplier()
    {
        if (grappleColliderTop && directionColliderForward || grappleColliderBottom && directionColliderReverse)
        {
            turnMultiplier = 1;
        }

        if (grappleColliderBottom && directionColliderForward || grappleColliderTop && directionColliderReverse)
        {
            turnMultiplier = -1;
        }

        if (grappleColliderTop == false && grappleColliderBottom == false)
        {
            turnMultiplier = 0;
        }

        else if (grappleColliderTop && grappleColliderBottom || directionColliderForward && directionColliderReverse)
        {
            Debug.LogWarning("Both checks returned true!!! Debug ASAP!");
        }
    }

    IEnumerator CheckBreakNextFixedUpdate()
    {
        yield return new WaitForFixedUpdate();
        isBroken = gameManager.GetComponent<CheckAndBreakGrapple>().BreakGrappleFunc();
    }    

    void TurnCalculations()
    {
        //Finds rotation angle for the RotateAround function with ***MATHS***
        float distanceRadius = springJoint.distance; //Finds grapple distance by reading the distance variable on the spring joint
        float vehicleVelocity = rb.velocity.magnitude; //Finds current vehicle velocity
        //Now the calculations are made
        float grappleCircumference = 2 * piFloat * distanceRadius; //Finds circumference of turning circle (distance)
        float fullRotationTime = grappleCircumference / vehicleVelocity; //Finds the time it would take to finish the circle. Measures in units per second
        float segmentsPerRotation = fullRotationTime / Time.fixedDeltaTime; //Finds how many segments are travelled during whole rotation
        float rotationAngle = (360 / segmentsPerRotation) * turnMultiplier; //Determines the angle of each segment and filters it through the turnMultiplier

        //Debug.Log("VELOCITY: " + vehicleVelocity);
        //Debug.Log(grappleCircumference + " / " + vehicleVelocity + " = " + fullRotationTime);
        //Debug.Log(rotationAngle);

        //Enables the spring joint and line renderer
        springJoint.enabled = true;
        lineRenderer.enabled = true;
        //Handles Rotation Logic
        Vector3 rotationMask = new Vector3(0, 0, 1); //Only rotates on Z axis
        Vector3 point = grapplePointObject.transform.position; //Assigns the grapple point position to a Vector3 for rotation
        transform.RotateAround(point, rotationMask, rotationAngle); //RotateAround function that enacts the rotation

        lineRendererPoints = new Vector3[] { grapplePointObject.transform.position, gameObject.transform.position }; //Defines the start and end of the line
        lineRenderer.SetPositions(lineRendererPoints); //Sets the positions to the previously defined positions
    }

    void EndGrapple()
    {
        //Disables the spring joint and line renderer
        springJoint.enabled = false;
        lineRenderer.enabled = false;
        grapplePointObject.GetComponent<SpriteRenderer>().enabled = false;

        grappleColliderTop = false;
        grappleColliderBottom = false;
        turnMultiplier = 0;
        grappleState = false;
        isBroken = false;

        doCalc = true;
    }
}