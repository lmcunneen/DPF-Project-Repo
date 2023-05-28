using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class VehicleMovement : MonoBehaviour
{
    //VARIABLE DECLARATION//

    public float maxSpeed = 200;
    Vector2 movement;
    Rigidbody2D rigidBody;
    [SerializeField] private float currentSpeed;

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

        }
        
        else
        {
            rigidBody.AddForce(transform.up * maxSpeed * Time.deltaTime);
        }
    }
}
