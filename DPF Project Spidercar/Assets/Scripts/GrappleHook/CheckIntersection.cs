using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckIntersection : MonoBehaviour
{
    /* SCRIPT FUNCTION:
     * Generalist function for checking if something in a layer mask is intersecting with the bounds of the game object collider
     * Function has to be called from other script (not standalone)
     * 
     * This is used to determine the relative position of the grappling hook...
     * And also to find the relative direction (forwards or backwards), both for turn multiplier calculations
     */

    private Collider2D hitCollider;
    private ContactFilter2D contactFilter;
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

        var boxCollider = gameObject.GetComponent<BoxCollider2D>();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = layerMask;
        contactFilter.useTriggers = true;

        hitCollider = Physics2D.OverlapBox(gameObject.transform.position, boxSize, rotationAngle, layerMask);

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
