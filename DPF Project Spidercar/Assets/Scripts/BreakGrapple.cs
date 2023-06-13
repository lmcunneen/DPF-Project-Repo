using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakGrapple : MonoBehaviour
{
    /* SCRIPT FUNCTION:
     * Continually raycasts during grapple to determine if it is intersecting with anything...
     * Then breaks the grapple
     * 
     * TO DO:
     *      Research more into raycasting to get a better picture of it
     *      Make sure that is like the raycasting in the stealth game, so its once continous ray rather than hundreds of repeating ones
     */

    public GameObject grapplePointObject;
    public GameObject grappleOrigin;
    RaycastHit2D grapplePointRaycastHit;

    private float grappleRayLength;
    public LayerMask grappleLayers; //Filters only Walls and Poles for grappling raycast
    private Color debugGrappleColour = Color.white;

    void Start()
    {
        grappleRayLength = 10f;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            BreakGrappleFunc();
        }
    }

    void BreakGrappleFunc()
    {
        grapplePointRaycastHit = Physics2D.Raycast(grappleOrigin.transform.position, grapplePointObject.transform.position, grappleRayLength, grappleLayers);
        Debug.DrawRay(grappleOrigin.transform.position, grapplePointObject.transform.position, debugGrappleColour, 1f);

        if (grapplePointRaycastHit == true)
        {
            Debug.Log("Raycast hit at this location: " + grapplePointRaycastHit.transform.position);
        }

        //grapplePointObject.transform.position = grapplePointRaycastHit.transform.position;
    }
}
