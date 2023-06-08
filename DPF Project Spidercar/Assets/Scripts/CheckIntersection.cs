using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckIntersection : MonoBehaviour
{
    public GameObject grapplePoint; //Only for debugging
    private Collider2D hitCollider;
    public LayerMask layerMask;
    private Vector3 objectSize;
    bool hasStarted;

    private void Start()
    {
        hasStarted = true;
    }

    public bool IsObjectIntersecting()
    {
        objectSize = transform.localScale;
        objectSize.x = Mathf.Abs(objectSize.x);
        objectSize.y = Mathf.Abs(objectSize.y);
        objectSize.z = 100;

        hitCollider = Physics2D.OverlapBox(gameObject.transform.position, objectSize / 2, transform.rotation.z, layerMask);

        Debug.Log(transform.rotation.z);

        if (hitCollider != null)
        {
            hitCollider = null;
            return true;
        }

        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        if (hasStarted)
        {
            Gizmos.DrawWireCube(gameObject.transform.position, objectSize);
        }
    }
}
