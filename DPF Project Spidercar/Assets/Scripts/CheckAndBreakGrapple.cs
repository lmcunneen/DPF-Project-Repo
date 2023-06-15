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
    public GameObject debugShape;
    RaycastHit2D grapplePointRaycastHit;
    RaycastHit2D raycastLengthChecker;
    private Vector3 rayDirection;
    private Vector3 lengthRayDirection;

    public float grappleRayLength;
    public LayerMask grappleLayers; //Filters only Walls and Poles for grappling raycast
    public LayerMask grappleLayersInverse;
    private Color debugGrappleColour = Color.red;

    void Start()
    {
        grappleRayLength = 30f;
    }

    public bool CheckGrappleFunc()
    {
        rayDirection = (grapplePointObject.transform.position - grappleOrigin.transform.position).normalized;

        grapplePointRaycastHit = Physics2D.Raycast(grappleOrigin.transform.position, rayDirection, grappleRayLength, grappleLayers);

        //Debug.DrawRay(grappleOrigin.transform.position, rayDirection * grappleRayLength, debugGrappleColour, 1f);

        if (grapplePointRaycastHit == true)
        {
            Debug.Log("Raycast has hit!");
            grapplePointObject.transform.position = grapplePointRaycastHit.point;
            debugShape.transform.position = grapplePointRaycastHit.point;
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
        rayDirection = (grapplePointObject.transform.position - grappleOrigin.transform.position);

        grapplePointRaycastHit = Physics2D.Raycast(grappleOrigin.transform.position, rayDirection, grappleRayLength, grappleLayersInverse);
        Debug.DrawRay(grappleOrigin.transform.position, rayDirection, debugGrappleColour, 1f);

        raycastLengthChecker = Physics2D.Raycast(grappleOrigin.transform.position, rayDirection, grappleRayLength, grappleLayers);
        debugShape.transform.position = raycastLengthChecker.point;

        float hitDistance = Vector2.Distance(grapplePointRaycastHit.point, raycastLengthChecker.point);
        Debug.Log("Distance between break: " + hitDistance);

        return false; //Debug line that stops the check, showing that the intersection is working

        if (hitDistance < 1.5f)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    /* ARCHIVE METHODS:
     *  rayDirection = (grapplePointObject.transform.position - grappleOrigin.transform.position).normalized;

        grapplePointRaycastHit = Physics2D.Raycast(grappleOrigin.transform.position, rayDirection, grappleRayLength, grappleLayersInverse);
        debugShape.transform.position = grapplePointRaycastHit.point;

        lengthRayDirection = (grapplePointObject.transform.position - debugShape.transform.position);

        raycastLengthChecker = Physics2D.Raycast(debugShape.transform.position, lengthRayDirection);

        Debug.Log("Distance between break: " + raycastLengthChecker.distance);

        if (raycastLengthChecker.distance < 1.5f)
        {
            return false;
        }

        else
        {
            return true;
        }
    * ----------------------------------------------------------------------
    */

}