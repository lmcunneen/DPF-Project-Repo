using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowLerp : MonoBehaviour
{
    /* SCRIPT FUNCTION:
     * Specialised script to make the camera follow the player with a little bit of drag, creating a specific game feels
     * This script is essentially a child of the FollowObject script with a few calculation changes
     * 
     * Still needs to be fully implemented...
     */

    public GameObject objectToFollow;
    [SerializeField] private float zPos;
    [SerializeField] private float lerpAngle;

    void Update()
    {
        LerpPivotFunction(objectToFollow);
    }

    void LerpPivotFunction(GameObject pivot)
    {
        //Conforms object to the position (linear interpolation)
        Vector3 pivotPosition = Vector3.Scale(pivot.transform.position, new Vector3(1, 1, 0));
        Vector3 objectPosition = pivotPosition;
        Vector3 currentPosition = gameObject.transform.position;
        objectPosition.z = zPos;
        gameObject.transform.position = Vector2.Lerp(transform.position, objectPosition, lerpAngle);
        currentPosition.z = zPos;
        //Conforms object to the rotation (spherical linear interpolation)
        Quaternion pivotRotation = pivot.transform.rotation;
        Quaternion objectRotation = pivotRotation * Quaternion.Euler(0, 0, 1);
        gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, objectRotation, lerpAngle);
    }
}
