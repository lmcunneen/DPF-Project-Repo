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
    RaycastHit2D grapplePointRaycastHit;
    private Vector3 rayDirection;

    public float grappleRayLength;
    public LayerMask grappleLayers; //Filters only Walls and Poles for grappling raycast
    //private Color debugGrappleColour = Color.white;

    void Start()
    {
        grappleRayLength = 20f;
    }

    public bool CheckGrappleFunc()
    {
        rayDirection = (grapplePointObject.transform.position - grappleOrigin.transform.position).normalized;
        
        /* Notes on Raycast:
         * The ray length is set at 100 so that it will always hit an object no matter how far
         * The ACTUAL length check is given in the grappleRayLength value, as the grappling hook has an if statement checking if its...
         * under a certain range.
         * This is done so the hit check and length check are seperate to allow the hit check to only occur once
         */ 
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
}
