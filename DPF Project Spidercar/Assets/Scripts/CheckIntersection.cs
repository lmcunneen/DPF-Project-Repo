using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIntersection : MonoBehaviour
{
    public GameObject debugShape;

    public bool IsObjectIntersecting(Vector2 positionToCheck)
    {
        debugShape.transform.position = positionToCheck;
        
        var collider = GetComponent<BoxCollider2D>();
        
        if (collider.bounds.Contains(debugShape.transform.position))
        {
            return true;
        }

        return false;
    }
}
