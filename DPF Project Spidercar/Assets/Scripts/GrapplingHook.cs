using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public GameObject grapplePointInstantiate;
    private SpringJoint2D springJoint;
    private Rigidbody2D rb;

    float piFloat = 3.141592f; //Max precision for float, but it doesn't matter too much

    private Ray2D grapplePointRaycast;
    private RaycastHit2D grapplePointRaycastHit;
    public float grappleRayLength = 10f;
    public LayerMask grappleLayers;

    void Start()
    {
        springJoint = GetComponent<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        grapplePointRaycast = Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics2D.Raycast(grapplePointRaycast, out grapplePointRaycastHit, grappleRayLength, grappleLayers))
            {
                Instantiate(grapplePointInstantiate, grapplePointRaycastHit.point, Quaternion.identity);
                
                //Finds rotation angle for the RotateAround function with ***MATHS***
                float distanceRadius = springJoint.distance; //Finds grapple distance by reading the distance variable on the spring joint
                float vehicleVelocity = rb.velocity.magnitude; //Finds current vehicle velocity
                //Now the calculations are made
                float grappleCircumference = 2 * piFloat * distanceRadius; //Finds circumference of turning circle (distance)
                float fullRotationTime = grappleCircumference / vehicleVelocity; //Finds the time it would take to finish the circle. Unsure of what measurement of time it would refer to...
                float segmentTimePerUpdate = fullRotationTime * Time.fixedDeltaTime; //This doesn't work, as it rounds down the number to small, making the result to miniscule. Return to this and fix it
                float rotationAngle = 360 / (segmentTimePerUpdate * 50); //Should determine the angle, but it doesn't :(

                Debug.Log("VELOCITY: " + vehicleVelocity);
                //Debug.Log(grappleCircumference + " / " + vehicleVelocity + " = " + fullRotationTime);
                //Debug.Log(rotationAngle);

                //Handles Rotation Logic
                springJoint.enabled = true;
                Vector3 rotationMask = new Vector3(0, 0, 1);
                Vector3 point = grapplePointInstantiate.transform.position;
                transform.RotateAround(point, rotationMask, Time.fixedDeltaTime * -rotationAngle);
            }
        }

        else
        {
            springJoint.enabled = false;
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