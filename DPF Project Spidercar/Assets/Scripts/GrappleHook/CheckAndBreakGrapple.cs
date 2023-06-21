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

    public float grappleRayLength; //Length of raycast
    public LayerMask grappleLayers; //Filters only Walls and Poles for grappling raycast
    public LayerMask grappleLayersInverse; //Filters only GrapplePoint for checking against another raycast to determine grapple intersections
    public LayerMask hookPathLayers;

    void Start()
    {
        grappleRayLength = 30f;
    }

    public bool CheckGrappleFunc()
    {
        rayDirection = (grapplePointObject.transform.position - grappleOrigin.transform.position).normalized;

        hookPathRaycast = Physics2D.Raycast(grappleOrigin.transform.position, rayDirection, grappleRayLength, hookPathLayers);

        debugShape.transform.position = hookPathRaycast.point;

        grapplePointRaycastHit = Physics2D.Raycast(grappleOrigin.transform.position, rayDirection, grappleRayLength, grappleLayers);

        //Debug.DrawRay(grappleOrigin.transform.position, rayDirection * grappleRayLength, debugGrappleColour, 1f);

        if (grapplePointRaycastHit == true)
        {
            Debug.Log("Raycast has hit!");
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

    private void Update()
    {
        grappleHook.transform.position = Vector2.Lerp(grappleOrigin.transform.position, hookPathRaycast.point, Time.deltaTime);
    }
}