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

    public float grappleRayLength;
    public LayerMask grappleLayers; //Filters only Walls and Poles for grappling raycast
    private Color debugGrappleColour = Color.white;

    void Start()
    {
        grappleRayLength = 20f;
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

        grapplePointRaycastHit = Physics2D.Raycast(grappleOrigin.transform.position, rayDirection);
        Debug.DrawRay(grappleOrigin.transform.position, rayDirection, debugGrappleColour, 1f);

        raycastLengthChecker = Physics2D.Raycast(grappleOrigin.transform.position, rayDirection, grappleRayLength, grappleLayers);
        debugShape.transform.position = raycastLengthChecker.point;

        float hitDistance = Vector2.Distance(grapplePointObject.transform.position, debugShape.transform.position);
        Debug.Log(hitDistance);

        if (hitDistance > 4f)
        {
            return false;
        }

        else
        {
            return true;
        }
    }
}
