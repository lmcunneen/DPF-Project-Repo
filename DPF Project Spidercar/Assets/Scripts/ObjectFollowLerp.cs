using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowLerp : MonoBehaviour
{
    public GameObject objectToFollow;
    [SerializeField] private float lerpRateInspector;
    [SerializeField] private float zPos;
    [SerializeField] private bool isStandalone; //Bool that runs the function in update if true. Done only for objects that these functions are NOT called elsewhere in the scripts

    void Update()
    {
        if (isStandalone)
        {
            LerpPivotFunction(objectToFollow, lerpRateInspector, zPos);
        }
    }

    public void LerpPivotFunction(GameObject pivot, float lerpRate, float zPos)
    {
        //Conforms object to the position (linear interpolation)
        Vector3 pivotPosition = Vector3.Scale(pivot.transform.position, new Vector3(1, 1, 0));
        Vector3 currentPosition = gameObject.transform.position;
        pivotPosition.z = zPos;
        gameObject.transform.position = Vector3.Lerp(currentPosition, pivotPosition, lerpRate * Time.deltaTime);
        currentPosition.z = zPos;
        //Conforms object to the rotation (spherical linear interpolation)
        Quaternion pivotRotation = pivot.transform.rotation;
        Quaternion objectRotation = pivotRotation * Quaternion.Euler(0, 0, 1);
        gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, objectRotation, lerpRate * Time.deltaTime);
        //Debug.Log("Lerp Func Running!");
    }
}
