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
        if (Input.GetKey(KeyCode.Mouse0))
        {
            LerpPivotFunction(objectToFollow);
        }

        else
        {
            PivotFunctionDuplicate(objectToFollow);
        }
    }

    void PivotFunctionDuplicate(GameObject pivot)
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

    void LerpPivotFunction(GameObject pivot)
    {
        //Conforms object to the position (linear interpolation)
        Vector3 pivotPosition = Vector3.Scale(pivot.transform.position, new Vector3(1, 1, 0));
        Vector3 currentPosition = gameObject.transform.position;
        pivotPosition.z = -10;
        gameObject.transform.position = Vector3.Lerp(currentPosition, pivotPosition, lerpAngle * Time.deltaTime);
        currentPosition.z = -10;
        //Conforms object to the rotation (spherical linear interpolation)
        Quaternion pivotRotation = pivot.transform.rotation;
        Quaternion objectRotation = pivotRotation * Quaternion.Euler(0, 0, 1);
        gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, objectRotation, lerpAngle * Time.deltaTime);
    }
}
