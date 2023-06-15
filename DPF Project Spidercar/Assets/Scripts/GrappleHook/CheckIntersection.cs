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
    private Vector3 boxSize;
    private Quaternion boxQuaternion;
    float rotationAngle;

    public bool IsObjectIntersecting()
    {
        boxSize = transform.localScale;
        boxSize.x = Mathf.Abs(boxSize.x);
        boxSize.y = Mathf.Abs(boxSize.y);
        boxSize.z = 10;

        boxQuaternion = transform.rotation;
        rotationAngle = boxQuaternion.eulerAngles.z;

        hitCollider = Physics2D.OverlapBox(gameObject.transform.position, boxSize, rotationAngle, layerMask);

        Debug.Log(rotationAngle + " " + hitCollider);

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
