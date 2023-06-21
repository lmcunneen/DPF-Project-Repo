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
     *  - Determining the /TURN DIRECTION/ by factoring in relative position and movement direction
     *  - /CALCULATING AND APPLIES ROTATIONS/ of the car turning around the grapple
     *  - /ENDING GRAPPLE/ under specified circumstances
     */

    public GameObject grapplePointObject;
    public GameObject gameManager;
    private Vector3 mouseWorldPos;

    private bool singleCalculationCheck;

    public bool grappleStateReference;
    private bool isGrappleBrokenReference;

    public GameObject topCollider;
    public GameObject bottomCollider;
    private bool isGrappleAbove;
    private bool isGrappleBelow;
    private bool reverseStateReference;
    private float turnDirectionMultiplier;

    public SpringJoint2D springJoint;
    private Rigidbody2D carRigidBody;
    public LineRenderer lineRenderer; 
    private Vector3[] lineRendererPoints;
    private float piFloat;

    void Awake()
    {
        grapplePointObject.GetComponent<SpriteRenderer>().enabled = false;

        singleCalculationCheck = true;

        isGrappleAbove = false;
        isGrappleBelow = false;
        isGrappleBrokenReference = false;

        springJoint = GetComponent<SpringJoint2D>();
        carRigidBody = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        piFloat = 3.141592f;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0)) //While the mouse is held down, do the following physics calculations
        {
            if (singleCalculationCheck == true)
            {
                mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Finds position of the mouse on the screen and defines a 'transform' variable
                mouseWorldPos.z = 0; //Makes the point conform to the 2D plane
                grapplePointObject.transform.position = mouseWorldPos; //Moves the grapple point to where the mouse was clicked

                grappleStateReference = gameManager.GetComponent<CheckAndBreakGrapple>().CheckGrappleFunc();

                if (grappleStateReference == true)
                {
                    grapplePointObject.GetComponent<SpriteRenderer>().enabled = true;

                    StartCoroutine(CheckTurnNextFixedUpdate());
                }
            }

            if (grappleStateReference == true)
            {
                StartCoroutine(CheckBreakNextFixedUpdate());

                if (isGrappleBrokenReference == false)
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
        isGrappleAbove = topCollider.GetComponent<CheckIntersection>().IsObjectIntersecting(); //Checks top collider

        if (isGrappleAbove == false) //If the top collider returned false, then check the bottom collider
        {
            isGrappleBelow = bottomCollider.GetComponent<CheckIntersection>().IsObjectIntersecting();
        }

        else //If it's true, we know it will be false, so skip the check
        {
            isGrappleBelow = false;
        }

        Debug.Log("Top state: " + isGrappleAbove);
        Debug.Log("Bottom state: " + isGrappleBelow);

        //Finds current input for breaking
        reverseStateReference = gameObject.GetComponent<VehicleMovement>().breakState;

        FindTurnMultiplier();

        Debug.Log("Turn calculation is done!");

        singleCalculationCheck = false;
    }

    void FindTurnMultiplier()
    {
        if (isGrappleAbove && !reverseStateReference || isGrappleBelow && reverseStateReference)
        {
            turnDirectionMultiplier = 1;
        }

        if (isGrappleBelow && !reverseStateReference || isGrappleAbove && reverseStateReference)
        {
            turnDirectionMultiplier = -1;
        }

        if (isGrappleAbove && isGrappleBelow)
        {
            turnDirectionMultiplier = 0;
        }

        else if (isGrappleAbove && isGrappleBelow)
        {
            Debug.LogWarning("Both checks returned true!!! Debug ASAP!");
        }
    }

    IEnumerator CheckBreakNextFixedUpdate()
    {
        yield return new WaitForFixedUpdate();
        isGrappleBrokenReference = gameManager.GetComponent<CheckAndBreakGrapple>().BreakGrappleFunc();
    }    

    void TurnCalculations()
    {
        //Finds rotation angle for the RotateAround function with ***MATHS***
        float distanceRadius = springJoint.distance; //Finds grapple distance by reading the distance variable on the spring joint
        float vehicleVelocity = carRigidBody.velocity.magnitude; //Finds current vehicle velocity
        //Now the calculations are made
        float grappleCircumference = 2 * piFloat * distanceRadius; //Finds circumference of turning circle (distance)
        float fullRotationTime = grappleCircumference / vehicleVelocity; //Finds the time it would take to finish the circle. Measures in units per second
        float segmentsPerRotation = fullRotationTime / Time.fixedDeltaTime; //Finds how many segments are travelled during whole rotation
        float rotationAngle = (360 / segmentsPerRotation) * turnDirectionMultiplier; //Determines the angle of each segment and filters it through the turnMultiplier

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

        isGrappleAbove = false;
        isGrappleBelow = false;
        turnDirectionMultiplier = 0;
        grappleStateReference = false;
        isGrappleBrokenReference = false;

        singleCalculationCheck = true;
    }
}