using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakGrapple : MonoBehaviour
{
    public GameObject grapplePointObject;
    public GameObject grappleOrigin;
    RaycastHit2D grapplePointRaycastHit;
    bool doRaycast;

    private float grappleRayLength;
    public LayerMask grappleLayers; //Filters only Walls and Poles for grappling raycast
    private Color debugGrappleColour = Color.white;

    void Start()
    {
        grappleRayLength = 10f;
        doRaycast = true;
    }

    bool BreakGrappleFunc()
    {
        if (doRaycast == true)
        {
            grapplePointRaycastHit = Physics2D.Raycast(grappleOrigin.transform.position, grapplePointObject.transform.position, grappleRayLength, grappleLayers);
            Debug.DrawRay(grappleOrigin.transform.position, grapplePointObject.transform.position, debugGrappleColour, 1f);
        }

        if (grapplePointRaycastHit == true)
        {
            doRaycast = false;

            Debug.Log("Raycast hit at this location: " + grapplePointRaycastHit.transform.position);
        }

        return grapplePointRaycastHit;

        //grapplePointObject.transform.position = grapplePointRaycastHit.transform.position;
    }
}
