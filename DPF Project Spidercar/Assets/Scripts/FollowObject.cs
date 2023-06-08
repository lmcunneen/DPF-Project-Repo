using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject objectToFollow;

    void Update()
    {
        PivotFunction(objectToFollow);
    }

    void PivotFunction(GameObject pivot)
    {
        Vector3 pivotPosition = Vector3.Scale(pivot.transform.position, new Vector3(1, 1, 0));
        Vector3 objectPosition = pivotPosition;
        objectPosition.z = -10f;
        gameObject.transform.position = objectPosition;

        Quaternion pivotRotation = pivot.transform.rotation;
        Quaternion objectRotation = pivotRotation * Quaternion.Euler(0, 0, 1);
        gameObject.transform.rotation = objectRotation;
    }
}
