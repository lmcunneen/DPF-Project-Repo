using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    //VARIABLE DECLARATION//

    [SerializeField] private float forwardSpeed = 500;
    [SerializeField] private float reverseSpeed = 300;
    Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddForce(-transform.up * reverseSpeed * Time.fixedDeltaTime);
        }
        
        else
        {
            rigidBody.AddForce(transform.up * forwardSpeed * Time.fixedDeltaTime);
        }
    }
}
