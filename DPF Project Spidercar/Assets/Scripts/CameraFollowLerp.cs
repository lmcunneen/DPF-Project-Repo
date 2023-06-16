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

    public GameObject vehicle;
    public GameObject objectToFollow;
    [SerializeField] private float zPos;
    [SerializeField] private float lerpRateInspector;
    [SerializeField] private float transitionRate;
    private float cameraDistance;
    private bool grappleSuccess;
    private bool transitionStart;
    private bool transitionFinish;

    void Start()
    {
        transitionRate = lerpRateInspector;
        transitionStart = true;
        transitionFinish = false;
    }

    void FixedUpdate()
    {
        grappleSuccess = vehicle.GetComponent<GrapplingHook>().grappleState;
        
        if (grappleSuccess)
        {
            transitionStart = true;
            transitionFinish = false;
            LerpPivotFunction(objectToFollow, lerpRateInspector);
        }
        
        else
        {
            if (transitionStart)
            {
                cameraDistance = Vector2.Distance(transform.position, objectToFollow.transform.position);
                //Debug.Log("CAMERA DISTANCE = " +  cameraDistance);
                transitionRate++;
                LerpPivotFunction(objectToFollow, transitionRate);

                if (cameraDistance < 0.75)
                {
                    transitionFinish = true;
                    transitionStart = false;
                    transitionRate = lerpRateInspector;
                }
            }

            if (transitionFinish)
            {
                transitionRate = lerpRateInspector;
                PivotFunctionDuplicate(objectToFollow);
            }
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
        //Debug.Log("Pivot Func Running!");
    }

    void LerpPivotFunction(GameObject pivot, float lerpRate)
    {
        //Conforms object to the position (linear interpolation)
        Vector3 pivotPosition = Vector3.Scale(pivot.transform.position, new Vector3(1, 1, 0));
        Vector3 currentPosition = gameObject.transform.position;
        pivotPosition.z = -10;
        gameObject.transform.position = Vector3.Lerp(currentPosition, pivotPosition, lerpRate * Time.deltaTime);
        currentPosition.z = -10;
        //Conforms object to the rotation (spherical linear interpolation)
        Quaternion pivotRotation = pivot.transform.rotation;
        Quaternion objectRotation = pivotRotation * Quaternion.Euler(0, 0, 1);
        gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, objectRotation, lerpRate * Time.deltaTime);
        //Debug.Log("Lerp Func Running!");
    }
}
