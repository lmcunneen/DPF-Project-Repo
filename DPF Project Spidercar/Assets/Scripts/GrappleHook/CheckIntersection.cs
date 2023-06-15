using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckIntersection : MonoBehaviour
{
    /* SCRIPT FUNCTION:
     * Specialised function for checking if the grappling hook point is intersecting with the script parent
     * This is used to determine the relative position of the grappling hook for turn multiplier calculations
     */

    private Collider2D hitCollider;
    public LayerMask layerMask;
    private Vector3 objectSize;
    float rotationAngle;

    private void Start()
    {

    }

    public bool IsObjectIntersecting()
    {
        objectSize = transform.localScale;
        objectSize.x = Mathf.Abs(objectSize.x);
        objectSize.y = Mathf.Abs(objectSize.y);
        objectSize.z = 100;

        Quaternion ugh = transform.rotation;
        rotationAngle = ugh.eulerAngles.z;

        hitCollider = Physics2D.OverlapBox(gameObject.transform.position, objectSize, rotationAngle, layerMask); // had /2 in objectSize

        //Debug.Log(rotationAngle + " " + hitCollider);

        if (hitCollider != null)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
