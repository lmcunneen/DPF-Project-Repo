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
    private int hitColliderInt;
    private Collider2D[] colliders = new Collider2D[1];
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

        //hitCollider = Physics2D.OverlapBox(gameObject.transform.position, boxSize, rotationAngle, layerMask);
        hitColliderInt = Physics2D.OverlapCollider(boxCollider, contactFilter, colliders);

        //Debug.Log(gameObject.name + " " + collider.gameObject.name);

        if (colliders[0].gameObject.layer == 3)
        {
            Array.Clear(colliders, 0, colliders.Length);
            return true;
        }

        else
        {
            Array.Clear(colliders, 0, colliders.Length);
            return false;
        }
    }
}
