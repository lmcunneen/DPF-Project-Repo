using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class VehicleMovement : MonoBehaviour
{
    //VARIABLE DECLARATION//

    [SerializeField] private float maxSpeed = 500;
    Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        float speedDebug = maxSpeed * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {

        }
        
        else
        {
            rigidBody.AddForce(transform.up * maxSpeed * Time.fixedDeltaTime);
        }
    }
}
