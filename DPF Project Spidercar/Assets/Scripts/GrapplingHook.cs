using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public GameObject grapplePointObject; //The game object that defines where the grapple hook is
    private SpringJoint2D springJoint; //The component that joins together the car and grapple point by a 'rope' essentially
    private Rigidbody2D rb; //The rigidbody component that calculates physics such as drag, mass and gravity
    private LineRenderer lineRenderer; //The component that draws the rope between the grapple and the car
    private Vector3[] lineRendererPoints;

    float piFloat; //Used for circumference calculations. 

    private float grappleRayLength;
    public LayerMask grappleLayers;
    private Color debugGrappleColour = Color.white;

    float relativePosition;
    float turnMultiplier; //The float used to make the rotationAngle positive or negative, making it turn correctly for left and right

    void Awake()
    {
        piFloat = 3.141592f;
        grappleRayLength = 10f;

        springJoint = GetComponent<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) //When the mouse is pressed down (activating once), find the grapple point
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Finds position of the mouse on the screen and defines a 'transform' variable
            mouseWorldPos.z = 0; //Makes the point conform to the 2D plane
            grapplePointObject.transform.position = mouseWorldPos; //Moves the grapple point to where the mouse was clicked
            RaycastHit2D grapplePointRaycastHit = Physics2D.Raycast(transform.position, grapplePointObject.transform.position, grappleRayLength, grappleLayers);
            Debug.DrawRay(transform.position, mouseWorldPos, debugGrappleColour, grappleLayers);

            Debug.Log("Raycast hit at this location: " + grapplePointRaycastHit.transform.position);

            //grapplePointObject.transform.position = grapplePointRaycastHit.transform.position;

            Vector2 grapplePointDot = grapplePointObject.transform.position - transform.position;
            relativePosition = Vector2.Dot(Vector2.left, grapplePointDot);
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0)) //While the mouse is held down, do the following physics calculations
        {

            if (relativePosition < 0)
            {
                turnMultiplier = 1;
            }

            else
            {
                turnMultiplier = -1;
            }

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
            transform.RotateAround(point, rotationMask, Time.fixedDeltaTime * -rotationAngle);

            lineRendererPoints = new Vector3[]  { grapplePointObject.transform.position , gameObject.transform.position }; //Defines the start and end of the line
            lineRenderer.SetPositions(lineRendererPoints); //Sets the positions to the previously defined positions
        }

        else //When LMB is let go, do the following...
        {
            //Disables the spring joint and line renderer
            springJoint.enabled = false;
            lineRenderer.enabled = false;
        }
    }
}

/* So the calculation code doesn't work as I intend it to... Here are some possible culprits, in order of troubleshooting priority:
             * 1. General logic is incorrect and needs to be sanity checked!
             *      I think this is incredibly likely, as my understanding of physics and applying forces is low, so researching and even breaking down
             *      the problem to make sure the calculation logic is sound will help immensely
             *      
             * 2. vehicleVelocity magnitude is not a way to derive distance correctly!
             *      This also feels likely, as I have no clue if comparing the two numbers is actually giving the result it should mathermatically. I'll
             *      have to do some pen to paper, step by step calculations to find if this is true.
             *      
             * 3. Check all variables to see if there values are consistent and therefore comparable against eachother!
             *      If the top two are fine, scumming through each variable to see what scale and measurements its operating on can help with seeing if
             *      that is what is causing the discrepancies and errors.
             */

/* !!!!ARCHIVE METHODS FOR GRAPPLING HOOK LOGIC!!!!
         * FIRST METHOD: 
         * Correctly points towards the object, but also rotates the object to face downwards in 3D space, causing the sprite to seemiongly dissapear.
         * Also has issue when it gets to the top of the object, as it begins to rotate the object 180 degrees in the y axis and starts to freak out
         * So the second method was pursued
         
        directionToFace = pointToLookTo.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(directionToFace);

        /* SECOND METHOD:
         * Uses a handy rotation mask to isolate just the z axis rotation to allow for both issues to be theoretically solved
         * However, this now prevents all rotations from occuring and makes the object face upwards and move in that direction
         * This makes sense, as when no rotations are exerted onto the car, it will continue in the direction its facing
         * But it doesn't make sense why the z axis rotations are not being applied
         
        directionToFace = pointToLookTo.transform.position - transform.position;
        Vector3 lookAtRotation = Quaternion.LookRotation(directionToFace).eulerAngles;
        transform.rotation = Quaternion.Euler(Vector3.Scale(lookAtRotation, rotationMask)); */