using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAndBreakGrapple : MonoBehaviour
{
    /* SCRIPT FUNCTION:
     * Continually raycasts during grapple to determine if it is intersecting with anything...
     * Then breaks the grapple
     */

    public GameObject grapplePointObject;
    public GameObject grappleOrigin;
    public GameObject grappleHook;
    public GameObject debugShape; //Used to show in world-space the raycast landed
    RaycastHit2D hookPathRaycast;
    RaycastHit2D grapplePointRaycastHit;
    RaycastHit2D raycastLengthChecker;
    private Vector3 rayDirection;
    private bool hasValidCollisionReferenced;

    private bool shotGrappleState;

    public float grappleRayLength; //Length of raycast
    public LayerMask grappleLayers; //Filters only Walls and Poles for grappling raycast
    public LayerMask grappleLayersInverse; //Filters only GrapplePoint for checking against another raycast to determine grapple intersections
    public LayerMask hookPathLayers;

    void Start()
    {
        grappleRayLength = 30f;
        shotGrappleState = false;
    }

    public IEnumerator ShootGrapple()
    {
        yield return new WaitForFixedUpdate();

        hasValidCollisionReferenced = grappleHook.GetComponent<HookIntersection>().hasValidCollision; //Checks if the collider is intersecting with anything
        var shootingDistance = Vector2.Distance(grappleHook.transform.position, grappleOrigin.transform.position);

        if (shootingDistance < 30f) //Makes it only allow grapples within 30 units
        {
            if (!hasValidCollisionReferenced)
            {
                shotGrappleState = false;
                
                StartCoroutine(ShootGrapple());
            }

            else
            {
                Debug.Log("The grapple hit!");
                grappleHook.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                shotGrappleState = true;
            }
        }

        else
        {
            Debug.Log("The grapple missed!");
            StopAllCoroutines();
            grappleHook.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            shotGrappleState = false;
        }
    }
    
    public bool CheckGrappleFunc()
    {
        grappleHook.transform.position = grappleOrigin.transform.position;
        grappleHook.GetComponent<SpriteRenderer>().enabled = true;
        grappleHook.transform.right = grapplePointObject.transform.position - grappleOrigin.transform.position; //Makes hook face desired direction
        grappleHook.GetComponent<Rigidbody2D>().AddForce(grappleHook.transform.right * 3000f);

        StartCoroutine(ShootGrapple());

        /*Issue is here currently:
         * The checks are not currently outputting correctly, and dont turn on at all until some unknown criteria is met
         * So the checks constantly updating is probably not happening correctly
         * Best way to solve this is to go back to the dreaded notepad document and go through line by line once again
         * Good luck future me!
         */ 

        if (shotGrappleState)
        {
            rayDirection = (grapplePointObject.transform.position - grappleOrigin.transform.position).normalized;

            grapplePointRaycastHit = Physics2D.Raycast(grappleOrigin.transform.position, rayDirection, grappleRayLength, grappleLayers);

            grapplePointObject.transform.position = grapplePointRaycastHit.point;
            //debugShape.transform.position = grapplePointRaycastHit.point;
            return true;
        }

        else
        {
            return false;
        }

        //grapplePointObject.transform.position = grapplePointRaycastHit.transform.position;
    }

    public bool BreakGrappleFunc()
    {
        rayDirection = (grapplePointObject.transform.position - grappleOrigin.transform.position).normalized;

        grapplePointRaycastHit = Physics2D.Raycast(grappleOrigin.transform.position, rayDirection, grappleRayLength, grappleLayersInverse);

        raycastLengthChecker = Physics2D.Raycast(grappleOrigin.transform.position, rayDirection, grappleRayLength, grappleLayers);
        //debugShape.transform.position = raycastLengthChecker.point;

        float hitDistance = Vector2.Distance(grapplePointRaycastHit.point, raycastLengthChecker.point);
        //Debug.Log("Distance between break: " + hitDistance);

        //return false; //Debug line that stops the check, showing that the intersection is working

        if (hitDistance < 1f)
        {
            return false;
        }

        else
        {
            return true;
        }
    }
}