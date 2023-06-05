using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDirectionGrappling : MonoBehaviour
{
    public int positionInt;
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 10)
        {
            positionInt = 1;
            Debug.Log("Trigger says above!");
        }

        if (collider.gameObject.layer == 11)
        {
            positionInt = - 1;
            Debug.Log("Trigger says below!");
        }

        else
        {
            positionInt = 0;
            Debug.Log("No trigger so neither!");
        }
    }
}
