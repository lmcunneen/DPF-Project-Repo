using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject cameraPivot;
    void Update()
    {
        Vector3 pivotPosition = Vector3.Scale(cameraPivot.transform.position, new Vector3(1, 1, 0));
        Vector3 cameraPosition = pivotPosition;
        cameraPosition.z = -10f;
        gameObject.transform.position = cameraPosition;

        Quaternion pivotRotation = cameraPivot.transform.rotation;
        Quaternion cameraRotation = pivotRotation * Quaternion.Euler(0, 0, 1);
        gameObject.transform.rotation = cameraRotation;
    }
}
