using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowLerp : MonoBehaviour
{
    /* SCRIPT FUNCTION:
     * Specialised script that manages the current active ObjectFollow script (normal or lerp) for the camera
     * NOTE: Needs both FollowObject and ObjectFollowLerp attached to Game Object in order to function, with isStandalone set as false in inspector
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
        grappleSuccess = vehicle.GetComponent<GrapplingHook>().grappleStateReference;
        
        if (grappleSuccess)
        {
            transitionStart = true;
            transitionFinish = false;
            gameObject.GetComponent<ObjectFollowLerp>().LerpPivotFunction(objectToFollow, lerpRateInspector, zPos);
        }
        
        else
        {
            if (transitionStart)
            {
                cameraDistance = Vector2.Distance(transform.position, objectToFollow.transform.position);
                //Debug.Log("CAMERA DISTANCE = " +  cameraDistance);
                transitionRate++;
                gameObject.GetComponent<ObjectFollowLerp>().LerpPivotFunction(objectToFollow, transitionRate, zPos);

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
                gameObject.GetComponent<FollowObject>().PivotFunction(objectToFollow, zPos);
            }
        }
    }
}
