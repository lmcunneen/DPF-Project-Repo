using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookIntersection : MonoBehaviour
{
    public bool hasValidCollision;
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            hasValidCollision = true;
        }

        else
        {
            hasValidCollision = false;
        }
    }
}
