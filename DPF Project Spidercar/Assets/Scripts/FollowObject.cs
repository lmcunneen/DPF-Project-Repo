using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    /* SCRIPT FUNCTION:
     * Generalist script that makes one object conform to the position and rotation of another
     * 
     * Primarily used for grappling hook calculations as many objects used for calculations are children of the car sprite...
     * Making their transforms relative to the car, thereby making some calculations difficult/impossible (with current systems)
     */

    public GameObject objectToFollow;
    [SerializeField] private float zPos;
    [SerializeField] private bool isStandalone; //Bool that runs the function in update if true. Done only for objects that these functions are NOT called elsewhere in the scripts

    void Update()
    {
        if (isStandalone)
        {
            PivotFunction(objectToFollow, zPos);
        }
    }

    public void PivotFunction(GameObject pivot, float zPos)
    {
        //Conforms object to the position
        Vector3 pivotPosition = Vector3.Scale(pivot.transform.position, new Vector3(1, 1, 0));
        pivotPosition.z = zPos;
        gameObject.transform.position = pivotPosition;
        //Conforms object to the rotation
        Quaternion pivotRotation = pivot.transform.rotation;
        Quaternion objectRotation = pivotRotation * Quaternion.Euler(0, 0, 1);
        gameObject.transform.rotation = objectRotation;
    }
}
